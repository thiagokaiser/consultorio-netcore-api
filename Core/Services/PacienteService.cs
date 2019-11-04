using System.Collections.Generic;
using Core.ViewModels;
using Core.Interfaces;
using Core.Models;


namespace Core.Services
{
    public class PacienteService
    {
        private IRepository repository;

        public PacienteService(IRepository repository) 
        {
            this.repository = repository;
        }
        public IEnumerable<Paciente> GetPacientes()
        {
            var pacientes = repository.GetPacientes();
            return pacientes;           

        }
        public Paciente GetPaciente(int id)
        {
            return repository.GetPaciente(id);
        }
        public ResultViewModel NewPaciente(Paciente paciente)
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

            return repository.NewPaciente(paciente);
        }
        public ResultViewModel UpdatePaciente(Paciente paciente)
        {
            return repository.UpdatePaciente(paciente);
        }
    }
}