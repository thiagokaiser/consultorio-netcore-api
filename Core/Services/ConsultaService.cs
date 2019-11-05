using Core.Interfaces;
using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
    public class ConsultaService : IRepositoryConsulta
    {
        private IRepositoryConsulta repository;
        public ConsultaService(IRepositoryConsulta repository)
        {
            this.repository = repository;
        }
        public Consulta GetConsulta(int id)
        {
            return repository.GetConsulta(id);
        }
        public IEnumerable<Consulta> GetConsultas()
        {
            return repository.GetConsultas();
        }

        public ResultViewModel NewConsulta(Consulta consulta)
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
            if(consulta.PacienteId == 0)
            {
                erros.Add(new ErroViewModel
                {
                    Campo = "PacienteId",
                    Erro = "Paciente obrigatório",
                    Solucao = "Favor informar um paciente válido"
                });
            }

            if(erros.Count > 0)
            {
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Ocorreram erros.",
                    Data = erros
                };                    
            }
            return repository.NewConsulta(consulta);
        }

    }
}
