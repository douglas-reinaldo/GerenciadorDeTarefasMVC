using GerenciadorDeTarefas.Models;

namespace GerenciadorDeTarefas.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        public void AdicionarUsuario(Usuario usuario);
        public Usuario ObterUsuarioPorId(int id);
        public Usuario ObterUsuarioPorEmail(string email);

        public int SalvarMudancas();
    }
}
