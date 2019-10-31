using System.Collections.Generic;
using Consultorio.Core.ViewModels;
using Core.Interfaces;
using Consultorio.Core.Models;

namespace Consultorio.Core.Services
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
    }
}