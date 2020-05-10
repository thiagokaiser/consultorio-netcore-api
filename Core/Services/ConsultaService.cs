using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ConsultaService
    {
        private IRepositoryConsulta repository;
        public ConsultaService(IRepositoryConsulta repository)
        {
            this.repository = repository;
        }
        public async Task<Consulta> GetConsultaAsync(int id)
        {
            return await repository.GetConsultaAsync(id);
        }
        public async Task<ListConsultaViewModel> GetConsultasAsync(Pager pager)
        {
            return await repository.GetConsultasAsync(pager);
        }
        public async Task<ListConsultaViewModel> GetConsultasPacienteAsync(int id, Pager pager)
        {
            return await repository.GetConsultasPacienteAsync(id, pager);
        }

        public async Task<ResultViewModel> NewConsultaAsync(Consulta consulta)
        {
            List<string> erros = ValidaCampos(consulta);

            if (erros.Count > 0)
            {
                throw new ConsultaException("Erro", erros);
            }
            
            return await repository.NewConsultaAsync(consulta);        
            
        }
        public async Task<ResultViewModel> UpdateConsultaAsync(Consulta consulta)
        {
            List<string> erros = ValidaCampos(consulta);
            
            if (erros.Count > 0)
            {
                throw new ConsultaException("Erro", erros);
            }
            return await repository.UpdateConsultaAsync(consulta);
        }
        
        public async Task<ResultViewModel> DeleteConsultaAsync(int id)
        {
            return await repository.DeleteConsultaAsync(id);
        }

        private List<string> ValidaCampos(Consulta consulta)
        {
            List<string> erros = new List<string>();

            if (consulta.Cid == "")
            {
                erros.Add("Cid obrigatório");

            }
            if (consulta.PacienteId == 0)
            {
                erros.Add("Paciente obrigatório");
            }
            return erros;
        }
    }
}
