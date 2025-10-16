using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Models.Enums;
using GerenciadorDeTarefas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeTarefas.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly GerenciadorTarefasDbContext _context;

        public TarefaRepository(GerenciadorTarefasDbContext context)
        {
            _context = context;
        }

        public void AddTarefa(Tarefa tarefa, int userId)
        {
            _context.Tarefa.Add(tarefa);
        }

        public void AtualizarTarefa(Tarefa tarefa)
        {
            _context.Tarefa.Update(tarefa);
        }

        public IEnumerable<Tarefa> BuscarTarefaPorPrioridade(Prioridade prioridade, int id)
        {
            return _context.Tarefa.Where(s => s.Prioridade.Equals(prioridade) && s.UsuarioId.Equals(id)).ToList();
        }

        public IEnumerable<Tarefa> BuscarTarefasPorStatus(Status status, int id)
        {
            return _context.Tarefa.Where(s => s.Status.Equals(status) && s.UsuarioId.Equals(id)).ToList();
        }

        public void DeletarTarefa(Tarefa tarefa)
        {
            _context.Tarefa.Remove(tarefa);
        }

        public List<Tarefa> GetTarefas(int userId)
        {
            return _context.Tarefa.Where(n => n.UsuarioId.Equals(userId)).ToList();
        }

        public Tarefa ObterTarefaPorId(int id)
        {
            return _context.Tarefa.Find(id);
        }

        public int SalvarMudancas()
        {
            return _context.SaveChanges();
        }
    }
}
