using System.Collections.Generic;
using Npgsql;
using Dapper.Contrib.Extensions;
using Core.Models;
using Core.Services;
using Core.Interfaces;
using Dapper;
using System;
using Core.ViewModels;
using System.Threading.Tasks;
using Core.Exceptions;

namespace Repositorio.Data
{
    public class PacienteData : IRepositoryPaciente
    {
        private string connectionString;

        public PacienteData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<Paciente> GetPacienteAsync(int id)
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                var paciente = await conexao.QueryFirstOrDefaultAsync<Paciente>(@"
                    Select * from Paciente Where Id = @Id",
                    new { Id = id }
                    );
                return paciente;
            }
        }
        public async Task<IEnumerable<Paciente>> GetPacientesAsync()
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                var pacientes = await conexao.QueryAsync<Paciente>("Select * from Paciente");
                return pacientes;
            }
        }
        public async Task<ResultViewModel> NewPacienteAsync(Paciente paciente)
        {            
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                try
                {                    
                    var query = "INSERT INTO Paciente(Nome, sexo, dtnascimento, prontuario, convenio) VALUES(@Nome, @Sexo, @DtNascimento, @prontuario, @convenio);"; 
                    await conexao.ExecuteAsync(query, paciente);                    
                    return new ResultViewModel
                    {
                        Success = true,
                        Message = "Paciente adicionado com sucesso",
                        Data = paciente
                    };
                }
                catch (Exception ex)
                {
                    var erros = new List<string> { ex.Message };
                    throw new PacienteException("Erro", erros);
                }
            }
        }
        public async Task<ResultViewModel> UpdatePacienteAsync(Paciente paciente)
        {            
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {                
                if (await this.GetPacienteAsync(paciente.Id) == null)
                {
                    throw new PacienteException("Erro ao atualizar Paciente", new List<string> { "Paciente não existe" });
                }
                try
                {
                    var query = @"Update Paciente Set 
                                  Nome = @Nome,
                                  Sobrenome = @Sobrenome,
                                  Sexo = @Sexo,
                                  DtNascimento = @DtNascimento,
                                  Prontuario = @Prontuario,
                                  Convenio = @Convenio
                                  Where Id = @Id";
                    await conexao.ExecuteAsync(query, paciente);                    
                    return new ResultViewModel
                    {
                        Success = true,
                        Message = "Paciente alterado com sucesso",
                        Data = paciente
                    };
                }
                catch (Exception ex)
                {
                    throw new PacienteException("Erro ao atualizar Paciente", new List<string> { ex.Message });
                }
            }            
        }
        public async Task<ResultViewModel> DeletePacienteAsync(int id)
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                var paciente = await this.GetPacienteAsync(id);
                if (paciente == null)
                {
                    throw new PacienteException("Erro ao deletar Paciente", new List<string> { "Paciente não existe" });
                }
                try
                {
                    var query = "DELETE FROM Paciente WHERE Id = @Id";
                    await conexao.ExecuteAsync(query, new { Id = id });
                    return new ResultViewModel
                    {
                        Success = true,
                        Message = "Paciente eliminado com sucesso.",
                        Data = null
                    };
                }
                catch (Exception ex)
                {
                    throw new PacienteException("Erro ao atualizar Paciente", new List<string> { ex.Message });
                }
            }
        }
    }
}


