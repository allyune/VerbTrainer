﻿using System;
namespace VerbTrainerUser.Infrastructure.Data.Models
{
	public class UserStatus : BaseAuthModel
	{
		public int UserStatusId { get; set; }
		public string UserStatusName { get; set; }
    }
}

