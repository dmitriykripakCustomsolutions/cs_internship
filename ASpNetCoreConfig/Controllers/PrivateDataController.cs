using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Models;
using BusinessLayer.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASpNetCoreConfig.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PrivateDataController : ControllerBase
    {
        private readonly IUserService _userService;

        public PrivateDataController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        [Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "First secred data string", "Second secred data string" };
        }

        [HttpGet]
        [Authorize]
        [Route("get-users")]
        public List<User> GetAllUsers()
        {
            return _userService.GetAll();
        }
    }
}