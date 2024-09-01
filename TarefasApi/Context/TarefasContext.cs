using TarefasApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace TarefasApi.Context
{
    public class TarefasContext : DbContext
    {
        public TarefasContext(DbContextOptions<TarefasContext> options) : base(options)
        {}
        public DbSet<Tarefa> Tarefas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
