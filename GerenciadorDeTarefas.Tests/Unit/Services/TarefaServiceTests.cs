using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Models.Enums;
using GerenciadorDeTarefas.Repositories.Interfaces;
using GerenciadorDeTarefas.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Sdk;

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

        #region BuscarTarefasPorStatus

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

            _tarefaRepositoryMock
                .Setup(r => r.BuscarTarefasPorStatusAsync(status, userId))
                .ReturnsAsync(tarefasSimulacao.Where(t => t.Status == status));

            // Act
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
            var tarefasSimulacao = new List<Tarefa>();

            _tarefaRepositoryMock
                .Setup(s => s.BuscarTarefasPorStatusAsync(Status.PENDENTE, userId))
                .ReturnsAsync(tarefasSimulacao.Where(s => s.Status == Status.PENDENTE));

            // Act
            var resultado = await _tarefaService.BuscarTarefasPorStatusAsync(Status.PENDENTE, userId);

            // Assert
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task BuscarTarefasPorStatus_DeveLancarTimeoutException_QuandoRepositoryLancarExcecao()
        {
            // Arrange
            int userId = 1;

            _tarefaRepositoryMock
                .Setup(r => r.BuscarTarefasPorStatusAsync(Status.PENDENTE, userId))
                .ThrowsAsync(new TimeoutException());

            // Act & Assert
            await Assert.ThrowsAsync<TimeoutException>(() =>
                _tarefaService.BuscarTarefasPorStatusAsync(Status.PENDENTE, userId));

            _tarefaRepositoryMock.Verify(
                r => r.BuscarTarefasPorStatusAsync(Status.PENDENTE, userId),
                Times.Once
            );
        }

        [Fact]
        public async Task BuscarTarefasPorStatus_DeveLancarArgumentNullException_QuandoIdUsuarioForNulo()
        {
            // Arrange
            int? userId = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _tarefaService.BuscarTarefasPorStatusAsync(Status.CONCLUIDA, userId));
        }

        [Fact]
        public async Task BuscarTarefasPorStatus_DeveLancarArgumentNullException_QuandoStatusForNulo()
        {
            // Arrange
            Status? status = null;
            int userId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _tarefaService.BuscarTarefasPorStatusAsync(status, userId));
        }


        [Fact]
        public async Task BuscarTarefasPorStatus_DeveLancarArgumentOutOfRangeException_QuandoUserIdForZero()
        {
            // Arrange
            int userId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _tarefaService.BuscarTarefasPorStatusAsync(Status.PENDENTE, userId));
        }

        [Fact]
        public async Task BuscarTarefasPorStatus_DeveLancarArgumentOutOfRangeException_QuandoUserIdForNegativo()
        {
            // Arrange
            int userId = -5;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _tarefaService.BuscarTarefasPorStatusAsync(Status.PENDENTE, userId));
        }

        #endregion




        #region BuscarTarefasPorPrioridade

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
            var resultado = await _tarefaService.BuscarTarefasPorPrioridadeAsync(prioridade, userId);

            // Assert
            Assert.Single(resultado);
            _tarefaRepositoryMock.Verify(
                s => s.BuscarTarefasPorPrioridadeAsync(prioridade, userId),
                Times.Once
            );
        }

        [Fact]
        public async Task BuscarTarefasPorPrioridade_DeveRetornarArgumentNullException_QuandoIdForNulo()
        {
            // Arrange
            int? userId = null;

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _tarefaService.BuscarTarefasPorPrioridadeAsync(Prioridade.Baixa, userId));

            Assert.Equal("id", excecao.ParamName);

            _tarefaRepositoryMock.Verify(
                s => s.BuscarTarefasPorPrioridadeAsync(It.IsAny<Prioridade>(), It.IsAny<int>()),
                Times.Never
            );
        }

        [Fact]
        public async Task BuscarTarefasPorPrioridade_DeveRetornarArgumentNullException_QuandoPrioridadeForNula()
        {
            // Arrange
            Prioridade? prioridade = null;
            int userId = 1;

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _tarefaService.BuscarTarefasPorPrioridadeAsync(prioridade, userId));

            Assert.Equal("prioridade", excecao.ParamName);

            _tarefaRepositoryMock.Verify(
                s => s.BuscarTarefasPorPrioridadeAsync(It.IsAny<Prioridade>(), It.IsAny<int>()),
                Times.Never
            );
        }

        [Fact]
        public async Task BuscarTarefasPorPrioridade_DeveLancarTimeoutException_QuandoRepositoryLancarExcecao()
        {
            // Arrange
            int userId = 1;
            _tarefaRepositoryMock
                .Setup(s => s.BuscarTarefasPorPrioridadeAsync(Prioridade.Alta, userId))
                .ThrowsAsync(new TimeoutException());

            // Act & Assert
            await Assert.ThrowsAsync<TimeoutException>(() =>
                _tarefaService.BuscarTarefasPorPrioridadeAsync(Prioridade.Alta, userId));

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
            var tarefasSimulacao = new List<Tarefa>();

            _tarefaRepositoryMock
                .Setup(s => s.BuscarTarefasPorPrioridadeAsync(Prioridade.Media, userId))
                .ReturnsAsync(tarefasSimulacao.Where(s => s.Prioridade == Prioridade.Media));

            // Act
            var resultado = await _tarefaService.BuscarTarefasPorPrioridadeAsync(Prioridade.Media, userId);

            // Assert
            Assert.Empty(resultado);
        }


        [Fact]
        public async Task BuscarTarefasPorPrioridade_DeveLancarArgumentOutOfRangeException_QuandoUserIdForZero()
        {
            // Arrange
            int userId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _tarefaService.BuscarTarefasPorPrioridadeAsync(Prioridade.Alta, userId));
        }

        [Fact]
        public async Task BuscarTarefasPorPrioridade_DeveLancarArgumentOutOfRangeException_QuandoUserIdForNegativo()
        {
            // Arrange
            int userId = -10;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _tarefaService.BuscarTarefasPorPrioridadeAsync(Prioridade.Alta, userId));
        }

        #endregion




        #region ListarTarefasDoUsuario

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
            var tarefasSimulacao = new List<Tarefa>();

            _tarefaRepositoryMock
                .Setup(s => s.ObterTarefasPorUserIdAsync(userId))
                .ReturnsAsync(tarefasSimulacao.Where(s => s.UsuarioId.Equals(userId)).ToList());

            // Act
            var resultado = await _tarefaService.ListarTarefasDoUsuarioAsync(userId);

            // Assert
            Assert.Empty(resultado);
        }



        [Fact]
        public async Task ListarTarefasPorUserId_DeveLancarArgumentNullException_QuandoUserIdForNulo()
        {
            // Arrange
            int? userId = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _tarefaService.ListarTarefasDoUsuarioAsync(userId));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public async Task ListarTarefasPorUserId_DeveLancarArgumentOutOfRangeException_QuandoUserIdForInvalido(int userId)
        {

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _tarefaService.ListarTarefasDoUsuarioAsync(userId));
        }



        [Fact]
        public async Task ListarTarefasPorUserId_DeveLancarTimeoutException_QuandoRepositoryLancarExcecao()
        {
            // Arrange
            int userId = 1;

            _tarefaRepositoryMock
                .Setup(s => s.ObterTarefasPorUserIdAsync(userId))
                .ThrowsAsync(new TimeoutException());

            // Act & Assert
            await Assert.ThrowsAsync<TimeoutException>(() =>
                _tarefaService.ListarTarefasDoUsuarioAsync(userId));
        }



        [Fact]
        public async Task ListarTarefasPorUserId_DeveLancarException_QuandoLancarExcecaoGenerica()
        {
            // Arrange
            int userId = 1;

            _tarefaRepositoryMock
                .Setup(s => s.ObterTarefasPorUserIdAsync(userId))
                .ThrowsAsync(new Exception("Erro genérico"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _tarefaService.ListarTarefasDoUsuarioAsync(userId));
        }

        #endregion




        #region BuscarTarefaPorId

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task BuscarTarefaPorId_DeveRetornarTarefa_QuandoTarefaExistir(int id)
        {
            // Arrange
            var tarefasSimulacao = new List<Tarefa>
            {
                new Tarefa { Id = 1, Titulo = "Tarefa 1" },
                new Tarefa { Id = 2, Titulo = "Tarefa 2" },
                new Tarefa { Id = 3, Titulo = "Tarefa 3" },
            };

            _tarefaRepositoryMock
                .Setup(s => s.ObterTarefaPorIdAsync(id))
                .ReturnsAsync(tarefasSimulacao.First(s => s.Id == id));

            // Act
            var resultado = await _tarefaService.BuscarTarefaPorIdAsync(id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(id, resultado.Id);
        }

        [Fact]
        public async Task BuscarTarefaPorId_DeveRetornarNull_QuandoTarefaNaoExistir()
        {
            // Arrange
            int id = 50;
            var tarefasSimulacao = new List<Tarefa>
            {
                new Tarefa { Id = 1, Titulo = "Tarefa 1" },
                new Tarefa { Id = 2, Titulo = "Tarefa 2" },
                new Tarefa { Id = 3, Titulo = "Tarefa 3" },
            };

            _tarefaRepositoryMock
                .Setup(s => s.ObterTarefaPorIdAsync(id))
                .ReturnsAsync(tarefasSimulacao.FirstOrDefault(s => s.Id == id));

            // Act
            Tarefa? tarefa = await _tarefaService.BuscarTarefaPorIdAsync(id);

            // Assert
            Assert.Null(tarefa);
        }



        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task BuscarTarefaPorId_DeveLancarArgumentOutOfRangeException_QuandoIdForInvalido(int id)
        {

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _tarefaService.BuscarTarefaPorIdAsync(id));
        }

        [Fact]
        public async Task BuscarTarefaPorId_DeveLancarArgumentNullException_QuandoIdForNulo()
        {
            // Arrange
            int? id = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _tarefaService.BuscarTarefaPorIdAsync(id));
        }

        [Fact]
        public async Task BuscarTarefaPorId_DeveLancarTimeoutException_QuandoRepositoryLancarExcecao()
        {
            // Arrange
            int id = 1;

            _tarefaRepositoryMock
                .Setup(s => s.ObterTarefaPorIdAsync(id))
                .ThrowsAsync(new TimeoutException());

            // Act & Assert
            await Assert.ThrowsAsync<TimeoutException>(() =>
                _tarefaService.BuscarTarefaPorIdAsync(id));
        }

        [Fact]
        public async Task BuscarTarefaPorId_DeveLancarException_QuandoRepositoryLancarExcecaoGenerica()
        {
            // Arrange
            int id = 1;

            _tarefaRepositoryMock
                .Setup(s => s.ObterTarefaPorIdAsync(id))
                .ThrowsAsync(new Exception("Erro genérico"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _tarefaService.BuscarTarefaPorIdAsync(id));
        }


        #endregion



        #region CriarTarefa




        [Fact]
        public async Task CriarTarefa_DeveChamarRepositorio_QuandoTarefaValida() 
        {
            // Arrange
            int userId = 1;

            List<Tarefa> simulacaoTarefas = new List<Tarefa>() 
            {
                new Tarefa { Id = 1, Titulo = "Tarefa 1", UsuarioId = userId },
                new Tarefa { Id = 2, Titulo = "Tarefa 2", UsuarioId = userId }
            };

            var novaTarefa = new Tarefa
            {
                Titulo = "Nova Tarefa",
                Descricao = "Descrição da nova tarefa",
                UsuarioId = userId
            };


            _tarefaRepositoryMock
                .Setup(s => s.AdicionarAsync(novaTarefa))
                .Callback<Tarefa>(s => simulacaoTarefas.Add(novaTarefa))
                .Returns(Task.CompletedTask);


            _tarefaRepositoryMock
                .Setup(s => s.SalvarMudancasAsync())
                .ReturnsAsync(1);

            // Act
            await _tarefaService.CriarTarefaAsync(novaTarefa, userId);

            // Assert
            Assert.Contains(novaTarefa, simulacaoTarefas);

            _tarefaRepositoryMock.Verify(
                s => s.AdicionarAsync(novaTarefa),
                Times.Once
            );

            _tarefaRepositoryMock.Verify(
                s => s.SalvarMudancasAsync(),
                Times.Once
            );

        }




        [Fact]
        public async Task CriarTarefa_DeveLancarArgumentNullException_QuandoTarefaForNula() 
        {
            // Arrange
            int userId = 1;
            Tarefa? tarefa = null;

            // Act e Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _tarefaService.CriarTarefaAsync(tarefa, userId));
        }


        [Fact]
        public async Task CriarTarefa_DeveLancarArgumentNullException_QuandoUserIdForNulo() 
        {
            // Arrange
            int? userId = null;
            var novaTarefa = new Tarefa
            {
                Id = 1,
                Titulo = "Tarefa Teste",
            };


            // Act e Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _tarefaService.CriarTarefaAsync(novaTarefa, userId));
        }




        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task CriarTarefa_DeveLancarArgumentOutOfRangeException_QuandoUserIdForInvalido(int userId) 
        {
            //Arrange
            var novaTarefa = new Tarefa 
            {
                Id = 1,
                Titulo = "Tarefa Teste",
            };

            // Act e Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _tarefaService.CriarTarefaAsync(novaTarefa, userId));
        }




        [Fact]
        public async Task CriarTarefa_DeveLancarExcecao_QuandoRepositorioLancarExcecao() 
        {
            // Arrange
            int userId = 1;
            var novaTarefa = new Tarefa 
            {
                Id = 1,
                Titulo = "Tarefa Teste",
            };

            _tarefaRepositoryMock
                .Setup(s => s.AdicionarAsync(novaTarefa))
                .ThrowsAsync(new TimeoutException());


            // Act e Assert
            await Assert.ThrowsAsync<TimeoutException>(() => _tarefaService.CriarTarefaAsync(novaTarefa, userId));
        }





        #endregion



    }
}