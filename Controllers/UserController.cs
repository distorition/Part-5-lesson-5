using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            REsponseToken token =userService.Authentication(name, pas);//получаем наш токен 
            if (token is null)
            {
                return BadRequest(new { message = "Name or Pas is incorect" });
            }
            SetTokenCucki(token.Refreshtoken);  //добавляем его в куки 
            return Ok(token);
        }

        [Authorize]
        [HttpPost("refresh-token")]
        public IActionResult Refresh()
        {
            string oldRefreshToken = Request.Cookies["refresh-token"];
            string newrefreshToken = userService.REfreshToken(oldRefreshToken);
            if (string.IsNullOrWhiteSpace(newrefreshToken))
            {
                return Unauthorized(new { message = "invalid token" });
            }
            SetTokenCucki(newrefreshToken);
            return Ok(newrefreshToken);
        }


        private void SetTokenCucki(string token)//метод для доавбления в куки 
        {
            var cockeiOption = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cockeiOption);
        }
    }
}
