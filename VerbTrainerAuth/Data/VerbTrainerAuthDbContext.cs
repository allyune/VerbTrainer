using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Design;
using VerbTrainerAuth.Models.Domain;
using VerbTrainerSharedModels.Models.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerbTrainerAuth.Data
{
	public class VerbTrainerAuthDbContext : DbContext
	{

		public VerbTrainerAuthDbContext(DbContextOptions options) : base(options)
		{

		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<RevokedAccessToken>()
                .HasIndex(t => t.Token)
                .IsUnique();

            modelBuilder.Entity<RevokedRefreshToken>()
                .HasIndex(t => t.Token)
                .IsUnique();

        }

        [NotMapped]
        public DbSet<User> Users { get; set; }

        public DbSet<RevokedAccessToken> RevokedAccessTokens { get; set; }
        public DbSet<RevokedRefreshToken> RevokedRefreshTokens { get; set; }
	}
}

