using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Design;
using VerbTrainerUser.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace VerbTrainerUser.Infrastructure.Data
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
            modelBuilder.Entity<RecoveryToken>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserStatus)
                .WithMany()
                .HasForeignKey(u => u.StatusCode);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RevokedAccessToken> RevokedAccessTokens { get; set; }
        public DbSet<RevokedRefreshToken> RevokedRefreshTokens { get; set; }
        public DbSet<RecoveryToken> RecoveryTokens { get; set; }
        public DbSet<UserStatus> UserStatuses { get; set; }
	}
}

