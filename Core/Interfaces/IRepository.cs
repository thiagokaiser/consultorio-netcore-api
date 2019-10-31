using System;
using System.Collections.Generic;
using System.Text;
using Consultorio.Core.Models;

namespace Core.Interfaces
{
    public interface IRepository
    {
        Paciente GetPaciente(int Id);
        IEnumerable<Paciente> GetPacientes();
    }
}
