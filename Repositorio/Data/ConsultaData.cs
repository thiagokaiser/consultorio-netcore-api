using Core.Interfaces;
using Core.Models;
using Core.ViewModels;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositorio.Data
{
    public class ConsultaData : IRepositoryConsulta
    {
        private string connectionString;
        public ConsultaData(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public Consulta GetConsulta(int id)
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                var consulta = conexao.QueryFirst<Consulta>(@"
                    Select * from consulta Where Id = @Id",
                    new { Id = id }
                    );
                return consulta;
            }

        }
        public IEnumerable<Consulta> GetConsultas()
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                var consultas = conexao.Query<Consulta>("Select * from consulta");
                return consultas;
            }
        }
        public ResultViewModel NewConsulta(Consulta consulta)
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                List<ErroViewModel> erros = new List<ErroViewModel>();

                var paciente = conexao.QueryFirstOrDefault<Paciente>("Select * from Paciente where id = @id", new { id = consulta.PacienteId });
                if(paciente == null)
                {
                    erros.Add(new ErroViewModel
                    {
                        Campo = "PacienteId",
                        Erro = "Paciente não cadastrado no sistema",
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
                try
                {
                    var query = @"Insert into Consulta(pacienteid, conduta, diagnostico, cid) 
                                          VALUES (@PacienteId,@Conduta,@Diagnostico,@Cid)";
                    conexao.Execute(query, consulta);
                    return new ResultViewModel
                    {
                        Success = true,
                        Message = "Consulta adicionada com sucesso.",
                        Data = consulta
                    };
                }
                catch (Exception ex)
                {
                    return new ResultViewModel
                    {
                        Success = false,
                        Message = "Erro",
                        Data = ex.ToString()
                    };
                }
                
            }

        }

    }
}
