using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Core.Services;
using Core.ViewModels;
using Core.Models;
using Core.Exceptions;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> GetPacienteAsync(int id)
        {
            try
            {
                var paciente = await service.GetPacienteAsync(id);
                return Ok(paciente);
            }
            catch (PacienteException ex)
            {
                return BadRequest(ex);
            }            
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetPacientesAsync()
        {
            try
            {
                var pacientes = await service.GetPacientesAsync();
                return Ok(pacientes);
            }
            catch (PacienteException ex)
            {
                return BadRequest(ex);                
            }
            
        }
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> NewPacienteAsync([FromBody] Paciente paciente)
        {
            try
            {
                var retorno = await service.NewPacienteAsync(paciente);
                return Ok(retorno);
            }
            catch (PacienteException ex)
            {
                return BadRequest(ex);
            }
        }
        [Route("{id:int}")]
        [HttpPut]
        public async Task<IActionResult> UpdatePacienteAsync([FromBody] Paciente paciente)
        {
            try
            {
                var retorno = await service.UpdatePacienteAsync(paciente);
                return Ok(retorno);
            }
            catch (PacienteException ex)
            {
                return BadRequest(ex);                
            }
            
        }
        [Route("{id:int}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePacienteAsync(int id)
        {
            try
            {
                ResultViewModel retorno = await service.DeletePacienteAsync(id);
                return Ok(retorno);
            }
            catch(PacienteException ex)
            {
                return BadRequest(ex);
            }                
            
        }

    }
}