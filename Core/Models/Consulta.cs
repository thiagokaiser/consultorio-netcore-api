using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Consulta
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; }
        public string Conduta { get; set; }
        public string Diagnostico { get; set; }
        public string Cid { get; set; }
        public DateTime DtConsulta { get; set; }
        public string Exames { get; set; }
        public DateTime Retorno { get; set; }



    }
}
