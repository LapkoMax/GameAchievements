using AutoMapper;
using DataAccess.Repository;
using Entities.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web.MediatRComands.Achievement
{
    public class AddNewAchievementCommand : IRequest<long>
    {
        public long gameId { get; set; }
        public AchievementForCreationDto achievement { get; set; }
        public class AddNewAchievementCommandHandler : IRequestHandler<AddNewAchievementCommand, long>
        {
            private readonly IRepositoryManager _repository;
            private readonly IMapper _mapper;
            public AddNewAchievementCommandHandler(IRepositoryManager repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<long> Handle(AddNewAchievementCommand command, CancellationToken token)
            {
                var achievementEntity = _mapper.Map<Entities.Models.Achievement>(command.achievement);
                _repository.Achievements.CreateAchievementForGame(command.gameId, achievementEntity);
                await _repository.SaveAsync();
                return (achievementEntity.Id);
            }
        }
    }
}
