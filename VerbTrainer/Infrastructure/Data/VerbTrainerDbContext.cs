﻿using System;
using Microsoft.EntityFrameworkCore;
using VerbTrainer.Infrastructure.Data.Models.Hebrew;

namespace VerbTrainer.Infrastructure.Data
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

            modelBuilder.Entity<Deck>()
                .HasMany(d => d.DeckVerbs)
                .WithOne(dv => dv.Deck)
                .HasForeignKey(dv => dv.DeckId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeckVerb>()
                .HasOne(dv => dv.Verb)
                .WithOne()
                .HasForeignKey<DeckVerb>(dv => dv.VerbId);

            modelBuilder.Entity<Verb>()
                .HasMany(v => v.DeckVerbs)
                .WithOne(dv => dv.Verb)
                .HasForeignKey(dv => dv.VerbId)
                .OnDelete(DeleteBehavior.Cascade);
        }

		public DbSet<Binyan> Binyanim { get; set; }
		public DbSet<Verb> Verbs { get; set; }
		public DbSet<Tense> Tenses { get; set; }
		public DbSet<Conjugation> Conjugations { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<DeckVerb> DeckVerbs { get; set; }
    }
}

