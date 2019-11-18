using System.Collections.Generic;
using Core.ViewModels;
using Core.Interfaces;
using Core.Models;
using System.Threading.Tasks;
using System.Linq;
using Core.Exceptions;
using System;

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
            var erros = new List<string>();
            if (string.IsNullOrEmpty(paciente.Nome))
            {
                erros.Add("Nome invalido");                
            }
            /*if (string.IsNullOrEmpty(paciente.Sobrenome))
            {
                erros.Add("Sobrenome invalido");

            }*/
            if (erros.Any())
            {
                throw new PacienteException("Ocorreram erros ao adicionar Paciente.", erros);
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