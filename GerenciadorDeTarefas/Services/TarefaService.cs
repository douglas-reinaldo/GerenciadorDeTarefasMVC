using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Models.Enums;
using GerenciadorDeTarefas.Repositories;
using GerenciadorDeTarefas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;

namespace GerenciadorDeTarefas.Services
{
    public class TarefaService
    {
        private readonly ILogger<TarefaService> _logger;
        private readonly ITarefaRepository _tarefaRepository;

        public TarefaService(ITarefaRepository tarefaRepository, ILogger<TarefaService> logger)
        {
            _tarefaRepository = tarefaRepository;
            _logger = logger;
        }

        public async Task<List<Tarefa>> GetTarefasAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Buscando tarefas do usuário {UserId}", userId);
                return await _tarefaRepository.GetTarefasAsync(userId);

            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Erro ao buscar tarefas do usuário {UserId}", userId);
                throw;
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "Timeout ao buscar tarefas do usuário {UserId}", userId);
                throw new InvalidOperationException("Tempo de espera esgotado ao buscar tarefas.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar tarefas do usuário {UserId}", userId);
                throw new InvalidOperationException("Erro inesperado ao buscar tarefas.", ex);
            }
        }



        public async Task AddTarefaAsync(Tarefa tarefa, int userId)
        {
            try
            {
                _logger.LogInformation("Adicionando tarefa '{Titulo}' para usuário {UserId}", tarefa.Titulo, userId);

                tarefa.UsuarioId = userId;
                tarefa.DataCriacao = DateTime.UtcNow;

                await _tarefaRepository.AddTarefaAsync(tarefa);
                await _tarefaRepository.SalvarMudancasAsync();

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




        public async Task<Tarefa> ObterTarefaPorIdAsync(int id)
        {
            if (id <= 0) 
            {
                _logger.LogWarning("ID da tarefa inválido: {TarefaId}", id);
                throw new ArgumentException(nameof(id), "ID da tarefa deve ser maior que zero.");
            }
            try 
            {
                Tarefa tarefa =  await _tarefaRepository.ObterTarefaPorIdAsync(id);
                if (tarefa == null) 
                {
                    _logger.LogWarning("Tarefa ID {TarefaId} não encontrada", id);
                    throw new InvalidOperationException("Tarefa não encontrada.");
                }
                return tarefa;
            }
            catch (TimeoutException ex)
            {
                throw new InvalidOperationException("Tempo de espera esgotado ao buscar a tarefa.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro inesperado ao buscar a tarefa.", ex);
            }
        }


        public async Task AtualizarTarefa(Tarefa tarefa)
        {
            if (tarefa == null)
            {
                _logger.LogWarning("Tentativa de atualizar tarefa nula");
                throw new ArgumentNullException(nameof(tarefa), "Tarefa não pode ser nula.");
            }

            try
            {
                _logger.LogInformation("Atualizando tarefa ID {TarefaId}", tarefa.Id);

                await _tarefaRepository.AtualizarTarefaAsync(tarefa);
                await _tarefaRepository.SalvarMudancasAsync();

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



        public async Task DeletarTarefa(Tarefa tarefa)
        {
            if (tarefa == null)
            {
                _logger.LogWarning("Tentativa de deletar tarefa nula");
                throw new ArgumentNullException(nameof(tarefa), "Tarefa não pode ser nula.");
            }

            try
            {
                _logger.LogInformation("Deletando tarefa ID {TarefaId} - '{Titulo}'", tarefa.Id, tarefa.Titulo);

                await _tarefaRepository.DeletarTarefaAsync(tarefa);
                await _tarefaRepository.SalvarMudancasAsync();

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


        public async Task<IEnumerable<Tarefa>> BuscarTarefasPorStatus(Status? status, int? id) 
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
                return await _tarefaRepository.BuscarTarefasPorStatusAsync(status.Value, id.Value);
            }

            catch (TimeoutException ex) 
            {
                _logger.LogError(ex, "Timeout ao buscar tarefas por status para o usuário ID {UserId}", id);
                throw new InvalidOperationException("Tempo de espera esgotado ao buscar tarefas por status.", ex);
            }
            
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Erro inesperado ao buscar tarefas por status para o usuário ID {UserId}", id);
                throw new InvalidOperationException("Erro inesperado ao buscar tarefas por status.", ex);
            }
        }


        public async Task<IEnumerable<Tarefa>> BuscarTarefaPorPrioridade(Prioridade? prioridade, int? id) 
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
                return await _tarefaRepository.BuscarTarefasPorPrioridadeAsync(prioridade.Value, id.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar tarefas por prioridade para o usuário ID {UserId}", id);
                throw new Exception("Erro inesperado ao buscar tarefas por prioridade.", ex);
            }
        }

    }
}
