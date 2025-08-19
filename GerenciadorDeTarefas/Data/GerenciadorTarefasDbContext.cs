using GerenciadorDeTarefas.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeTarefas.Data
{
    public class GerenciadorTarefasDbContext : DbContext
    {
        public GerenciadorTarefasDbContext(DbContextOptions<GerenciadorTarefasDbContext> options) : base(options)
        {

        }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Tarefa> Tarefa { get; set; }
    }
}
