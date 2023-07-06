using System;
using Microsoft.EntityFrameworkCore;

namespace VerbTrainer.Data
{
	public class VerbTrainerDbContext: DbContext
	{

        public VerbTrainerDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}

