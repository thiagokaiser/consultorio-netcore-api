using Core.Services;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("v1/security")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService service;

        public AuthController(AuthService service)
        {
            this.service = service;
        }        

        [HttpPost("registrar")]
        public async Task<IActionResult> NewUser([FromBody] RegisterUserViewModel registeruser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            try
            {
                return Ok(new { accessToken = await service.NewUser(registeruser)});
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            try
            {
                return Ok(new { accessToken = await service.Login(loginUser)});
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }            
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await service.Logout();
            return Ok();
        }        

        [Authorize]
        [HttpPost("perfil")]
        public async Task<IActionResult> LoadUserByEmail([FromBody] UserViewModel user)
        {            
            try
            {                
                return Ok(await service.LoadUserByEmail(user));
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }            
        }

        [Authorize]
        [HttpPut("perfil")]
        public async Task<IActionResult> UpdateUser([FromBody] UserViewModel userForm)
        {
            try
            {
                return Ok(new { accessToken = await service.UpdateUser(userForm)});
            }
            catch (Exception ex)
            {
                return BadRequest(ex);                
            }            
        }

        [Authorize]
        [HttpPut("senha")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassViewModel userForm)
        {
            try
            {
                await service.ChangePassword(userForm);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
