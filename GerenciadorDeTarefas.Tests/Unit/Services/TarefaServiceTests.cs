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
            var resultado = await _tarefaService.BuscarTarefasPorStatus(status, userId);

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
            var resultado = await _tarefaService.BuscarTarefasPorStatus(Status.PENDENTE, userId);

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
            await Assert.ThrowsAsync<InvalidOperationException>(() => _tarefaService.BuscarTarefasPorStatus(Status.PENDENTE, userId));
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
            await Assert.ThrowsAsync<ArgumentNullException>(() => _tarefaService.BuscarTarefasPorStatus(Status.CONCLUIDA, userId));
        }


        [Fact]
        public async Task BuscarTarefasPorStatus_DeveLancarExcecao_QuandoStatusForNullo() 
        {
            // Arrange
            Status? status = null;
            int userId = 1;

            // Act e Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _tarefaService.BuscarTarefasPorStatus(status, userId));
        }
    }
}
