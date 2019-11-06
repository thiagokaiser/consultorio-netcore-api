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
                var paciente = await conexao.QueryFirstAsync<Paciente>(@"
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
                    var query = "INSERT INTO Paciente(Nome) VALUES(@Nome);"; 
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
                    return new ResultViewModel
                    {
                        Success = false,
                        Message = "Erro",
                        Data = ex.ToString()
                    };
                }
            }
        }
        public async Task<ResultViewModel> UpdatePacienteAsync(Paciente paciente)
        {            
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                try
                {
                    var query = @"Update Paciente Set 
                                  Nome = @Nome
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


