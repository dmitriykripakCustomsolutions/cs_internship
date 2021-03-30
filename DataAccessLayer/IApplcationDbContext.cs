using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
	public interface IApplcationDbContext
	{
		public DbSet<User> Users { get; set; }
		int SaveChanges();
	}
}
