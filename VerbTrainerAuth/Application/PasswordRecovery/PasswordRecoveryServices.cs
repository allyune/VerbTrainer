using System;
using System.Security.Cryptography;
using System.Text.Json;
using VerbTrainerAuth.Application.Mapping;
using VerbTrainerAuth.Application.User;
using VerbTrainerAuth.Domain.Entities;
using VerbTrainerAuth.Domain.Interfaces;
using VerbTrainerAuth.DTOs;
using VerbTrainerAuth.Infrastructure.Data.Models;
using VerbTrainerAuth.Infrastructure.Messaging.Producer;
using Models = VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerAuth.Application.PasswordRecovery
{
	public class PasswordRecoveryServices : IPasswordRecoveryServices
	{
        private readonly IAsyncRecoveryTokenRepository _recoveryTokenRepository;
        private readonly IAsyncUserRepository _userRepository;
        private readonly IMessagingProducer _messagingProducer;
        private readonly IRecoveryTokenMapper _recoveryTokenMapper;
        private readonly ILogger<UserServices> _logger;

        public PasswordRecoveryServices(IAsyncRecoveryTokenRepository tokenRepository,
                                        IAsyncUserRepository userRepository,
                                        IMessagingProducer producer,
                                        IRecoveryTokenMapper mapper,
                                        ILogger<UserServices> logger)
        {
            _recoveryTokenRepository = tokenRepository;
            _userRepository = userRepository;
            _logger = logger;
            _messagingProducer = producer;
            _recoveryTokenMapper = mapper;
        }

        public RecoveryTokenEntity CreateToken(Models.User user)
        {
            const int tokenSize = 64;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            byte[] tokenBytes = RandomNumberGenerator.GetBytes(tokenSize);
            string tokenString = Convert.ToHexString(tokenBytes);
            return RecoveryTokenEntity
                   .CreateNew(tokenString,
                              user.Id,
                              DateTime.Now.AddMinutes(60),
                              false);
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

        public async Task SaveRecoveryToken(RecoveryTokenEntity tokenEntity)
        {
            RecoveryToken tokenModel = _recoveryTokenMapper.EntityToModel(tokenEntity);
            await _recoveryTokenRepository.AddAsync(tokenModel);
            await _recoveryTokenRepository.SaveChangesAsync();
        }

        public void RequestRecoveryEmail(PasswordRecoveryRequestDto requestDto)
        {
            //TODO: exception handling? wait for acknowledgment/message sent response
            var json = JsonSerializer.Serialize(requestDto);
            _messagingProducer.SendMessage(json, "password_reminder");
        }
    }
}

