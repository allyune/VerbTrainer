using System;
using System.Security.Cryptography;
using System.Text.Json;
using VerbTrainerAuth.Application.Exceptions;
using VerbTrainerAuth.Application.Services.Mapping;
using VerbTrainerAuth.Application.Services.User;
using VerbTrainerAuth.Domain.Entities;
using VerbTrainerAuth.Domain.Interfaces;
using VerbTrainerAuth.DTOs;
using VerbTrainerAuth.Infrastructure.Data.Models;
using VerbTrainerAuth.Infrastructure.Messaging.Producer;

namespace VerbTrainerAuth.Application.ResetPassword
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

            SendPasswordResetEmail(_recoveryTokenMapper.EntityToDto(token));

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

        private void SendPasswordResetEmail(PasswordRecoveryRequestDto requestDto)
        {
            //TODO: exception handling? wait for acknowledgment/message sent response
            var json = JsonSerializer.Serialize(requestDto);
            _messagingProducer.SendMessage(json, "password_reminder");
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

