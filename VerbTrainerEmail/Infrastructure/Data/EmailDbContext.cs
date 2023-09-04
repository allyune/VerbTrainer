using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Design;
using VerbTrainerEmail.Infrastructure.Data.Models;
using Email = VerbTrainerEmail.Infrastructure.Data.Models.Email;

namespace VerbTrainerEmail.Infrastructure.Data
{
    public class EmailDbContext : DbContext
    {

        public EmailDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<HistoryRow>().ToTable("sharedmigrationshistory");

            modelBuilder.Entity<Email>()
                .HasMany(e => e.Attachments)
                .WithOne(a => a.Email)
                .HasForeignKey(a => a.EmailId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmailAttachment>()
                .HasOne(a => a.Email)
                .WithMany(e => e.Attachments)
                .HasForeignKey(a => a.EmailId);
        }

        public DbSet<Email> Emails { get; set; }
    }
}

