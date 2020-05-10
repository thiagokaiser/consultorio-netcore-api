using Core.Models.Identity;
using Core.ViewModels;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepositoryAuth
    {        
        Task<string> NewUser(RegisterUserViewModel registeruser);
        Task<string> Login(LoginUserViewModel loginUser);
        Task Logout();
        Task<User> LoadUserByEmail(UserViewModel user);
        Task<string> UpdateUser(UserViewModel userForm);
        Task ChangePassword(ChangePassViewModel userForm);
    }
}
