using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Models.Enums;

namespace GerenciadorDeTarefas.Repositories.Interfaces
{
    public interface ITarefaRepository
    {
        public Task<List<Tarefa>> ObterTarefasPorUserIdAsync(int userId);
        public Task AdicionarAsync(Tarefa tarefa);
        public Task<Tarefa> ObterTarefaPorIdAsync(int id);
        public Task Atualizar(Tarefa tarefa);
        public Task Deletar(Tarefa tarefa);
        public Task<IEnumerable<Tarefa>> BuscarTarefasPorStatusAsync(Status status, int id);
        public Task<IEnumerable<Tarefa>> BuscarTarefasPorPrioridadeAsync(Prioridade prioridade, int id);

        public Task<int> SalvarMudancasAsync();
    }
}
