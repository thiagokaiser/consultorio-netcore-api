using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.ViewModels;

namespace Core.Interfaces
{
    public interface IRepositoryPaciente
    {
        Task<Paciente> GetPacienteAsync(int Id);
        Task<IEnumerable<Paciente>> GetPacientesAsync();
        Task<ResultViewModel> NewPacienteAsync(Paciente paciente);
        Task<ResultViewModel> UpdatePacienteAsync(Paciente paciente);
        Task<ResultViewModel> DeletePacienteAsync(int id);
    }
}
