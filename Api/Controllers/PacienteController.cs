using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Core.Services;
using Core.ViewModels;
using Core.Models;
using System.Threading.Tasks;

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
        public async Task<Paciente> GetPacienteAsync(int id)
        {
            var paciente = await service.GetPacienteAsync(id);

            return paciente;
        }

        [Route("")]
        [HttpGet]
        public async Task<IEnumerable<Paciente>> GetPacientesAsync()
        {
            var pacientes = await service.GetPacientesAsync();
            return pacientes;
        }
        [Route("")]
        [HttpPost]
        public async Task<ResultViewModel> NewPacienteAsync([FromBody] Paciente paciente)
        {

            return await service.NewPacienteAsync(paciente);

        }
        [Route("")]
        [HttpPut]
        public async Task<ResultViewModel> UpdatePacienteAsync([FromBody] Paciente paciente)
        {

            return await service.UpdatePacienteAsync(paciente);

        }
    }
}