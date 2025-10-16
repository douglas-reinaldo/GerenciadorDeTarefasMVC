using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repositories.Interfaces;

namespace GerenciadorDeTarefas.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly GerenciadorTarefasDbContext _context;    
        public UsuarioRepository(GerenciadorTarefasDbContext context)
        {
            _context = context;
        }

        public void AdicionarUsuario(Usuario usuario)
        {
            _context.Add(usuario);
        }

        public Usuario ObterUsuarioPorEmail(string email)
        {
            return _context.Usuario.Find(email);
        }

        public Usuario ObterUsuarioPorId(int id)
        {
            return _context.Usuario.Find(id);
        }

        public int SalvarMudancas()
        {
            return _context.SaveChanges();
        }
    }
}
