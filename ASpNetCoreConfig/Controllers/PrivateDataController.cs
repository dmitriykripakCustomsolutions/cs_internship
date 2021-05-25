using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.ComputerService;
using BusinessLayer.Models;
using BusinessLayer.UserService;
using IBoxer.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASpNetCoreConfig.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PrivateDataController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IComputerService _computerService;

        public PrivateDataController(IUserService userService, IComputerService computerService)
        {
            _userService = userService;
            _computerService = computerService;
        }


        [HttpGet]
        [Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "First secred data string", "Second secred data string" };
        }

        [HttpGet]
        //[Authorize]
        [Route("get-users")]
        public List<User> GetAllUsers()
        {
            var manufacturers = _computerService.GetComputerManufacturers();
            return _userService.GetAll();
        }
    }
}