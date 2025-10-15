using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices.Marshalling;

namespace GerenciadorDeTarefas.Services
{
    public class TarefaService
    {
        private readonly GerenciadorTarefasDbContext _context;
        private readonly ILogger<TarefaService> _logger;

        public TarefaService(GerenciadorTarefasDbContext context, ILogger<TarefaService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Tarefa> GetTarefas(int userId)
        {
            return _context.Tarefa.Where(n => n.UsuarioId == userId).ToList();
        }



        public void AddTarefa(Tarefa tarefa, int userId)
        {
            try
            {
                _logger.LogInformation("Adicionando tarefa '{Titulo}' para usuário {UserId}", tarefa.Titulo, userId);

                tarefa.UsuarioId = userId;
                tarefa.DataCriacao = DateTime.UtcNow;

                _context.Tarefa.Add(tarefa);
                _context.SaveChanges();

                _logger.LogInformation("Tarefa ID {TarefaId} criada com sucesso", tarefa.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao salvar tarefa '{Titulo}' no banco de dados para usuário {UserId}",
                    tarefa.Titulo, userId);
                throw new InvalidOperationException("Erro ao salvar tarefa no banco de dados.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao adicionar tarefa '{Titulo}' para usuário {UserId}",
                    tarefa.Titulo, userId);
                throw new InvalidOperationException("Erro inesperado ao adicionar tarefa.", ex);
            }
        }




        public Tarefa ObterTarefaPorId(int id)
        {
            return _context.Tarefa.Find(id);
        }


        public void AtualizarTarefa(Tarefa tarefa)
        {
            if (tarefa == null)
            {
                _logger.LogWarning("Tentativa de atualizar tarefa nula");
                throw new ArgumentNullException(nameof(tarefa), "Tarefa não pode ser nula.");
            }

            try
            {
                _logger.LogInformation("Atualizando tarefa ID {TarefaId}", tarefa.Id);

                _context.Tarefa.Update(tarefa);
                _context.SaveChanges();

                _logger.LogInformation("Tarefa ID {TarefaId} atualizada com sucesso", tarefa.Id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Conflito de concorrência ao atualizar tarefa ID {TarefaId}", tarefa.Id);
                throw new InvalidOperationException("A tarefa foi modificada por outro usuário.", ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao atualizar tarefa ID {TarefaId} no banco de dados", tarefa.Id);
                throw new InvalidOperationException("Erro ao atualizar tarefa no banco de dados.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar tarefa ID {TarefaId}", tarefa.Id);
                throw new InvalidOperationException("Erro inesperado ao atualizar tarefa.", ex);
            }
        }



        public void DeletarTarefa(Tarefa tarefa)
        {
            if (tarefa == null)
            {
                _logger.LogWarning("Tentativa de deletar tarefa nula");
                throw new ArgumentNullException(nameof(tarefa), "Tarefa não pode ser nula.");
            }

            try
            {
                _logger.LogInformation("Deletando tarefa ID {TarefaId} - '{Titulo}'", tarefa.Id, tarefa.Titulo);

                _context.Tarefa.Remove(tarefa);
                _context.SaveChanges();

                _logger.LogInformation("Tarefa ID {TarefaId} deletada com sucesso", tarefa.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao deletar tarefa ID {TarefaId} do banco de dados", tarefa.Id);
                throw new InvalidOperationException("Erro ao deletar tarefa do banco de dados.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar tarefa ID {TarefaId}", tarefa.Id);
                throw new InvalidOperationException("Erro inesperado ao deletar tarefa.", ex);
            }
        }


        public IEnumerable<Tarefa> BuscarTarefasPorStatus(Status? status, int? id) 
        {
            if (id is null) 
            {
                _logger.LogWarning("ID do usuário nulo na busca de tarefas por status");
                throw new ArgumentNullException(nameof(id), "ID do usuário não pode ser nulo.");
            }

            if (!status.HasValue) 
            {
                _logger.LogWarning("Status nulo na busca de tarefas por status");
                throw new ArgumentNullException(nameof(status), "Status não pode ser nulo.");
            }

            try 
            {
                _logger.LogInformation("Buscando tarefas do usuário pelo status");
                return _context.Tarefa.Where(s => s.Status.Equals(status) && s.UsuarioId.Equals(id)).ToList();
            }

            catch (Exception ex) 
            {
                _logger.LogError(ex, "Erro inesperado ao buscar tarefas por status para o usuário ID {UserId}", id);
                throw new InvalidOperationException("Erro inesperado ao buscar tarefas por status.", ex);
            }
        }


        public IEnumerable<Tarefa> BuscarTarefaPorPrioridade(Prioridade? prioridade, int? id) 
        {
            if (id is null)
            {
                _logger.LogWarning("ID do usuário nulo na busca de tarefas por prioridade");
                throw new ArgumentNullException(nameof(id), "ID do usuário não pode ser nulo.");
            }

            if (!prioridade.HasValue) 
            {
                _logger.LogWarning("Prioridade nula na busca de tarefas por prioridade");
                throw new ArgumentNullException(nameof(prioridade), "Prioridade não pode ser nula.");
            }
            try
            {
                _logger.LogInformation("Buscando tarefas do usuário pela prioridade");
                return _context.Tarefa.Where(p => p.Prioridade.Equals(prioridade) && p.UsuarioId.Equals(id)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar tarefas por prioridade para o usuário ID {UserId}", id);
                throw new InvalidOperationException("Erro inesperado ao buscar tarefas por prioridade.", ex);
            }
        }

    }
}
