using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Models.Enums;
using System.Runtime.InteropServices.Marshalling;

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

        public Tarefa ObterTarefaPorId(int Id) 
        {
            return _context.Tarefa.FirstOrDefault(n => n.Id == Id);
        }


        public void AtualizarTarefa(Tarefa tarefa) 
        {

            if (tarefa == null) 
            {
                throw new Exception("Tarefa não encontrada!");
            }

            try
            {
                _context.Tarefa.Update(tarefa);
                _context.SaveChanges();
            }
            catch (Exception e) 
            {
                throw new Exception(e.Message);
            }

            
        }

        public void DeletarTarefa(Tarefa tarefa) 
        {
            _context.Tarefa.Remove(tarefa);
            _context.SaveChanges();
        }
        
    }
}
