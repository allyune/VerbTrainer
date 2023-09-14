﻿using System;
namespace VerbTrainerUser.Application.RegisterUser
{
	public interface IRegisterUserHandler
	{
        public Task<bool> RegisterUser(
            string email, string password, string firstName);
    }
}

