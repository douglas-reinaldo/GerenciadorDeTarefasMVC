using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Models.Enums;

namespace GerenciadorDeTarefas.Repositories.Interfaces
{
    public interface ITarefaRepository
    {
        public List<Tarefa> GetTarefas(int userId);
        public void AddTarefa(Tarefa tarefa, int userId);
        public Tarefa ObterTarefaPorId(int id);
        public void AtualizarTarefa(Tarefa tarefa);
        public void DeletarTarefa(Tarefa tarefa);
        public IEnumerable<Tarefa> BuscarTarefasPorStatus(Status status, int id);
        public IEnumerable<Tarefa> BuscarTarefaPorPrioridade(Prioridade prioridade, int id);

        public int SalvarMudancas();
    }
}
