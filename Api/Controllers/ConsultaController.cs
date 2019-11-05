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
        public Consulta GetConsulta(int id)
        {
            return service.GetConsulta(id);
        }

        [HttpGet]
        public IEnumerable<Consulta> GetConsultas()
        {
            return service.GetConsultas();
        }

        [HttpPost]
        public ResultViewModel NewConsulta([FromBody] Consulta consulta)
        {
            return service.NewConsulta(consulta);
        }

        
    }
}
