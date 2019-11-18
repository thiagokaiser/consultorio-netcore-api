using Core.Exceptions;
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
        public async Task<IEnumerable<Consulta>> GetConsultasPacienteAsync(int id)
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                var consultas = await conexao.QueryAsync<Consulta>(@"
                                SELECT * FROM consulta
                                WHERE pacienteid = @Id
                                ", new { Id = id });
                return consultas;
            }
        }
        public async Task<ResultViewModel> NewConsultaAsync(Consulta consulta)
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                List<string> erros = await ValidaConsultaAsync(conexao, consulta);                

                if (erros.Count > 0)
                {
                    throw new ConsultaException("Erro", erros);
                }
                try
                {
                    var query = @"Insert into Consulta(pacienteid, conduta, diagnostico, cid, dtconsulta, exames, retorno) 
                                          VALUES (@PacienteId,@Conduta,@Diagnostico,@Cid,@DtConsulta,@Exames,@Retorno)";
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
                    throw new ConsultaException("Erro", new List<string> { ex.Message });
                }
                
            }

        }
        public async Task<ResultViewModel> UpdateConsultaAsync(Consulta consulta)
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                List<string> erros = await ValidaConsultaAsync(conexao, consulta);
                
                var validConsulta = await conexao.QueryFirstOrDefaultAsync<Consulta>("Select id from Consulta where id = @id", new { id = consulta.Id });
                if (validConsulta == null)
                {
                    erros.Add("Consulta não cadastrada no sistema");
                }
                if (erros.Count > 0)
                {
                    throw new ConsultaException("Erro", erros);
                }
                try
                {
                    var query = @"Update Consulta SET
                                     pacienteid  = @PacienteId,
                                     conduta     = @Conduta,
                                     diagnostico = @Diagnostico,
                                     cid         = @Cid,
                                     dtConsulta  = @DtConsulta,
                                     exames      = @Exames,
                                     retorno     = @Retorno
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
                    throw new ConsultaException("Erro", new List<string> { ex.Message });                
                }

            }

        }

        private async Task<List<string>> ValidaConsultaAsync(NpgsqlConnection conexao, Consulta consulta)
        {
            List<string> erros = new List<string> { };

            var paciente = await conexao.QueryFirstOrDefaultAsync<Paciente>("Select * from Paciente where id = @id", new { id = consulta.PacienteId });
            if (paciente == null)
            {
                erros.Add("Paciente não cadastrado no sistema");
            }

            return erros;

        }

        public async Task DeleteConsultaAsync(int id)
        {            
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))         
            {
                List<string> erros = new List<string>();
                if (this.GetConsultaAsync(id) == null)
                {
                    erros.Add("Consulta não existe");                       
                }
                if (erros.Count > 0)
                {
                    throw new ConsultaException("erro ao deletar", erros);
                }
                try
                {
                    var query = "DELETE FROM Consulta Where id = @Id";
                    await conexao.ExecuteAsync(query, new { Id = id });

                }
                catch (Exception ex)
                {
                    throw new ConsultaException("Erro", new List<string> { ex.Message });
                }

            }
        }
    }
}
