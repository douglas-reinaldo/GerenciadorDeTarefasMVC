using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Models.Enums;

namespace GerenciadorDeTarefas.Repositories.Interfaces
{
    public interface ITarefaRepository
    {
        public Task<List<Tarefa>> GetTarefasAsync(int userId);
        public Task AddTarefaAsync(Tarefa tarefa);
        public Task<Tarefa> ObterTarefaPorIdAsync(int id);
        public Task AtualizarTarefaAsync(Tarefa tarefa);
        public Task DeletarTarefaAsync(Tarefa tarefa);
        public Task<IEnumerable<Tarefa>> BuscarTarefasPorStatusAsync(Status status, int id);
        public Task<IEnumerable<Tarefa>> BuscarTarefasPorPrioridadeAsync(Prioridade prioridade, int id);

        public Task<int> SalvarMudancasAsync();
    }
}
