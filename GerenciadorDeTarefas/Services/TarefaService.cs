using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Models.Enums;

namespace GerenciadorDeTarefas.Services
{
    public class TarefaService
    {
        private readonly GerenciadorTarefasDbContext _context;

        public TarefaService(GerenciadorTarefasDbContext context) 
        {
            _context = context;
        }

        public List<Tarefa> GetTarefas(int userId) 
        {   
             return _context.Tarefa.Where(n => n.UsuarioId == userId).ToList();
        }

        public void AddTarefa(Tarefa tarefa, int userId) 
        {
            tarefa.UsuarioId = userId;
            tarefa.DataCriacao = DateTime.Now;
            tarefa.Status = Status.PENDENTE;

            _context.Tarefa.Add(tarefa);
            _context.SaveChanges();
        }

        
    }
}
