using BusinessLayer.Models;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer.ComputerService
{
	public class AdvancedComputerService: IComputerService
	{
		private readonly IApplicationDbContext _dbContext;
		private readonly ILogger _logger;

		public AdvancedComputerService(IApplicationDbContext dbContext, ILogger logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public string AddManufacturer(ComputerManufacturerDto computerManufacturer)
		{
			throw new NotImplementedException();
		}

		public bool DeleteManufacturer(string id)
		{
			throw new NotImplementedException();
		}

		public List<ComputerManufacturerDto> GetComputerManufacturers()
		{
			_logger.LogInformation("AdvancedComputerService: GetComputerManufacturers");
			var manufacturers = _dbContext.ComputerManufacturers.Include(c => c.ComputerModels).ToList();
			var resultList = new List<ComputerManufacturerDto>();

			foreach (var manufacturer in manufacturers)
			{
				resultList.Add(new ComputerManufacturerDto
				{
					ManufacturerName = manufacturer.ManufacturerName,
					ComputerModels = manufacturer?.ComputerModels?.Select(model => new ComputerModelDto
					{
						ModelName = model.ModelName
					}
					).ToList()
				});
			}

			return resultList;
		}
	}
}
