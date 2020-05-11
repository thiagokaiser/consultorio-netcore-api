using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Exceptions
{
    public class UserException : Exception
    {
        public List<string> erros = new List<string>();

        public UserException(string message, List<string> erros) : base(message)
        {
            this.erros = erros;
        }

        public UserException(List<string> erros) : base("Ocorreram erros")
        {
            this.erros = erros;
        }

        public UserException(IEnumerable<IdentityError> erros)
        {
            this.erros = erros.Select(x => x.Description).ToList();
        }

        public UserException(string message) : base(message)
        {
            this.erros.Add(message);
        }
    }
}
