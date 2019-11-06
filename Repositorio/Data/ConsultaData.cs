using Core.Interfaces;
using Core.Models;
using Core.ViewModels;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Data
{
    public class ConsultaData : IRepositoryConsulta
    {
        private string connectionString;
        public ConsultaData(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<Consulta> GetConsultaAsync(int id)
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                var consulta = await conexao.QueryFirstAsync<Consulta>(@"
                    Select * from consulta Where Id = @Id",
                    new { Id = id }
                    );
                return consulta;
            }

        }
        public async Task<IEnumerable<Consulta>> GetConsultasAsync()
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                var consultas = await conexao.QueryAsync<Consulta>("Select * from consulta");
                return consultas;
            }
        }
        public async Task<ResultViewModel> NewConsultaAsync(Consulta consulta)
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                List<ErroViewModel> erros = await ValidaConsultaAsync(conexao, consulta);                

                if (erros.Count > 0)
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
                    await conexao.ExecuteAsync(query, consulta);
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
        public async Task<ResultViewModel> UpdateConsultaAsync(Consulta consulta)
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                List<ErroViewModel> erros = await ValidaConsultaAsync(conexao, consulta);

                var validConsulta = await conexao.QueryFirstOrDefaultAsync<Consulta>("Select id from Consulta where id = @id", new { id = consulta.Id });
                if (validConsulta == null)
                {
                    erros.Add(new ErroViewModel
                    {
                        Campo = "Id",
                        Erro = "Consulta não cadastrada no sistema",
                        Solucao = "Favor informar uma consulta válida"
                    });
                }

                if (erros.Count > 0)
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
                    var query = @"Update Consulta SET
                                     pacienteid  = @PacienteId,
                                     conduta     = @Conduta,
                                     diagnostico = @Diagnostico,
                                     cid         = @Cid
                                  WHERE Id = @Id";

                    await conexao.ExecuteAsync(query, consulta);
                    return new ResultViewModel
                    {
                        Success = true,
                        Message = "Consulta alterada com sucesso.",
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

        private async Task<List<ErroViewModel>> ValidaConsultaAsync(NpgsqlConnection conexao, Consulta consulta)
        {
            List<ErroViewModel> erros = new List<ErroViewModel> { };

            var paciente = await conexao.QueryFirstOrDefaultAsync<Paciente>("Select * from Paciente where id = @id", new { id = consulta.PacienteId });
            if (paciente == null)
            {
                erros.Add(new ErroViewModel
                {
                    Campo = "PacienteId",
                    Erro = "Paciente não cadastrado no sistema",
                    Solucao = "Favor informar um paciente válido"
                });
            }

            return erros;

        }
    }
}
