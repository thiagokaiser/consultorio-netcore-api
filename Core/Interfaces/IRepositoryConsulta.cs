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
        Task<IEnumerable<Consulta>> GetConsultasAsync();
        Task<IEnumerable<Consulta>> GetConsultasPacienteAsync(int id);
        Task<ResultViewModel> NewConsultaAsync(Consulta consulta);
        Task<ResultViewModel> UpdateConsultaAsync(Consulta consulta);
        Task DeleteConsultaAsync(int id);
    }
}
