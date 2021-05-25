using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.ComputerService;
using BusinessLayer.Lifecycle;
using BusinessLayer.Models;
using IBoxer.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASpNetCoreConfig.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ComputerController : ControllerBase
	{
		private readonly IComputerService _computerService;

		private readonly ITransientInterface _transientInterface_1;
		private readonly ITransientInterface _transientInterface_2;

		private readonly IScopedInterface _scopedInterface_1;
		private readonly IScopedInterface _scopedInterface_2;

		private readonly ISingletonInterface _singletoneInterface_1;
		private readonly ISingletonInterface _singletoneInterface_2;

		public ComputerController(IApplicationDbContext applcationDbContext,
			IComputerService computerService,
			ITransientInterface transientInterface_1,
			ITransientInterface transientInterface_2,
			IScopedInterface scopedInterface_1,
			IScopedInterface scopedInterface_2,
			ISingletonInterface singletonInterface_1,
			ISingletonInterface singletonInterface_2
			)
		{
			_computerService = computerService;

			_transientInterface_1 = transientInterface_1;
			_transientInterface_2 = transientInterface_2;

			_scopedInterface_1 = scopedInterface_1;
			_scopedInterface_2 = scopedInterface_2;

			_singletoneInterface_1 = singletonInterface_1;
			_singletoneInterface_2 = singletonInterface_2;
		}

		[HttpGet]
		[Route("display-lifetime")]
		public ActionResult<object> DisplayLifetime()
		{
			var _scopedGuid_1 = _scopedInterface_1.GetGuid();
			var _scopedGuid_2 = _scopedInterface_2.GetGuid();

			var _transientGuid_1 = _transientInterface_1.GetGuid();
			var _transientGuid_2 = _transientInterface_2.GetGuid();

			var _singletoneGuid_1 = _singletoneInterface_1.GetGuid();
			var _singletoneGuid_2 = _singletoneInterface_2.GetGuid();

			return new ActionResult<object>(
				new { _scopedGuid_1 , _scopedGuid_2, _transientGuid_1, _transientGuid_2, _singletoneGuid_1, _singletoneGuid_2 }
				);
		}

		[HttpPost]
		public ActionResult<string> AddManufacturer(ComputerManufacturerDto computerManufacturer)
		{
			if (computerManufacturer.ManufacturerName != null)
			{
				//return _computerService.AddManufacturer(computerManufacturer);
			}

			return BadRequest("You've tried to add an invalid data!");
		}

		[HttpGet]
		public ActionResult<List<ComputerManufacturerDto>> GetManufacturers()
		{
			return _computerService.GetComputerManufacturers();
		}

		[HttpDelete]
		public ActionResult<bool> RemoveManufacturer(string id)
		{
			//var isSuccess = _computerService.DeleteManufacturer(id);

			//if (isSuccess)
			//	return Ok();

			return BadRequest($"A manufacturer with {id} id doesn't exist!");
		}
	}
}