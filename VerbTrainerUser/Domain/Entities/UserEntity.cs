using System;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;
using VerbTrainerUser.Domain.Base;
using VerbTrainerUser.Domain.ValueObjects;
using VerbTrainerUser.Domain.Exceptions;

namespace VerbTrainerUser.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public Email Email { get; private set; }
        public UserStatus Status { get; private set; }
        public Password Password { get; set; }
        public DateTime? LastLogin { get; private set; }

        private UserEntity(int id, string firstName, string? lastName,
                           Email email, UserStatus status,
                           Password password, DateTime? lastLogin)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Status = status;
            LastLogin = lastLogin;
            Password = password;
        }

        // Create a brand new entity
        public static UserEntity CreateNew(
            int id,
            string email,
            string passwordRawString,
            string firstName,
            UserStatus status = UserStatus.Free)
        {
            string? lastName = null;
            Email emailObj = Email.CreateNew(email);
            Password passwordObj = Password.CreateNew(passwordRawString);
            DateTime? lastLogin = null;

            return new UserEntity(
                id,
                firstName,
                lastName,
                emailObj,
                status,
                passwordObj,
                lastLogin);
        }

        // For mapping from models (entity is already stored in db
        public static UserEntity CreateNew(
            string email,
            UserStatus status,
            string passwordHashString,
            string salt,
            int id,
            DateTime? lastLogin,
            string firstName,
            string? lastName)
        {

            Email emailObj = Email.CreateNew(email);
            Password passwordObj = Password.CreateNew(passwordHashString, salt);

            return new UserEntity(
                id,
                firstName,
                lastName,
                emailObj,
                status,
                passwordObj,
                lastLogin);
        }

        public void UpdatePassword(string newPassword)
        {
            Password.UpdateValue(newPassword);
        }

        public bool VerifyPassword(string passwordRawString)
        {
            return Password.CompareTo(passwordRawString);
        }

    }
}

