using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ASpNetCoreConfig.Models;
using BusinessLayer.ComputerService;
using IBoxer.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ASpNetCoreConfig.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IComputerService _computerService;

        public AuthController(IComputerService computerService)
        {
            _computerService = computerService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginModel user)
        {
            var manufacturers = _computerService.GetComputerManufacturers();

            if (user == null)
            {
                return BadRequest("Invalid data");
            }
            if(user.UserName == "johndoe" && user.Password == "12345")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokenOptions = new JwtSecurityToken(
                    issuer: "https://localhost:44382",
                    audience: "https://localhost:44382",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                    );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}