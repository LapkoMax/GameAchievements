using AutoMapper;
using DataAccess.Repository;
using DataAccess.RequestFeatures;
using Entities.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web.MediatRComands.Achievement
{
    public class GetAchievementsCommand : IRequest<IEnumerable<AchievementDto>>
    {
        public long gameId { get; set; }
        public AchievementParameters achievementParameters { get; set; }
    }
    public class GetAchievementsCommandHandler : IRequestHandler<GetAchievementsCommand, IEnumerable<AchievementDto>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public GetAchievementsCommandHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<AchievementDto>> Handle(GetAchievementsCommand command, CancellationToken token)
        {
            AchievementParameters achievementParameters = command.achievementParameters;
            if (achievementParameters == null) achievementParameters = new AchievementParameters { };
            var achievements = await _repository.Achievements.GetAllAchievementsAsync(command.gameId, achievementParameters);
            var achievementsDto = _mapper.Map<IEnumerable<AchievementDto>>(achievements);
            return (achievementsDto);
        }
    }
}
