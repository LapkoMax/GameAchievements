using AutoMapper;
using DataAccess.Repository;
using DataAccess.RequestFeatures;
using Entities.DataTransferObjects;
using Entities.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Web.MediatRComands.Game;
using Xunit;

namespace GameAchivements.UnitTests
{
    public class GameTests
    {
        [Fact]
        public async Task ShouldReturnGameSuccess()
        {
            var command = new GetGameCommand()
            {
                gameId = Guid.NewGuid().ToByteArray().Sum(x => x)
            };

            var gameMock = new Mock<IGameRepository>();
            gameMock.Setup(repo => repo.GetGameAsync(It.IsInRange(1, long.MaxValue, Moq.Range.Inclusive), false));

            var mock = new Mock<IRepositoryManager>();
            mock.Setup(repo => repo.Game).Returns(gameMock.Object);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<GameDto>(It.IsAny<Game>())).Returns(new GameDto { Id = command.gameId });

            var commandHandler = new GetGameCommandHandler(mock.Object, mapperMock.Object);

            var gameDto = await commandHandler.Handle(command, CancellationToken.None);

            Assert.IsType<GameDto>(gameDto);
            Assert.Equal(command.gameId, gameDto.Id);
        }

        [Fact]
        public async Task ShouldReturnAllGamesSuccess()
        {
            var command = new GetGamesCommand()
            {
                gameParameters = new GameParameters { }
            };

            var gameMock = new Mock<IGameRepository>();
            gameMock.Setup(repo => repo.GetAllGamesAsync(command.gameParameters, false)).Returns(TestGames());

            var mock = new Mock<IRepositoryManager>();
            mock.Setup(repo => repo.Game).Returns(gameMock.Object);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<GameDto>>(It.IsAny<IEnumerable<Game>>())).Returns(TestGamesDto());

            var commandHandler = new GetGamesCommandHandler(mock.Object, mapperMock.Object);

            var gamesDto = await commandHandler.Handle(command, CancellationToken.None);

            Assert.NotNull(gamesDto);
            Assert.IsAssignableFrom<IEnumerable<GameDto>>(gamesDto);
            Assert.Equal(TestGamesDto().Count(), gamesDto.Count());
        }
        public async Task<PagedList<Game>> TestGames()
        {
            return new PagedList<Game>(new List<Game>{ 
                new Game
                {
                    Id = 1,
                    Name = "A",
                    Description = "A"
                },
                new Game
                {
                    Id = 2,
                    Name = "B",
                    Description = "B"
                },
                new Game
                {
                    Id = 3,
                    Name = "C",
                    Description = "C"
                }}, 3, 1, 10);
        }
        public IEnumerable<GameDto> TestGamesDto()
        {
            return new List<GameDto>
            {
                new GameDto
                {
                    Id = 1,
                    Name = "A",
                    Description = "A"
                },
                new GameDto
                {
                    Id = 2,
                    Name = "B",
                    Description = "B"
                },
                new GameDto
                {
                    Id = 3,
                    Name = "C",
                    Description = "C"
                }
            };
        }

        [Fact]
        public async Task ShouldDeleteGameSuccess()
        {
            var command = new DeleteGameCommand()
            {
                gameId = Guid.NewGuid().ToByteArray().Sum(x => x)
            };

            var gameMock = new Mock<IGameRepository>();
            gameMock.Setup(repo => repo.DeleteGame(new Game { Id = It.IsInRange(1, long.MaxValue, Moq.Range.Inclusive) }));

            var mock = new Mock<IRepositoryManager>();
            mock.Setup(repo => repo.Game).Returns(gameMock.Object);
            mock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var commandHandler = new DeleteGameCommandHandler(mock.Object);

            var id = await commandHandler.Handle(command, CancellationToken.None);

            Assert.Equal(command.gameId, id);
        }

        [Fact]
        public async Task ShouldAddNewGameSuccess()
        {
            var command = new AddNewGameCommand()
            {
                game = new GameForCreationDto { }
            };

            long gameId = Guid.NewGuid().ToByteArray().Sum(x => x);

            var gameMock = new Mock<IGameRepository>();
            gameMock.Setup(repo => repo.CreateGame(new Game { Id = gameId }));

            var mock = new Mock<IRepositoryManager>();
            mock.Setup(repo => repo.Game).Returns(gameMock.Object);
            mock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<Game>(It.IsAny<GameForCreationDto>())).Returns(new Game { Id = gameId });

            var commandHandler = new AddNewGameCommandHandler(mock.Object, mapperMock.Object);

            var id = await commandHandler.Handle(command, CancellationToken.None);

            Assert.True(id > 0);
        }

        [Fact]
        public async Task ShouldUpdateGameSuccess()
        {
            long gameId = Guid.NewGuid().ToByteArray().Sum(x => x);
            var command = new UpdateGameCommand()
            {
                gameId = gameId,
                game = new GameForUpdateDto { }
            };

            var gameMock = new Mock<IGameRepository>();
            gameMock.Setup(repo => repo.GetGameAsync(It.IsInRange(1, long.MaxValue, Moq.Range.Inclusive), true));

            var mock = new Mock<IRepositoryManager>();
            mock.Setup(repo => repo.Game).Returns(gameMock.Object);
            mock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<Game>(It.IsAny<GameForUpdateDto>())).Returns(new Game { Id = gameId });

            var commandHandler = new UpdateGameCommandHandler(mock.Object, mapperMock.Object);

            var id = await commandHandler.Handle(command, CancellationToken.None);

            Assert.True(id > 0);
            Assert.Equal(command.gameId, id);
        }

        [Fact]
        public async Task ShouldUpdateGenresForGameSuccess()
        {
            long gameId = Guid.NewGuid().ToByteArray().Sum(x => x);
            var command = new UpdateGenresForGameCommand()
            {
                gameId = gameId,
                genreIds = "1 2 3"
            };

            var ggMock = new Mock<IGameGenresRepository>();
            ggMock.Setup(repo => repo.GetAllGameGenresAsync(It.IsInRange(1, long.MaxValue, Moq.Range.Inclusive), true));

            var mock = new Mock<IRepositoryManager>();
            mock.Setup(repo => repo.GameGenres).Returns(ggMock.Object);
            mock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var commandHandler = new UpdateGenresForGameCommandHandler(mock.Object);

            var id = await commandHandler.Handle(command, CancellationToken.None);

            Assert.True(id > 0);
            Assert.Equal(command.gameId, id);
        }
    }
}
