using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerbTrainer.Infrastructure.Data.Models.Hebrew
{

	public class Tense : BaseVerbTrainerModel
    {
		public int Id { get; set; }
		public string Name { get; set; }
	}
}


