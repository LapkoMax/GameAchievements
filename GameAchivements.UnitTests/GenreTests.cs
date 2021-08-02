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
using Web.MediatRComands.Genre;
using Xunit;

namespace GameAchivements.UnitTests
{
    public class GenreTests
    {
        [Fact]
        public async Task ShouldReturnGenreSuccess()
        {
            var command = new GetGenreCommand()
            {
                genreId = Guid.NewGuid().ToByteArray().Sum(x => x)
            };

            var genreMock = new Mock<IGenreRepository>();
            genreMock.Setup(repo => repo.GetGenreAsync(It.IsInRange(1, long.MaxValue, Moq.Range.Inclusive), false));

            var mock = new Mock<IRepositoryManager>();
            mock.Setup(repo => repo.Genre).Returns(genreMock.Object);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<GenreDto>(It.IsAny<Genre>())).Returns(new GenreDto { Id = command.genreId });

            var commandHandler = new GetGenreCommandHandler(mock.Object, mapperMock.Object);

            var genreDto = await commandHandler.Handle(command, CancellationToken.None);

            Assert.IsType<GenreDto>(genreDto);
            Assert.Equal(command.genreId, genreDto.Id);
        }

        [Fact]
        public async Task ShouldReturnAllGenresSuccess()
        {
            var command = new GetGenresCommand()
            {
                genreParameters = new GenreParameters { }
            };

            var genreMock = new Mock<IGenreRepository>();
            genreMock.Setup(repo => repo.GetAllGenresAsync(command.genreParameters, false)).Returns(TestGenres());

            var mock = new Mock<IRepositoryManager>();
            mock.Setup(repo => repo.Genre).Returns(genreMock.Object);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<GenreDto>>(It.IsAny<IEnumerable<Genre>>())).Returns(TestGenresDto());

            var commandHandler = new GetGenresCommandHandler(mock.Object, mapperMock.Object);

            var genresDto = await commandHandler.Handle(command, CancellationToken.None);

            Assert.NotNull(genresDto);
            Assert.IsAssignableFrom<IEnumerable<GenreDto>>(genresDto);
            Assert.Equal(TestGenresDto().Count(), genresDto.Count());
        }
        public async Task<PagedList<Genre>> TestGenres()
        {
            return new PagedList<Genre>(new List<Genre>{
                new Genre
                {
                    Id = 1,
                    Name = "A",
                    Description = "A"
                },
                new Genre
                {
                    Id = 2,
                    Name = "B",
                    Description = "B"
                },
                new Genre
                {
                    Id = 3,
                    Name = "C",
                    Description = "C"
                }}, 3, 1, 10);
        }
        public IEnumerable<GenreDto> TestGenresDto()
        {
            return new List<GenreDto>
            {
                new GenreDto
                {
                    Id = 1,
                    Name = "A",
                    Description = "A"
                },
                new GenreDto
                {
                    Id = 2,
                    Name = "B",
                    Description = "B"
                },
                new GenreDto
                {
                    Id = 3,
                    Name = "C",
                    Description = "C"
                }
            };
        }

        [Fact]
        public async Task ShouldDeleteGenreSuccess()
        {
            var command = new DeleteGenreCommand()
            {
                genreId = Guid.NewGuid().ToByteArray().Sum(x => x)
            };

            var genreMock = new Mock<IGenreRepository>();
            genreMock.Setup(repo => repo.DeleteGenre(new Genre { Id = It.IsInRange(1, long.MaxValue, Moq.Range.Inclusive) }));

            var mock = new Mock<IRepositoryManager>();
            mock.Setup(repo => repo.Genre).Returns(genreMock.Object);
            mock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var commandHandler = new DeleteGenreCommandHandler(mock.Object);

            var id = await commandHandler.Handle(command, CancellationToken.None);

            Assert.Equal(command.genreId, id);
        }

        [Fact]
        public async Task ShouldAddNewGenreSuccess()
        {
            var command = new AddNewGenreCommand()
            {
                genre = new GenreForCreationDto { }
            };

            long genreId = Guid.NewGuid().ToByteArray().Sum(x => x);

            var genreMock = new Mock<IGenreRepository>();
            genreMock.Setup(repo => repo.CreateGenre(new Genre { Id = genreId }));

            var mock = new Mock<IRepositoryManager>();
            mock.Setup(repo => repo.Genre).Returns(genreMock.Object);
            mock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<Genre>(It.IsAny<GenreForCreationDto>())).Returns(new Genre { Id = genreId });

            var commandHandler = new AddNewGenreCommandHandler(mock.Object, mapperMock.Object);

            var id = await commandHandler.Handle(command, CancellationToken.None);

            Assert.True(id > 0);
        }

        [Fact]
        public async Task ShouldUpdateGenreSuccess()
        {
            long genreId = Guid.NewGuid().ToByteArray().Sum(x => x);
            var command = new UpdateGenreCommand()
            {
                genreId = genreId,
                genre = new GenreForUpdateDto { }
            };

            var genreMock = new Mock<IGenreRepository>();
            genreMock.Setup(repo => repo.GetGenreAsync(It.IsInRange(1, long.MaxValue, Moq.Range.Inclusive), true));

            var mock = new Mock<IRepositoryManager>();
            mock.Setup(repo => repo.Genre).Returns(genreMock.Object);
            mock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<Genre>(It.IsAny<GenreForUpdateDto>())).Returns(new Genre { Id = genreId });

            var commandHandler = new UpdateGenreCommandHandler(mock.Object, mapperMock.Object);

            var id = await commandHandler.Handle(command, CancellationToken.None);

            Assert.True(id > 0);
            Assert.Equal(command.genreId, id);
        }
    }
}
