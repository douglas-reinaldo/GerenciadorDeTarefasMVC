using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeTarefas.Data
{
    public class GerenciadorTarefasDbContext : DbContext
    {
        public GerenciadorTarefasDbContext(DbContextOptions<GerenciadorTarefasDbContext> options) : base(options)
        {

        }
    }
}
