using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.ViewModels;
using InfrastructureEF.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureEF.Repositories
{
    public class ConsultaRepository : IRepositoryConsulta
    {
        private readonly DataContext dataContext;

        public ConsultaRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<ResultViewModel> DeleteConsultaAsync(int id)
        {
            try
            {
                var consulta = await dataContext.Consulta.SingleAsync(p => p.Id == id);
                dataContext.Remove(consulta);
                await dataContext.SaveChangesAsync();
                return new ResultViewModel
                {
                    Success = true,
                    Message = "Consulta eliminada com sucesso.",
                    Data = consulta
                };
            }
            catch (Exception ex)
            {
                throw new ConsultaException("Erro", new List<string> { ex.Message });
            }            
        }

        public async Task<Consulta> GetConsultaAsync(int id)
        {
            try
            {
                var consulta = await dataContext.Consulta.SingleAsync(p => p.Id == id);
                return consulta;
            }
            catch (Exception ex)
            {
                throw new ConsultaException("Erro", new List<string> { ex.Message });                
            }            
        }

        public async Task<ListConsultaViewModel> GetConsultasAsync(Pager pager)
        {
            var consultas = dataContext.Consulta.AsNoTracking().ToList();
            return new ListConsultaViewModel()
            {
                count = 100,
                consultas = consultas
            };
        }

        public async Task<ListConsultaViewModel> GetConsultasPacienteAsync(int id, Pager pager)
        {
            var consultas = dataContext.Consulta.AsNoTracking().ToList();
            return new ListConsultaViewModel()
            {
                count = 100,
                consultas = consultas
            };            
        }

        public async Task<ResultViewModel> NewConsultaAsync(Consulta consulta)
        {
            try
            {
                dataContext.Add(consulta);
                await dataContext.SaveChangesAsync();
                return new ResultViewModel
                {
                    Success = true,
                    Message = "Consulta adicionada com sucesso.",
                    Data = consulta
                };

            }
            catch (Exception ex)
            {
                throw new ConsultaException("Erro", new List<string> { ex.Message });                
            }            
        }

        public async Task<ResultViewModel> UpdateConsultaAsync(Consulta consulta)
        {
            try
            {
                dataContext.Update(consulta);
                await dataContext.SaveChangesAsync();
                return new ResultViewModel
                {
                    Success = true,
                    Message = "Consulta alterada com sucesso.",
                    Data = consulta
                };

            }
            catch (Exception ex)
            {
                throw new ConsultaException("Erro", new List<string> { ex.Message });
            }
        }
    }
}
