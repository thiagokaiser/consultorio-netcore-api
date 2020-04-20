using Core.Models;
using System.Collections.Generic;

namespace Core.ViewModels
{
    public class ListPacienteViewModel
    {
        public int count { get; set; }
        public IEnumerable<Paciente> pacientes { get; set; }

    }
}