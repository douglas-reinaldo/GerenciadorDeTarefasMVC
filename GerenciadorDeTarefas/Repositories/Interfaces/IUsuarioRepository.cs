using GerenciadorDeTarefas.Models;

namespace GerenciadorDeTarefas.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        public Task AdicionarUsuarioAsync(Usuario usuario);
        public Task<Usuario> ObterUsuarioPorIdAsync(int id);
        public Task<Usuario> ObterUsuarioPorEmailAsync(string email);

        public Task<int> SalvarMudancasAsync();
    }
}
