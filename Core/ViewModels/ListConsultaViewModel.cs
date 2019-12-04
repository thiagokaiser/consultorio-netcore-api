using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ViewModels
{
    public class ListConsultaViewModel
    {
        public int count { get; set; }
        public IEnumerable<Consulta> consultas { get; set; }
    }
}
