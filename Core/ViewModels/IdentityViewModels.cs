using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ViewModels
{
    public class RegisterUserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

    }
    public class LoginUserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
