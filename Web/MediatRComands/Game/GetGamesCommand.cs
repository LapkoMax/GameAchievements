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

namespace Web.MediatRComands.Game
{
    public class GetGamesCommand : IRequest<IEnumerable<GameDto>>
    {
        public GameParameters gameParameters { get; set; }
        public class GetGamesCommandHandler : IRequestHandler<GetGamesCommand, IEnumerable<GameDto>>
        {
            private readonly IRepositoryManager _repository;
            private readonly IMapper _mapper;
            public GetGamesCommandHandler(IRepositoryManager repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<IEnumerable<GameDto>> Handle(GetGamesCommand command, CancellationToken token)
            {
                GameParameters gameParameters = command.gameParameters;
                if (gameParameters == null) gameParameters = new GameParameters { };
                var games = await _repository.Game.GetAllGamesAsync(gameParameters);
                var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);
                return (gamesDto);
            }
        }
    }
}
