using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Consultorio.Core.Services;
using Consultorio.Core.ViewModels;
using Consultorio.Core.Models;

namespace ConsultorioDotnet.Controllers
{    
    [ApiController]
    [Route("v1/paciente")]
    public class PacienteController : ControllerBase
    {

        private readonly PacienteService _service;

        public PacienteController(PacienteService service)
        {
            _service = service;
        }


        [Route("{id:int}")]
        [HttpGet]        
        public Paciente GetPaciente(int id)
        {
            var paciente = _service.GetPaciente(id);
            return paciente;
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<Paciente> GetPacientes()
        {
            var pacientes = _service.GetPacientes();
            return pacientes;
        }


    }
}