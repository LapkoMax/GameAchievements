using AutoMapper;
using DataAccess.Repository;
using Entities.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web.MediatRComands.Game
{
    public class GetGameCommand : IRequest<GameDto>
    {
        public long gameId { get; set; }
        public class GetGameCommandHandler : IRequestHandler<GetGameCommand, GameDto>
        {
            private readonly IRepositoryManager _repository;
            private readonly IMapper _mapper;
            public GetGameCommandHandler(IRepositoryManager repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<GameDto> Handle(GetGameCommand command, CancellationToken token)
            {
                var game = await _repository.Game.GetGameAsync(command.gameId);
                var gameDto = _mapper.Map<GameDto>(game);
                return (gameDto);
            }
        }
    }
}
