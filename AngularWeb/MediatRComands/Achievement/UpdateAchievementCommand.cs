using AutoMapper;
using DataAccess.Repository;
using Entities.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AngularWeb.MediatRComands.Achievement
{
    public class UpdateAchievementCommand : IRequest<long>
    {
        public long gameId { get; set; }
        public long achievementId { get; set; }
        public AchievementForUpdateDto achievement { get; set; }
    }
    public class UpdateAchievementCommandHandler : IRequestHandler<UpdateAchievementCommand, long>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public UpdateAchievementCommandHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<long> Handle(UpdateAchievementCommand command, CancellationToken token)
        {
            var achievementEntity = await _repository.Achievements.GetAchievementAsync(command.gameId, command.achievementId, true);
            _mapper.Map(command.achievement, achievementEntity);
            await _repository.SaveAsync();
            return (command.achievementId);
        }
    }
}