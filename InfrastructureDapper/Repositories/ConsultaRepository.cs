using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.ViewModels;
using Dapper;
using Npgsql;

namespace InfrastructureDapper.Repositories
{
    public class ConsultaRepository : IRepositoryConsulta
    {
        private string connectionString;
        public ConsultaRepository(string connectionString)
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

                try
                {
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
                catch (Exception ex)
                {
                    throw new ConsultaException(ex.Message);
                }
            }
        }

        public async Task<ListConsultaViewModel> GetConsultasPacienteAsync(int id, Pager pager)
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                var queryParams = new { 
                    Id = id,
                    Page = pager.Page,
                    PageSize = pager.PageSize,
                    OffSet = pager.OffSet,
                    OrderBy = pager.OrderBy,
                    SearchText = pager.SearchText
                };

                string safeOrderBy = SafeOrderBy(pager.OrderBy);

                string where = @"conduta LIKE @SearchText
                                 OR diagnostico LIKE @SearchText
                                 OR exames LIKE @SearchText                                 
                                 OR paciente.nome LIKE @SearchText";

                try
                {                
                    string queryCount = $"Select COUNT(*) from consulta INNER JOIN paciente on paciente.id = consulta.pacienteid " +
                                        $"WHERE consulta.pacienteid = @Id AND({where})";
                    var consultascount = await conexao.QueryAsync<int>(queryCount, queryParams);

                    string query = $"Select * from consulta INNER JOIN paciente on paciente.id = consulta.pacienteid " +
                                   $"WHERE consulta.pacienteid = @Id AND({where}) ORDER BY {safeOrderBy} Limit @PageSize OffSet @OffSet";

                    var consultas = await conexao.QueryAsync<Consulta, Paciente, Consulta>(query,
                                                                                           (consulta, paciente) =>
                                                                                           {
                                                                                               consulta.Paciente = paciente;
                                                                                               return consulta;
                                                                                           },
                                                                                           splitOn: "id",
                                                                                           param: queryParams
                                                                                          );

                    return new ListConsultaViewModel { count = consultascount.First(), consultas = consultas };
                }
                catch (Exception ex)
                {
                    throw new ConsultaException(ex.Message);
                }
            }
        }

        public async Task<ResultViewModel> NewConsultaAsync(Consulta consulta)
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                await ValidaConsultaAsync(conexao, consulta);
                
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
                    throw new ConsultaException(ex.Message);
                }                
            }
        }

        public async Task<ResultViewModel> UpdateConsultaAsync(Consulta consulta)
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                await ValidaConsultaAsync(conexao, consulta);

                var validConsulta = await conexao.QueryFirstOrDefaultAsync<Consulta>("Select id from Consulta where id = @id", new { id = consulta.Id });
                if (validConsulta == null) throw new ConsultaException("Consulta não cadastrada no sistema");

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
                    throw new ConsultaException(ex.Message);                
                }
            }
        }

        public async Task<ResultViewModel> DeleteConsultaAsync(int id)
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {                
                if (this.GetConsultaAsync(id) == null)
                {
                    throw new ConsultaException("Consulta não existe");                    
                }
                
                try
                {
                    var query = "DELETE FROM Consulta Where id = @Id";
                    await conexao.ExecuteAsync(query, new { Id = id });

                    return new ResultViewModel
                    {
                        Success = true,
                        Message = "Consulta eliminada com sucesso.",
                        Data = null
                    };
                }
                catch (Exception ex)
                {
                    throw new ConsultaException(ex.Message);
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

        private async Task ValidaConsultaAsync(NpgsqlConnection conexao, Consulta consulta)
        {
            List<string> erros = new List<string> { };

            if (consulta.Conduta == "exemplo") erros.Add("Conduta inválida");

            var paciente = await conexao.QueryFirstOrDefaultAsync<Paciente>("Select * from Paciente where id = @id", new { id = consulta.PacienteId });
            if (paciente == null) erros.Add("Paciente não cadastrado no sistema");            

            if (erros.Count > 0)
            {
                throw new ConsultaException(erros);
            }
        }        
    }
}
