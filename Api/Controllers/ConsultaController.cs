using Core.Exceptions;
using Core.Models;
using Core.Services;
using Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("v1/consulta")]
    public class ConsultaController : ControllerBase
    {
        private readonly ConsultaService service;

        public ConsultaController(ConsultaService service)
        {
            this.service = service;
        }

        [Route("{id:int}")]
        [HttpGet]
        public async Task<Consulta> GetConsultaAsync(int id)
        {
            return await service.GetConsultaAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<Consulta>> GetConsultasAsync()
        {
            return await service.GetConsultasAsync();
        }
        [Route("paciente/{id:int}")]
        [HttpGet]
        public async Task<IEnumerable<Consulta>> GetConsultasPacienteAsync(int id)
        {
            return await service.GetConsultasPacienteAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> NewConsultaAsync([FromBody] Consulta consulta)
        {
            try
            {                
                return Ok(await service.NewConsultaAsync(consulta));
            }
            catch (ConsultaException ex)
            {
                return BadRequest(ex);
            }
            
        }

        [Route("{id:int}")]
        [HttpPut]
        public async Task<IActionResult> UpdateConsultaAsync([FromBody] Consulta consulta)
        {
            try
            {
                return Ok(await service.UpdateConsultaAsync(consulta));

            }
            catch (ConsultaException ex)
            {
                return BadRequest(ex);
            }            
        }

        [Route("{id:int}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteConsultaAsync(int id)
        {
            try
            {
                await service.DeletePacienteAsync(id);
                return Ok();
            }
            catch (ConsultaException ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
