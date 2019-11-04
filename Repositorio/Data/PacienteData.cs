using System.Collections.Generic;
using Npgsql;
using Dapper.Contrib.Extensions;
using Core.Models;
using Core.Services;
using Core.Interfaces;
using Dapper;
using System;
using Core.ViewModels;

namespace Repositorio.Data
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
                    Select * from Paciente Where Id = @Id",
                    new { Id = id }
                    );
                return paciente;
            }
        }
        public IEnumerable<Paciente> GetPacientes()
        {
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                var pacientes = conexao.Query<Paciente>("Select * from Paciente");
                return pacientes;
            }
        }
        public ResultViewModel NewPaciente(Paciente paciente)
        {            
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                try
                {
                    var query = "INSERT INTO Paciente(Nome) VALUES(@Nome);"; 
                    conexao.Execute(query, paciente);                    
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
                        Data = ex
                    };
                }
            }
        }
        public ResultViewModel UpdatePaciente(Paciente paciente)
        {
            
            using (NpgsqlConnection conexao = new NpgsqlConnection(connectionString))
            {
                try
                {
                    var query = @"Update Paciente Set 
                                  Nome = @Nome
                                  Where Id = @Id";
                    conexao.Execute(query, paciente);                    
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


