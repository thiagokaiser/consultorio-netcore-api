using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;
using Core.ViewModels;

namespace Core.Interfaces
{
    public interface IRepositoryPaciente
    {
        Paciente GetPaciente(int Id);
        IEnumerable<Paciente> GetPacientes();
        ResultViewModel NewPaciente(Paciente paciente);
        ResultViewModel UpdatePaciente(Paciente paciente);
    }
}
