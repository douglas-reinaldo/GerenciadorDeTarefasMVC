using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorDeTarefas.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }

        [NotMapped]
        public string Senha { get; set; }
        public List<Tarefa> Tarefas { get; set; } = new List<Tarefa>();

    }
}
