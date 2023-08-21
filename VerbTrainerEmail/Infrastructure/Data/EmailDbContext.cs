using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VerbTrainerEmail.Domain.Entities.Email;
using VerbTrainerEmail.Domain.Entities.User;

namespace VerbTrainerEmail.Infrastructure.Data
{
    public class EmailDbContext : DbContext
    {

        public EmailDbContext(DbContextOptions options) : base(options)
        {
        }

        // should it reference main service?
        public DbSet<User> Users { get; set; }
        public DbSet<Email> Emails { get; set; }
    }
}

