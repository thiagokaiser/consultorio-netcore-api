using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.ViewModels;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ListConsultaViewModel> GetConsultasAsync(Pager pager)
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                string safeOrderBy = SafeOrderBy(pager.OrderBy);                

                string where = @"WHERE conduta LIKE @SearchText
                                 OR diagnostico LIKE @SearchText
                                 OR exames LIKE @SearchText                                 
                                 OR paciente.nome LIKE @SearchText";

                string queryCount = $"Select COUNT(*) from consulta INNER JOIN paciente on paciente.id = consulta.pacienteid {where}";
                var consultascount = await conexao.QueryAsync<int>(queryCount, pager);

                string query = $"Select * from consulta INNER JOIN paciente on paciente.id = consulta.pacienteid " +                             
                             $"{where} ORDER BY {safeOrderBy} Limit @PageSize OffSet @OffSet";

                var consultas = await conexao.QueryAsync<Consulta, Paciente, Consulta>(query,
                                                                                       (consulta, paciente) =>
                                                                                       {
                                                                                           consulta.Paciente = paciente;
                                                                                           return consulta;
                                                                                       },                                                                                       
                                                                                       splitOn:"id",
                                                                                       param: pager                                                                                       
                                                                                      );
                
                return new ListConsultaViewModel { count = consultascount.First(), consultas = consultas };
            }
        }
        public async Task<IEnumerable<Consulta>> GetConsultasPacienteAsync(int id)
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                var consultas = await conexao.QueryAsync<Consulta, Paciente, Consulta>(@"
                                SELECT * FROM consulta
                                INNER JOIN paciente ON paciente.id = consulta.pacienteid
                                WHERE consulta.pacienteid = @Id",
                                (consulta, paciente) =>
                                {
                                    consulta.Paciente = paciente;
                                    return consulta;
                                },
                                splitOn: "Id",
                                param: new { Id = id });

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

        private string SafeOrderBy(string orderby)
        {
            var safeOrderBy = "";

            switch (orderby)
            {
                case "dtconsulta asc":
                    safeOrderBy = "consulta.dtconsulta ASC";
                    break;
                case "dtconsulta desc":
                    safeOrderBy = "consulta.dtconsulta DESC";
                    break;
                case "nome asc":
                    safeOrderBy = "paciente.nome ASC";
                    break;
                case "nome desc":
                    safeOrderBy = "paciente.nome DESC";
                    break;
                case "id asc":
                    safeOrderBy = "consulta.id ASC";
                    break;
                case "id desc":
                    safeOrderBy = "consulta.id DESC";
                    break;
                default:
                    safeOrderBy = "consulta.id ASC";
                    break;
            }

            return safeOrderBy;
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
    }
}
