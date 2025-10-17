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


        public async Task AddTarefaAsync(Tarefa tarefa)
        {
            await _context.Tarefa.AddAsync(tarefa);
        }



        public async Task AtualizarTarefaAsync(Tarefa tarefa)
        {
            _context.Tarefa.Update(tarefa);
        }



        public async Task<IEnumerable<Tarefa>> BuscarTarefaPorPrioridadeAsync(Prioridade prioridade, int id)
        {
            return await _context.Tarefa.Where(s => s.Prioridade.Equals(prioridade) && s.UsuarioId.Equals(id)).ToListAsync();
        }



        public async Task<IEnumerable<Tarefa>> BuscarTarefasPorStatusAsync(Status status, int id)
        {
            return await _context.Tarefa.Where(s => s.Status.Equals(status) && s.UsuarioId.Equals(id)).ToListAsync();
        }



        public async Task DeletarTarefaAsync(Tarefa tarefa)
        {
            _context.Tarefa.Remove(tarefa);
        }



        public async Task<List<Tarefa>> GetTarefasAsync(int userId)
        {
            return await _context.Tarefa.Where(n => n.UsuarioId.Equals(userId)).ToListAsync();
        }



        public async Task<Tarefa> ObterTarefaPorIdAsync(int id)
        {
            return await _context.Tarefa.FindAsync(id);
        }



        public async Task<int> SalvarMudancasAsync()
        {
            return await _context.SaveChangesAsync();
        }

        


    }
}
