using System.Collections.Generic;
using Npgsql;
using Dapper.Contrib.Extensions;
using Consultorio.Core.Models;
using Consultorio.Core.Services;
using Core.Interfaces;
using Dapper;

namespace Consultorio.Repositorio.Data
{
    public class PacienteData : IRepository
    {
        private string connectionString;

        public PacienteData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Paciente GetPaciente(int id)
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                var paciente = conexao.QueryFirst<Paciente>(@"
                    Select * from public.""Paciente"" Where ""Id"" = @Id",
                    new { Id = id }
                    );
                return paciente;
            }
        }
        public IEnumerable<Paciente> GetPacientes()
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                var pacientes = conexao.Query<Paciente>("Select * from \"Paciente\"");
                return pacientes;
            }
        }
    }
}


