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

        [HttpPost]
        public async Task<ResultViewModel> NewConsultaAsync([FromBody] Consulta consulta)
        {
            return await service.NewConsultaAsync(consulta);
        }

        [HttpPut]
        public async Task<ResultViewModel> UpdateConsultaAsync([FromBody] Consulta consulta)
        {
            return await service.UpdateConsultaAsync(consulta);
        }


    }
}
