using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Exceptions
{
    public class ConsultaException : Exception
    {
        public List<string> erros = new List<string> { };

        public ConsultaException(string message, List<string> erros) : base(message)
        {
            this.erros = erros;
        }

        public ConsultaException(List<string> erros) : base("Ocorreram erros")
        {
            this.erros = erros;
        }

        public ConsultaException(string message) : base(message)
        {
            this.erros.Add(message);
        }

    }
}
