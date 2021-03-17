﻿using BusinessLayer.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer.UserService
{
	public class UserService : IUserService
	{
		private readonly IApplcationDbContext _dbContext;
		public UserService(IApplcationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public List<User> GetAll()
		{
			var users = _dbContext.Users.ToList();
			var userResult = new List<User>();

			foreach (var user in users)
			{
				var mappedUser = new User { FirstName = user.FirstName, LastName = user.LastName };
				userResult.Add(mappedUser);

			}

			return userResult;
		}
	}
}
