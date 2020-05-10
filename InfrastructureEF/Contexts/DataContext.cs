using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureEF.Contexts
{
    public class DataContext : DbContext
    {
        public DbSet<Consulta> Consulta { get; set; }
        public DbSet<Paciente> Paciente { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}