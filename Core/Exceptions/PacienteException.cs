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

        public PacienteException(List<string> erros) : base("Ocorreram erros")
        {
            this.erros = erros;
        }

        public PacienteException(string message) : base(message)
        {
            
        }

    }
}
