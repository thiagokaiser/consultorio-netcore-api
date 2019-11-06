using Core.Interfaces;
using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
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
        public async Task<IEnumerable<Consulta>> GetConsultasAsync()
        {
            return await repository.GetConsultasAsync();
        }

        public async Task<ResultViewModel> NewConsultaAsync(Consulta consulta)
        {
            List<ErroViewModel> erros = await Task.Run(() => ValidaCampos(consulta));

            if (erros.Count > 0)
            {
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Ocorreram erros.",
                    Data = erros
                };                    
            }
            return await repository.NewConsultaAsync(consulta);
        }
        public async Task<ResultViewModel> UpdateConsultaAsync(Consulta consulta)
        {
            List<ErroViewModel> erros = await Task.Run(() => ValidaCampos(consulta));
            
            if (erros.Count > 0)
            {
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Ocorreram erros.",
                    Data = erros
                };
            }
            return await repository.UpdateConsultaAsync(consulta);
        }

        private List<ErroViewModel> ValidaCampos(Consulta consulta)
        {
            List<ErroViewModel> erros = new List<ErroViewModel>();

            if (consulta.Cid == "")
            {
                erros.Add(new ErroViewModel
                {
                    Campo = "Cid",
                    Erro = "Cid obrigatório",
                    Solucao = "Favor informar um codigo válido"
                });

            }
            if (consulta.PacienteId == 0)
            {
                erros.Add(new ErroViewModel
                {
                    Campo = "PacienteId",
                    Erro = "Paciente obrigatório",
                    Solucao = "Favor informar um paciente válido"
                });
            }
            return erros;
        }
    }
}
