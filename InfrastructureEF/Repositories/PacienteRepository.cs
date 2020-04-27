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
    public class PacienteRepository : IRepositoryPaciente
    {
        private readonly DataContext dataContext;

        public PacienteRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<ResultViewModel> DeletePacienteAsync(int id)
        {
            try
            {
                var paciente = await dataContext.Paciente.SingleAsync(p => p.Id == id);
                dataContext.Remove(paciente);
                await dataContext.SaveChangesAsync();
                return new ResultViewModel
                {
                    Success = true,
                    Message = "Paciente eliminado com sucesso.",
                    Data = paciente
                };
            }
            catch (Exception ex)
            {
                throw new PacienteException("Erro", new List<string> { ex.Message });
            }
        }

        public async Task<Paciente> GetPacienteAsync(int id)
        {
            try
            {
                var paciente = await dataContext.Paciente.SingleAsync(p => p.Id == id);
                return paciente;
            }
            catch (Exception ex)
            {
                throw new PacienteException("Erro", new List<string> { ex.Message });
            }
        }

        public async Task<ListPacienteViewModel> GetPacientesAsync(Pager pager)
        {
            var pacientes = dataContext.Paciente.Where(x =>
                    EF.Functions.Like(x.Nome, pager.SearchText) ||
                    EF.Functions.Like(x.Prontuario, pager.SearchText) ||
                    EF.Functions.Like(x.Convenio, pager.SearchText)).Skip(pager.OffSet).Take(pager.PageSize).ToList();
            var pacientescount = dataContext.Paciente.Where(x =>
                    EF.Functions.Like(x.Nome, pager.SearchText) ||
                    EF.Functions.Like(x.Prontuario, pager.SearchText) ||
                    EF.Functions.Like(x.Convenio, pager.SearchText)).ToList();

            return new ListPacienteViewModel()
            {
                count = pacientescount.Count(),
                pacientes = pacientes
            };
        }

        public async Task<ResultViewModel> NewPacienteAsync(Paciente paciente)
        {
            try
            {
                dataContext.Add(paciente);
                await dataContext.SaveChangesAsync();
                return new ResultViewModel
                {
                    Success = true,
                    Message = "Paciente adicionado com sucesso.",
                    Data = paciente
                };

            }
            catch (Exception ex)
            {
                throw new PacienteException("Erro", new List<string> { ex.Message });
            }
        }

        public async Task<ResultViewModel> UpdatePacienteAsync(Paciente paciente)
        {
            try
            {
                dataContext.Update(paciente);
                await dataContext.SaveChangesAsync();
                return new ResultViewModel
                {
                    Success = true,
                    Message = "Paciente alterado com sucesso.",
                    Data = paciente
                };

            }
            catch (Exception ex)
            {
                throw new PacienteException("Erro", new List<string> { ex.Message });
            }
        }
    }
}
