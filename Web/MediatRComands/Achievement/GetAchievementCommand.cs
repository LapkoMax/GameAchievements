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
    public class GetAchievementCommand : IRequest<AchievementDto>
    {
        public long gameId { get; set; }
        public long achievementId { get; set; }
        public class GetAchievementCommandHandler : IRequestHandler<GetAchievementCommand, AchievementDto>
        {
            private readonly IRepositoryManager _repository;
            private readonly IMapper _mapper;
            public GetAchievementCommandHandler(IRepositoryManager repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<AchievementDto> Handle(GetAchievementCommand command, CancellationToken token)
            {
                var achievement = await _repository.Achievements.GetAchievementAsync(command.gameId, command.achievementId);
                var achievementDto = _mapper.Map<AchievementDto>(achievement);
                return (achievementDto);
            }
        }
    }
}
