using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Core.ViewModels;
using Core.Interfaces;
using Core.Models;
using Core.Exceptions;

namespace Core.Services
{
    public class PacienteService
    {
        private IRepositoryPaciente repository;

        public PacienteService(IRepositoryPaciente repository)
        {
            this.repository = repository;
        }

        public async Task<ListPacienteViewModel> GetPacientesAsync(Pager pager)
        {
            var pacientes = await repository.GetPacientesAsync(pager);
            return pacientes;

        }

        public async Task<Paciente> GetPacienteAsync(int id)
        {
            return await repository.GetPacienteAsync(id);
        }

        public async Task<ResultViewModel> NewPacienteAsync(Paciente paciente)
        {            
            if (string.IsNullOrEmpty(paciente.Nome))
            {
                throw new PacienteException("Nome inválido");
            }

            return await repository.NewPacienteAsync(paciente);
        }

        public async Task<ResultViewModel> UpdatePacienteAsync(Paciente paciente)
        {
            return await repository.UpdatePacienteAsync(paciente);
        }

        public async Task<ResultViewModel> DeletePacienteAsync(int id)
        {
            return await repository.DeletePacienteAsync(id);
        }
    }
}