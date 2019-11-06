using System.Collections.Generic;
using Core.ViewModels;
using Core.Interfaces;
using Core.Models;
using System.Threading.Tasks;

namespace Core.Services
{
    public class PacienteService
    {
        private IRepositoryPaciente repository;
        public PacienteService(IRepositoryPaciente repository)
        {
            this.repository = repository;
        }
        public async Task<IEnumerable<Paciente>> GetPacientesAsync()
        {
            var pacientes = await repository.GetPacientesAsync();
            return pacientes;

        }
        public async Task<Paciente> GetPacienteAsync(int id)
        {
            return await repository.GetPacienteAsync(id);
        }
        public async Task<ResultViewModel> NewPacienteAsync(Paciente paciente)
        {
            if (paciente.Nome == "")
            {
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Nome inválido",
                    Data = paciente
                };
            }

            return await repository.NewPacienteAsync(paciente);
        }
        public async Task<ResultViewModel> UpdatePacienteAsync(Paciente paciente)
        {
            return await repository.UpdatePacienteAsync(paciente);
        }
    }
}