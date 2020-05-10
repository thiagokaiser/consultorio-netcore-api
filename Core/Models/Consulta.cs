using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("consulta")]
    public class Consulta
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("pacienteid")]
        public int PacienteId { get; set; }
        [Column("conduta")]
        public string Conduta { get; set; }
        [Column("diagnostico")]
        public string Diagnostico { get; set; }
        [Column("cid")]
        public string Cid { get; set; }
        [Column("dtconsulta")]
        public DateTime DtConsulta { get; set; }
        [Column("exames")]
        public string Exames { get; set; }
        [Column("retorno")]
        public DateTime Retorno { get; set; }
        public Paciente Paciente { get; set; }
    }
}
