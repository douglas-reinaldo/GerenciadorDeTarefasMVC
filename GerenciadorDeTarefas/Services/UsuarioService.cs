using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;

namespace GerenciadorDeTarefas.Services
{
    public class UsuarioService
    {
        private readonly GerenciadorTarefasDbContext _context;

        public UsuarioService(GerenciadorTarefasDbContext context) { _context = context; }

        public void AdicionarUsuario(Usuario usuario) 
        {
            _context.Add(usuario);
            _context.SaveChanges();
        }
    }
}
