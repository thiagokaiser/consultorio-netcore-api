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
using Core.Interfaces;

namespace Core.Services
{
    public class AuthService
    {
        private readonly IRepositoryAuth repository;

        public AuthService(IRepositoryAuth repository)
        {
            this.repository = repository;
        }

        public async Task<string> NewUser(RegisterUserViewModel registeruser)
        {
            return await repository.NewUser(registeruser);
        }
                
        public async Task<string> Login(LoginUserViewModel loginUser)
        {
            return await repository.Login(loginUser);
        }
                
        public async Task Logout()
        {
            await repository.Logout();
        }
                
        public async Task<User> LoadUserByEmail(UserViewModel user)
        {
            return await repository.LoadUserByEmail(user);
        }
                
        public async Task<string> UpdateUser(UserViewModel userForm)
        {
            return await repository.UpdateUser(userForm);
        }
                
        public async Task ChangePassword(ChangePassViewModel userForm)
        {
            await repository.ChangePassword(userForm);            
        }
    }
}
