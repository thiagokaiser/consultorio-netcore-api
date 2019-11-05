using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IRepositoryConsulta
    {
        Consulta GetConsulta(int id);
        IEnumerable<Consulta> GetConsultas();
        ResultViewModel NewConsulta(Consulta consulta);
        ResultViewModel UpdateConsulta(Consulta consulta);

    }
}
