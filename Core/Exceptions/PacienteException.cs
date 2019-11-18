using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Exceptions
{
    public class PacienteException : Exception
    {

        public List<string> erros = new List<string> { };

        public PacienteException(string message, List<string> erros) : base(message)
        {
            this.erros = erros;
        }

    }
}
