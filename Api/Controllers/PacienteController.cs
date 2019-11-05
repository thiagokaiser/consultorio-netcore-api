using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Core.Services;
using Core.ViewModels;
using Core.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("v1/paciente")]
    public class PacienteController : ControllerBase
    {

        private readonly PacienteService service;

        public PacienteController(PacienteService service)
        {
            this.service = service;
        }

        [Route("{id:int}")]
        [HttpGet]
        public Paciente GetPaciente(int id)
        {
            var paciente = service.GetPaciente(id);

            return paciente;
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<Paciente> GetPacientes()
        {
            var pacientes = service.GetPacientes();
            return pacientes;
        }
        [Route("")]
        [HttpPost]
        public ResultViewModel NewPaciente([FromBody] Paciente paciente)
        {

            return service.NewPaciente(paciente);

        }
        [Route("")]
        [HttpPut]
        public ResultViewModel UpdatePaciente([FromBody] Paciente paciente)
        {

            return service.UpdatePaciente(paciente);

        }
    }
}