using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Models.Enums;
using GerenciadorDeTarefas.Repositories.Interfaces;
using GerenciadorDeTarefas.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorDeTarefas.Tests.Unit.Services
{

    public class TarefaServiceTests
    {
        private readonly Mock<ITarefaRepository> _tarefaRepositoryMock;
        private readonly TarefaService _tarefaService;
        public TarefaServiceTests()
        {
            _tarefaRepositoryMock = new Mock<ITarefaRepository>();
            _tarefaService = new TarefaService(_tarefaRepositoryMock.Object, Mock.Of<ILogger<TarefaService>>());
        }


        [Theory]
        [InlineData(Status.PENDENTE)]
        [InlineData(Status.CONCLUIDA)]
        [InlineData(Status.ANDAMENTO)]
        public async Task BuscarTarefasPorStatus_DeveRetornarTarefas_QuandoStatusValido(Status status)
        {
            // Arrange
            int userId = 1;

            var tarefasSimulacao = new List<Tarefa>
            {
                new Tarefa { Id = 1, Titulo = "T1", Status = Status.PENDENTE, UsuarioId = userId },
                new Tarefa { Id = 2, Titulo = "T2", Status = Status.ANDAMENTO, UsuarioId = userId },
                new Tarefa { Id = 3, Titulo = "T3", Status = Status.CONCLUIDA, UsuarioId = userId },
            };



            // Setup: Configurar o mock para retornar tarefas pelo status
            _tarefaRepositoryMock
                .Setup(r => r.BuscarTarefasPorStatusAsync(status, userId))
                .ReturnsAsync(tarefasSimulacao.Where(t => t.Status == status));



            // ACT (Ação)
            var resultado = await _tarefaService.BuscarTarefasPorStatusAsync(status, userId);

            // Assert
            Assert.Single(resultado);

            _tarefaRepositoryMock.Verify(
                r => r.BuscarTarefasPorStatusAsync(status, userId),
                Times.Once
            );
        }


        [Fact]
        public async Task BuscarTarefasPorStatus_DeveRetornarListaVazia_QuandoNenhumaTarefaEncontrada()
        {
            // Arrange
            int userId = 1;

            // Dados para simulação
            var tarefasSimulacao = new List<Tarefa>
            {
            };

            // Setup: Configurar o mock para retornar uma lista vazia
            _tarefaRepositoryMock
                .Setup(s => s.BuscarTarefasPorStatusAsync(Status.PENDENTE, userId))
                .ReturnsAsync(tarefasSimulacao.Where(s => s.Status == Status.PENDENTE));


            // Act
            var resultado = await _tarefaService.BuscarTarefasPorStatusAsync(Status.PENDENTE, userId);

            // Assert
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task BuscarTarefasPorStatus_DeveLancarExcecao_QuandoRepositoryLancarExcecao()
        {
            // Arrange

            int userId = 1;


            // Setup: Configurar o mock para lançar uma exceção
            _tarefaRepositoryMock
                .Setup(r => r.BuscarTarefasPorStatusAsync(Status.PENDENTE, userId))
                .ThrowsAsync(new TimeoutException());


            // Act e Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _tarefaService.BuscarTarefasPorStatusAsync(Status.PENDENTE, userId));
            _tarefaRepositoryMock.Verify(
                r => r.BuscarTarefasPorStatusAsync(Status.PENDENTE, userId),
                Times.Once
            );
        }


        [Fact]
        public async Task BuscarTarefasPorStatus_DeveLancarExcecao_QuandoIdUsuarioForNulo()
        {
            // Arrange
            int? userId = null;


            // Act e Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _tarefaService.BuscarTarefasPorStatusAsync(Status.CONCLUIDA, userId));
        }


        [Fact]
        public async Task BuscarTarefasPorStatus_DeveLancarExcecao_QuandoStatusForNullo()
        {
            // Arrange
            Status? status = null;
            int userId = 1;

            // Act e Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _tarefaService.BuscarTarefasPorStatusAsync(status, userId));
        }






        // Casos de teste para BuscarTarefasPorPrioridadeAsync podem ser adicionados aqui seguindo o mesmo padrão
        [Theory]
        [InlineData(Prioridade.Baixa)]
        [InlineData(Prioridade.Media)]
        [InlineData(Prioridade.Alta)]
        public async Task BuscarTarefasPorPrioridade_DeveRetornarTarefas_QuandoPrioridadeValida(Prioridade prioridade)
        {
            // Arrange
            int userId = 1;
            var tarefasSimulacao = new List<Tarefa>
            {
                new Tarefa { Id = 1, Titulo = "T1", Prioridade = Prioridade.Baixa, UsuarioId = userId },
                new Tarefa { Id = 2, Titulo = "T2", Prioridade = Prioridade.Media, UsuarioId = userId },
                new Tarefa { Id = 3, Titulo = "T3", Prioridade = Prioridade.Alta, UsuarioId = userId },
            };

            _tarefaRepositoryMock
                .Setup(s => s.BuscarTarefasPorPrioridadeAsync(prioridade, userId))
                .ReturnsAsync(tarefasSimulacao.Where(s => s.Prioridade == prioridade));

            // Act
            var restultado = await _tarefaService.BuscarTarefasPorPrioridadeAsync(prioridade, userId);

            // Assert
            Assert.Single(restultado);
            _tarefaRepositoryMock.Verify(
                s => s.BuscarTarefasPorPrioridadeAsync(prioridade, userId),
                Times.Once
                );
        }


        [Fact]
        public async Task BuscarTarefasPorPrioridade_DeveRetornarExcessao_QuandoIdForNulo()
        {
            // Arrange
            int? userId = null;

            // Act e Assert
            var excessao = await Assert.ThrowsAsync<ArgumentNullException>(() => _tarefaService.BuscarTarefasPorPrioridadeAsync(Prioridade.Baixa, userId));
            Assert.Equal("id", excessao.ParamName);

            _tarefaRepositoryMock.Verify(
                s => s.BuscarTarefasPorPrioridadeAsync(It.IsAny<Prioridade>(), It.IsAny<int>()),
                Times.Never
                );
        }

        [Fact]
        public async Task BuscarTarefasPorPrioridade_DeveRetornarExcessao_QuandoPrioridadeForNula()
        {
            // Arrange
            Prioridade? prioridade = null;
            int userId = 1;

            // Act e Assert
            var excessao = await Assert.ThrowsAsync<ArgumentNullException>(() => _tarefaService.BuscarTarefasPorPrioridadeAsync(prioridade, userId));
            Assert.Equal("prioridade", excessao.ParamName);

            _tarefaRepositoryMock.Verify(
                s => s.BuscarTarefasPorPrioridadeAsync(It.IsAny<Prioridade>(), It.IsAny<int>()),
                Times.Never
                );
        }

        [Fact]
        public async Task BuscarTarefasPorPrioridade_DeveRetornarExcessao_QuandoRepositoryLancarExcessao()
        {
            // Arrange
            int userId = 1;
            _tarefaRepositoryMock
                .Setup(s => s.BuscarTarefasPorPrioridadeAsync(It.IsAny<Prioridade>(), userId))
                .ThrowsAsync(new TimeoutException());

            // Act e Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _tarefaService.BuscarTarefasPorPrioridadeAsync(It.IsAny<Prioridade>(), userId));

            _tarefaRepositoryMock.Verify(
                s => s.BuscarTarefasPorPrioridadeAsync(Prioridade.Alta, userId),
                Times.Once
                );
        }


        [Fact]
        public async Task BuscarTarefasPorPrioridade_DeveRetornarListaVazia_QuandoNenhumaTarefaEncontrada()
        {
            // Arrange
            int userId = 1;
            var tarefasSimulacao = new List<Tarefa>
            { };

            _tarefaRepositoryMock
                .Setup(s => s.BuscarTarefasPorPrioridadeAsync(Prioridade.Media, userId))
                .ReturnsAsync(tarefasSimulacao.Where(s => s.Prioridade == Prioridade.Media));

            // Act
            var resultado = await _tarefaService.BuscarTarefasPorPrioridadeAsync(Prioridade.Media, userId);

            // Assert
            Assert.Empty(resultado);
        }



        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task ListarTarefasPorUserId_DeveRetornarTarefas_QuandoExistiremTarefas(int userId)
        {
            // Arrange
            var tarefasSimulacao = new List<Tarefa>
            {
                new Tarefa { Id = 1, Titulo = "Tarefa 1", UsuarioId = 1 },
                new Tarefa { Id = 2, Titulo = "Tarefa 2", UsuarioId = 2 },
                new Tarefa { Id = 3, Titulo = "Tarefa 3", UsuarioId = 3 },
                new Tarefa { Id = 4, Titulo = "Tarefa 4", UsuarioId = 1 },
                new Tarefa { Id = 5, Titulo = "Tarefa 5", UsuarioId = 2 },
                new Tarefa { Id = 6, Titulo = "Tarefa 6", UsuarioId = 3 },
            };

            // Setup: Configurar o mock para retornar tarefas pelo userId
            _tarefaRepositoryMock
                .Setup(s => s.ObterTarefasPorUserIdAsync(userId))
                .ReturnsAsync(tarefasSimulacao.Where(s => s.UsuarioId.Equals(userId)).ToList());


            // Act
            var resultado = await _tarefaService.ListarTarefasDoUsuarioAsync(userId);

            // Assert
            Assert.Equal(2, resultado.Count);
            _tarefaRepositoryMock.Verify(
                s => s.ObterTarefasPorUserIdAsync(userId),
                Times.Once
                );
        }


        [Fact]
        public async Task ListarTarefasPorUserId_DeveRetornarListaVazia_QuandoNaoHouverTarefas()
        {
            // Arrange
            int userId = 1;

            var tarefasSimulacao = new List<Tarefa> { };

            // Setup: Configurar o mock para retornar a mesma lista, não importa a condição
            _tarefaRepositoryMock
                .Setup(s => s.ObterTarefasPorUserIdAsync(userId))
                .ReturnsAsync(tarefasSimulacao.Where(s => s.UsuarioId.Equals(userId)).ToList());

            // Act
            var resultado = await _tarefaService.ListarTarefasDoUsuarioAsync(userId);

            // Assert
            Assert.Empty(resultado);
        }



        [Fact]
        public async Task ListarTarefasPorUserId_DeveLancarExcecao_QuandoUserIdForNulo()
        {
            // Arrange
            int? userId = null;

            // Act e Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _tarefaService.ListarTarefasDoUsuarioAsync(userId));
        }


        [Fact]
        public async Task ListarTarefasPorUserId_DeveLancarExcecao_QuandoUserIdForInvalido()
        {
            // Arrange
            int userId = -1;

            // Act e Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _tarefaService.ListarTarefasDoUsuarioAsync(userId));
        }



        [Fact]
        public async Task ListarTarefasPorUserId_DeveLancarExcecao_QuandoRepositoryLancarExcecao()
        {
            // Arrange
            int userId = 1;

            // Setup: Configurar o mock para lançar uma exceção
            _tarefaRepositoryMock
                .Setup(s => s.ObterTarefasPorUserIdAsync(userId))
                .ThrowsAsync(new TimeoutException());

            // Act e Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _tarefaService.ListarTarefasDoUsuarioAsync(userId));
        }



        [Fact]
        public async Task ListarTarefasPorUserId_DeveLancarExcecao_QuandoLancarExcecaoGenerica()
        {
            // Arrange
            int userId = 1;


            // Setup: Configurar o mock para lançar uma exceção genérica
            _tarefaRepositoryMock
                .Setup(s => s.ObterTarefasPorUserIdAsync(userId))
                .ThrowsAsync(new Exception());


            // Act e Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _tarefaService.ListarTarefasDoUsuarioAsync(userId));
        }


    }
}
