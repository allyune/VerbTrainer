using System;
using Microsoft.EntityFrameworkCore;
using VerbTrainerAuth.Models.Domain;

namespace VerbTrainerAuth.Data
{
	public class VerbTrainerAuthDbContext : DbContext
	{

		public VerbTrainerAuthDbContext(DbContextOptions options) : base(options)
		{

		}

		public DbSet<User> Users { get; set; }
		public DbSet<RevokedAccessToken> RevokedAccessTokens { get; set; }
		public DbSet<RevokedRefreshToken> RevokedRefreshTokens { get; set; }
	}
}

