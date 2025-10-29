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


        public async Task<List<Tarefa>> ListarTarefasDoUsuarioAsync(int? userId)
        {
            if (userId is null)
            {
                _logger.LogWarning("ID do usuário nulo na busca de tarefas");
                throw new ArgumentNullException(nameof(userId));
            }

            if (userId <= 0)
            {
                _logger.LogWarning("ID do usuário inválido: {UserId}", userId);
                throw new ArgumentOutOfRangeException(nameof(userId), "O ID do usuário deve ser maior que zero.");
            }

            try
            {
                _logger.LogInformation("Buscando tarefas do usuário {UserId}", userId);
                var tarefas = await _tarefaRepository.ObterTarefasPorUserIdAsync(userId.Value);

                _logger.LogInformation("Encontradas {Count} tarefas para usuário {UserId}",
                    tarefas.Count, userId);

                return tarefas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar tarefas do usuário {UserId}", userId);
                throw; 
            }
        }



        public async Task CriarTarefaAsync(Tarefa tarefa, int userId)
        {
            try
            {
                _logger.LogInformation("Adicionando tarefa '{Titulo}' para usuário {UserId}", tarefa.Titulo, userId);

                tarefa.UsuarioId = userId;
                tarefa.DataCriacao = DateTime.UtcNow;

                await _tarefaRepository.AdicionarAsync(tarefa);
                await _tarefaRepository.SalvarMudancasAsync();

                _logger.LogInformation("Tarefa ID {TarefaId} criada com sucesso", tarefa.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao adicionar tarefa '{Titulo}' para usuário {UserId}",
                    tarefa.Titulo, userId);
                throw;
            }
        }




        public async Task<Tarefa?> BuscarTarefaPorIdAsync(int? id)
        {
            if (id is null) 
            {
                _logger.LogWarning("ID da tarefa nulo na busca de tarefa por ID");
                throw new ArgumentNullException(nameof(id), "O ID da tarefa não pode ser nulo.");
            }
            if (id <= 0) 
            {
                _logger.LogWarning("ID da tarefa inválido: {TarefaId}", id);
                throw new ArgumentOutOfRangeException(nameof(id), "O ID da tarefa deve ser maior que zero.");

            }
            try
            {
                Tarefa tarefa = await _tarefaRepository.ObterTarefaPorIdAsync(id.Value);
                if (tarefa == null)
                {
                    _logger.LogInformation("Tarefa ID {TarefaId} não encontrada.", id);
                    return null;
                }
                return tarefa;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task AtualizarDadosDaTarefaAsync(Tarefa tarefa)
        {
            if (tarefa == null)
            {
                _logger.LogWarning("Tentativa de atualizar tarefa nula");
                throw new ArgumentNullException(nameof(tarefa), "Tarefa não pode ser nula.");
            }

            try
            {
                _logger.LogInformation("Atualizando tarefa ID {TarefaId}", tarefa.Id);

                await _tarefaRepository.Atualizar(tarefa);
                await _tarefaRepository.SalvarMudancasAsync();

                _logger.LogInformation("Tarefa ID {TarefaId} atualizada com sucesso", tarefa.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar tarefa ID {TarefaId}", tarefa.Id);
                throw;
            }
        }



        public async Task RemoverTarefaAsync(Tarefa tarefa)
        {
            if (tarefa == null)
            {
                _logger.LogWarning("Tentativa de deletar tarefa nula");
                throw new ArgumentNullException(nameof(tarefa), "Tarefa não pode ser nula.");
            }

            try
            {
                _logger.LogInformation("Deletando tarefa ID {TarefaId} - '{Titulo}'", tarefa.Id, tarefa.Titulo);

                await _tarefaRepository.Deletar(tarefa);
                await _tarefaRepository.SalvarMudancasAsync();

                _logger.LogInformation("Tarefa ID {TarefaId} deletada com sucesso", tarefa.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar tarefa ID {TarefaId}", tarefa.Id);
                throw;
            }
        }



        public async Task<IEnumerable<Tarefa>> BuscarTarefasPorStatusAsync(Status? status, int? id) 
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
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Erro inesperado ao buscar tarefas por status para o usuário ID {UserId}", id);
                throw;
            }
        }


        public async Task<IEnumerable<Tarefa>> BuscarTarefasPorPrioridadeAsync(Prioridade? prioridade, int? id) 
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
                throw;
            }
        }

    }
}
