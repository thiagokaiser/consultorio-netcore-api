using Microsoft.AspNetCore.Identity;
using System;

namespace Core.Models.Identity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DtNascimento { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Descricao { get; set; }
        public DateTime DtRegistro { get; set; }
    }
}
