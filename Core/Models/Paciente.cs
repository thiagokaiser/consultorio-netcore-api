using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("paciente")]
    public class Paciente
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("nome")]
        public string Nome { get; set; }
        [Column("sobrenome")]
        public string Sobrenome { get; set; }
        [Column("sexo")]
        public string Sexo { get; set; }
        [Column("dtnascimento")]
        public DateTime DtNascimento { get; set; }
        [Column("prontuario")]
        public string Prontuario { get; set; }
        [Column("convenio")]
        public string Convenio { get; set; }
    }
}