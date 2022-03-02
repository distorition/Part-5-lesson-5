using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Part_3_Lesson_4.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Part_3_Lesson_4.Controllers
{
    [Authorize]
    [ApiController]
    [Route("controller")]
    public class UserController : Controller
    {
        private readonly UserRepositories userRepositories;
        private readonly UserService userService;
        public UserController(UserRepositories user)
        {
            userRepositories = user;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res=await userRepositories.GEt();
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User newUser)
        {
            await userRepositories.Add(newUser);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] User user)
        {
           
            await userRepositories.Ubdate(user);
            return NoContent();
        
        }

        [HttpDelete]
        [Route("{userid}")]
        public async Task<IActionResult> Delet(int id)
        {
            await userRepositories.Delete(id);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("authentication")]
        public IActionResult Authitication([FromQuery] string name , string pas)
        {
            string token =userService.Authentication(name, pas);
            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest(new { message = "Name or Pas is incorect" });
            }
            return Ok(token);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
