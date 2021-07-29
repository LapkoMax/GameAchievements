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
    public class AddNewGameCommand : IRequest<long>
    {
        public GameForCreationDto game { get; set; }
        public class AddNewGameCommandHandler : IRequestHandler<AddNewGameCommand, long>
        {
            private readonly IRepositoryManager _repository;
            private readonly IMapper _mapper;
            public AddNewGameCommandHandler(IRepositoryManager repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<long> Handle(AddNewGameCommand command, CancellationToken token)
            {
                var gameEntity = _mapper.Map<Entities.Models.Game>(command.game);
                _repository.Game.CreateGame(gameEntity);
                await _repository.SaveAsync();
                return (gameEntity.Id);
            }
        }
    }
}
