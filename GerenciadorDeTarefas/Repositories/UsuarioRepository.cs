using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeTarefas.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly GerenciadorTarefasDbContext _context;    
        public UsuarioRepository(GerenciadorTarefasDbContext context)
        {
            _context = context;
        }


        public async Task AdicionarUsuarioAsync(Usuario usuario)
        {
            await _context.AddAsync(usuario);
        }



        public async Task<Usuario> ObterUsuarioPorEmailAsync(string email)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Email == email);
        }

        

        public async Task<Usuario> ObterUsuarioPorIdAsync(int id)
        {
            return await _context.Usuario.FindAsync(id);
        }



        public async Task<int> SalvarMudancasAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
