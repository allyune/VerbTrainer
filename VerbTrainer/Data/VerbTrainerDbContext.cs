using System;
using Microsoft.EntityFrameworkCore;
using VerbTrainer.Models.Domain;

namespace VerbTrainer.Data
{
	public class VerbTrainerDbContext : DbContext
	{

		public VerbTrainerDbContext(DbContextOptions options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Conjugation>()
				.HasKey(c => new { c.VerbId, c.TenseId });
		}

		public DbSet<Binyan> Binyanim { get; set; }
		public DbSet<Verb> Verbs { get; set; }
		public DbSet<Tense> Tenses { get; set; }
		public DbSet<Conjugation> Conjugations { get; set; }


	}
}

