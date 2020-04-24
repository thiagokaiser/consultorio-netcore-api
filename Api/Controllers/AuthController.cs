using Api.Models.Identity;
using Api.Security;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.Controllers
{    
    [Route("v1/security")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly JwtSettings jwtSettings;

        public AuthController(SignInManager<User> signInManager,
                              UserManager<User> userManager,
                              IOptions<JwtSettings> jwtSettings)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.jwtSettings = jwtSettings.Value;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> NewUser([FromBody] RegisterUserViewModel registeruser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            var user = new User
            {
                UserName = registeruser.Email,
                Email = registeruser.Email,
                EmailConfirmed = true,
                FirstName = registeruser.firstName,
                LastName = registeruser.lastName
            };

            var result = await userManager.CreateAsync(user, registeruser.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            await signInManager.SignInAsync(user, false);

            var token = await GerarJwt(registeruser.Email);
            return Ok(new { accessToken = token });

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            var result = await signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);
            if (result.Succeeded)
            {
                var token = await GerarJwt(loginUser.Email);
                return Ok(new { accessToken = token });
            }
            return BadRequest(new Exception("Usuário e senha inválidos"));
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }

        private async Task<string> GerarJwt(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(await userManager.GetClaimsAsync(user));
            identityClaims.AddClaim(new Claim("firstName", user.FirstName));
            identityClaims.AddClaim(new Claim("lastName", user.LastName));
            identityClaims.AddClaim(new Claim("email", user.Email));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Issuer = jwtSettings.Emissor,
                Audience = jwtSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(jwtSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        [Authorize]
        [HttpPost("perfil")]
        public async Task<IActionResult> LoadUserByEmail([FromBody] UserViewModel user)
        {            
            try
            {                
                return Ok(await userManager.FindByEmailAsync(user.Email));
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
            var result = await signInManager.PasswordSignInAsync(userForm.Email, userForm.Password, false, true);
            if (!result.Succeeded)
            {
                return BadRequest(new Exception("Senha inválida"));                
            }
            try
            {                
                var user = await userManager.FindByEmailAsync(userForm.Email);

                user.FirstName = userForm.FirstName;
                user.LastName = userForm.LastName;
                user.DtNascimento = userForm.DtNascimento;
                user.Descricao = userForm.Descricao;
                user.Cidade = userForm.Cidade;
                user.Estado = userForm.Estado;

                var userUpdated = await userManager.UpdateAsync(user);

                var newToken = await GerarJwt(user.Email);

                return Ok(new { accessToken = newToken });
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }            
        }
        [Authorize]
        [HttpPut("senha")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassViewModel userForm)
        {
            if (userForm.NewPassword != userForm.ConfirmNewPassword)
            {
                return BadRequest(new Exception("Senhas não conferem"));
            }
            try
            {
                var user = await userManager.FindByEmailAsync(userForm.Email);

                var changePass = userManager.ChangePasswordAsync(user, userForm.OldPassword, userForm.NewPassword);

                if (changePass.Result.Succeeded)
                {
                    return Ok(changePass.Result);
                }
                return BadRequest(changePass.Result.Errors);
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
