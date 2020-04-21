using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepositoryConsulta
    {
        Task<Consulta> GetConsultaAsync(int id);
        Task<ListConsultaViewModel> GetConsultasAsync(Pager pager);
        Task<ListConsultaViewModel> GetConsultasPacienteAsync(int id, Pager pager);
        Task<ResultViewModel> NewConsultaAsync(Consulta consulta);
        Task<ResultViewModel> UpdateConsultaAsync(Consulta consulta);
        Task DeleteConsultaAsync(int id);
    }
}
