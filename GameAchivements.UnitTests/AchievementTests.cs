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
using Web.MediatRComands.Achievement;
using Xunit;

namespace GameAchivements.UnitTests
{
    public class AchievementTests
    {
        [Fact]
        public async Task ShouldReturnAchievementSuccess()
        {
            var command = new GetAchievementCommand()
            {
                gameId = Guid.NewGuid().ToByteArray().Sum(x => x),
                achievementId = Guid.NewGuid().ToByteArray().Sum(x => x)
            };

            var achievementMock = new Mock<IAchievementsRepository>();
            achievementMock.Setup(repo => repo.GetAchievementAsync(It.IsInRange(1, long.MaxValue, Moq.Range.Inclusive), It.IsInRange(1, long.MaxValue, Moq.Range.Inclusive), false));

            var mock = new Mock<IRepositoryManager>();
            mock.Setup(repo => repo.Achievements).Returns(achievementMock.Object);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<AchievementDto>(It.IsAny<Achievement>())).Returns(new AchievementDto { Id = command.achievementId });

            var commandHandler = new GetAchievementCommandHandler(mock.Object, mapperMock.Object);

            var achievementDto = await commandHandler.Handle(command, CancellationToken.None);

            Assert.IsType<AchievementDto>(achievementDto);
            Assert.Equal(command.achievementId, achievementDto.Id);
        }

        [Fact]
        public async Task ShouldReturnAllAchievementsSuccess()
        {
            var command = new GetAchievementsCommand()
            {
                gameId = Guid.NewGuid().ToByteArray().Sum(x => x),
                achievementParameters = new AchievementParameters { }
            };

            var achievementMock = new Mock<IAchievementsRepository>();
            achievementMock.Setup(repo => repo.GetAllAchievementsAsync(command.gameId, command.achievementParameters, false)).Returns(TestAchievements());

            var mock = new Mock<IRepositoryManager>();
            mock.Setup(repo => repo.Achievements).Returns(achievementMock.Object);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<AchievementDto>>(It.IsAny<IEnumerable<Achievement>>())).Returns(TestAchievementsDto());

            var commandHandler = new GetAchievementsCommandHandler(mock.Object, mapperMock.Object);

            var achievementsDto = await commandHandler.Handle(command, CancellationToken.None);

            Assert.NotNull(achievementsDto);
            Assert.IsAssignableFrom<IEnumerable<AchievementDto>>(achievementsDto);
            Assert.Equal(TestAchievementsDto().Count(), achievementsDto.Count());
        }
        public async Task<PagedList<Achievement>> TestAchievements()
        {
            return new PagedList<Achievement>(new List<Achievement>{
                new Achievement
                {
                    Id = 1,
                    Name = "A",
                    Description = "A",
                    Condition = "A"
                },
                new Achievement
                {
                    Id = 2,
                    Name = "B",
                    Description = "B",
                    Condition = "B"
                },
                new Achievement
                {
                    Id = 3,
                    Name = "C",
                    Description = "C",
                    Condition = "C"
                }}, 3, 1, 10);
        }
        public IEnumerable<AchievementDto> TestAchievementsDto()
        {
            return new List<AchievementDto>
            {
                new AchievementDto
                {
                    Id = 1,
                    Name = "A",
                    Description = "A",
                    Condition = "A"
                },
                new AchievementDto
                {
                    Id = 2,
                    Name = "B",
                    Description = "B",
                    Condition = "B"
                },
                new AchievementDto
                {
                    Id = 3,
                    Name = "C",
                    Description = "C",
                    Condition = "C"
                }
            };
        }

        [Fact]
        public async Task ShouldDeleteAchievementSuccess()
        {
            var command = new DeleteAchievementCommand()
            {
                gameId = Guid.NewGuid().ToByteArray().Sum(x => x),
                achievementId = Guid.NewGuid().ToByteArray().Sum(x => x)
            };

            var achievementMock = new Mock<IAchievementsRepository>();
            achievementMock.Setup(repo => repo.DeleteAchievementFromGame(new Achievement { Id = command.achievementId, GameId = command.gameId }));

            var mock = new Mock<IRepositoryManager>();
            mock.Setup(repo => repo.Achievements).Returns(achievementMock.Object);
            mock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var commandHandler = new DeleteAchievementCommandHandler(mock.Object);

            var id = await commandHandler.Handle(command, CancellationToken.None);

            Assert.Equal(command.achievementId, id);
        }

        [Fact]
        public async Task ShouldAddNewAchievementSuccess()
        {
            var command = new AddNewAchievementCommand()
            {
                gameId = Guid.NewGuid().ToByteArray().Sum(x => x),
                achievement = new AchievementForCreationDto { }
            };

            long achievementId = Guid.NewGuid().ToByteArray().Sum(x => x);

            var achievementMock = new Mock<IAchievementsRepository>();
            achievementMock.Setup(repo => repo.CreateAchievementForGame(command.gameId, new Achievement { Id = achievementId }));

            var mock = new Mock<IRepositoryManager>();
            mock.Setup(repo => repo.Achievements).Returns(achievementMock.Object);
            mock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<Achievement>(It.IsAny<AchievementForCreationDto>())).Returns(new Achievement { Id = achievementId });

            var commandHandler = new AddNewAchievementCommandHandler(mock.Object, mapperMock.Object);

            var id = await commandHandler.Handle(command, CancellationToken.None);

            Assert.True(id > 0);
        }

        [Fact]
        public async Task ShouldUpdateAchievementSuccess()
        {
            long achievementId = Guid.NewGuid().ToByteArray().Sum(x => x);
            var command = new UpdateAchievementCommand()
            {
                gameId = Guid.NewGuid().ToByteArray().Sum(x => x),
                achievementId = achievementId,
                achievement = new AchievementForUpdateDto { }
            };

            var achievementMock = new Mock<IAchievementsRepository>();
            achievementMock.Setup(repo => repo.GetAchievementAsync(command.gameId, command.achievementId, true));

            var mock = new Mock<IRepositoryManager>();
            mock.Setup(repo => repo.Achievements).Returns(achievementMock.Object);
            mock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<Achievement>(It.IsAny<AchievementForUpdateDto>())).Returns(new Achievement { Id = achievementId });

            var commandHandler = new UpdateAchievementCommandHandler(mock.Object, mapperMock.Object);

            var id = await commandHandler.Handle(command, CancellationToken.None);

            Assert.True(id > 0);
            Assert.Equal(command.achievementId, id);
        }
    }
}
