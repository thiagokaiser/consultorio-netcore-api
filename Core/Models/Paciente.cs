using System;

namespace Core.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Sexo { get; set; }
        public DateTime DtNascimento { get; set; }
        public string Prontuario { get; set; }
        public string Convenio { get; set; }


    }
}