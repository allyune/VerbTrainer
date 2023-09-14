using System;
using System.Security.Cryptography;
using System.Text.Json;
using VerbTrainerUser.Application.Exceptions;
using VerbTrainerUser.Application.Services.Mapping;
using VerbTrainerUser.Application.Services.User;
using VerbTrainerUser.Domain.Entities;
using VerbTrainerUser.Domain.Interfaces;
using VerbTrainerUser.DTOs;
using VerbTrainerUser.Infrastructure.Data.Models;
using VerbTrainerUser.Infrastructure.Messaging.Producer;

namespace VerbTrainerUser.Application.ResetPassword
{
	public class ResetPasswordHandler : IResetPasswordHandler
    {
        private readonly IAsyncRecoveryTokenRepository _recoveryTokenRepository;
        private readonly IUserService _userService;
        private readonly IMessagingProducer _messagingProducer;
        private readonly IRecoveryTokenMapper _recoveryTokenMapper;
        private readonly ILogger<ResetPasswordHandler> _logger;

        public ResetPasswordHandler(IAsyncRecoveryTokenRepository tokenRepository,
                                        IUserService userService,
                                        IMessagingProducer producer,
                                        IRecoveryTokenMapper mapper,
                                        ILogger<ResetPasswordHandler> logger)
        {
            _recoveryTokenRepository = tokenRepository;
            _userService = userService;
            _logger = logger;
            _messagingProducer = producer;
            _recoveryTokenMapper = mapper;
        }

        public async Task ResetPassword(string email)
        {
            bool userExists = await _userService.CheckUserExists(email);
            if (!userExists)
            {
                throw new UserDoesNotExistException(
                    $"User with email {email} does not exist");
            }

            // checking if user already has active token
            UserEntity user = await _userService.getUserInfoByEmail(email);

            RecoveryTokenEntity? activeToken = await GetActiveTokenByUserId(user.Id);

            if (activeToken is not null)
            {
                activeToken.SetUsed(true);
            }
          
            RecoveryTokenEntity token = RecoveryTokenEntity.CreateNew(user.Id);
            await SaveRecoveryToken(token);

            SendPasswordResetEmail(_recoveryTokenMapper.EntityToDto(user, token));

        }

        public async Task<bool> ValidateRecoveryToken(string token, int userId)
        {
            RecoveryToken? res = await _recoveryTokenRepository
                                        .GetAsync(t => t.Token == token &&
                                                       t.UserId == userId);
            if (res != null &&
                res.Validity < DateTime.Now &&
                res.Used != true)
            {
                return true;
            }
            return false;
        }

        private async Task SaveRecoveryToken(RecoveryTokenEntity tokenEntity)
        {
            RecoveryToken tokenModel = _recoveryTokenMapper.EntityToModel(tokenEntity);
            await _recoveryTokenRepository.AddAsync(tokenModel);
            await _recoveryTokenRepository.SaveChangesAsync();
        }

        private void SendPasswordResetEmail(PasswordResetRequestDto requestDto)
        {
            //TODO: exception handling? wait for acknowledgment/message sent response
            _messagingProducer.SendMessage(requestDto, "password_reminder");
        }

        private async Task<RecoveryTokenEntity?> GetActiveTokenByUserId(int? userId)
        {
            RecoveryToken? token = await _recoveryTokenRepository.GetAsync(
                t => t.UserId == userId &&
                t.Validity > DateTime.UtcNow &&
                !t.Used);

            if (token is not null)
            {
                return _recoveryTokenMapper.ModelToEntity(token);
            }

            return null;
        }
    }
}

