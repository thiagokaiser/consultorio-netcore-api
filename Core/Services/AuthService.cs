using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Core.Models.Identity;
using Core.Security;
using Core.ViewModels;

namespace Core.Services
{
    public class AuthService
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly JwtSettings jwtSettings;

        public AuthService(SignInManager<User> signInManager,
                           UserManager<User> userManager,
                           IOptions<JwtSettings> jwtSettings)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.jwtSettings = jwtSettings.Value;
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
        
        public async Task<string> NewUser(RegisterUserViewModel registeruser)
        {
            var user = new User
            {
                UserName = registeruser.Email,
                Email = registeruser.Email,
                EmailConfirmed = true,
                FirstName = registeruser.firstName,
                LastName = registeruser.lastName
            };

            var result = await userManager.CreateAsync(user, registeruser.Password);

            if (!result.Succeeded) throw new Exception();                    
            //result.Errors;
            await signInManager.SignInAsync(user, false);
            
            return await GerarJwt(registeruser.Email);
        }
                
        public async Task<string> Login(LoginUserViewModel loginUser)
        {
            var result = await signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {                
                return await GerarJwt(loginUser.Email);
            }
            throw new Exception("Usuário e senha inválidos");
        }
                
        public async Task Logout()
        {
            await signInManager.SignOutAsync();            
        }
                
        public async Task<User> LoadUserByEmail(UserViewModel user)
        {
            try
            {
                return await userManager.FindByEmailAsync(user.Email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
                
        public async Task<string> UpdateUser(UserViewModel userForm)
        {
            var result = await signInManager.PasswordSignInAsync(userForm.Email, userForm.Password, false, true);
            if (!result.Succeeded)
            {
                throw new Exception("Senha inválida");
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

                return newToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
                
        public async Task ChangePassword(ChangePassViewModel userForm)
        {
            if (userForm.NewPassword != userForm.ConfirmNewPassword)
            {
                throw new Exception("Senhas não conferem");
            }
            try
            {
                var user = await userManager.FindByEmailAsync(userForm.Email);
                var changePass = userManager.ChangePasswordAsync(user, userForm.OldPassword, userForm.NewPassword);

                if (!changePass.Result.Succeeded)
                {
                    throw new Exception();
                    //return Ok(changePass.Result);
                }

                //return BadRequest(changePass.Result.Errors);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
