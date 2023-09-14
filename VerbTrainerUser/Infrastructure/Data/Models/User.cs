using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ValueObjects = VerbTrainerUser.Domain.ValueObjects;

namespace VerbTrainerUser.Infrastructure.Data.Models
{
    public class User : BaseAuthModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new int Id { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }

        public int StatusCode { get; set; }
        public UserStatus UserStatus {get; set;}

        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime? LastLogin { get; set; }
        //public ICollection<Deck>? Decks { get; set; }

        private User(string firstName,
                     string? lastName,
                     int statusCode,
                     string email,
                     string password,
                     string salt,
                     DateTime? lastLogin)
        {
            FirstName = firstName;
            LastName = lastName;
            StatusCode = statusCode;
            Email = email;
            Password = password;
            Salt = salt;
            LastLogin = lastLogin;
        }

        public static User CreateNew(string email,
                                     ValueObjects.UserStatus status,
                                     string password,
                                     string salt,
                                     DateTime? lastLogin,
                                     string firstName,
                                     string? lastName = null)
        {
            int statusCode = (int)status;
            return new User(firstName,
                            lastName,
                            statusCode,
                            email,
                            password,
                            salt,
                            lastLogin);
        }

    }

 }

