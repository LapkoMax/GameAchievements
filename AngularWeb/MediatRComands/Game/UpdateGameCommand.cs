using AutoMapper;
using DataAccess.Repository;
using Entities.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AngularWeb.MediatRComands.Game
{
    public class UpdateGameCommand : IRequest<long>
    {
        public long gameId { get; set; }
        public GameForUpdateDto game { get; set; }
    }
    public class UpdateGameCommandHandler : IRequestHandler<UpdateGameCommand, long>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public UpdateGameCommandHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<long> Handle(UpdateGameCommand command, CancellationToken token)
        {
            var gameEntity = await _repository.Game.GetGameAsync(command.gameId, true);
            _mapper.Map(command.game, gameEntity);
            await _repository.SaveAsync();
            return (command.gameId);
        }
    }
}
