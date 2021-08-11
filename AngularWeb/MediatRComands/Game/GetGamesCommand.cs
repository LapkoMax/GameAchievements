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

namespace AngularWeb.MediatRComands.Game
{
    public class GetGamesCommand : IRequest<IEnumerable<GameDto>>
    {
        public GameParameters gameParameters { get; set; }
        public string genreIds { get; set; }
    }
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
            if (!string.IsNullOrWhiteSpace(command.genreIds)) gameParameters.PageSize = int.MaxValue;
            var games = await _repository.Game.GetAllGamesAsync(gameParameters);
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);
            var gamesDtoToReturn = gamesDto;
            if (!string.IsNullOrWhiteSpace(command.genreIds))
            {
                var ids = command.genreIds.Split(' ');
                foreach (string id in ids)
                {
                    var genre = await _repository.Genre.GetGenreAsync(Convert.ToInt64(id.Trim()));
                    gamesDtoToReturn = gamesDtoToReturn.Where(game => game.Genres.Contains(genre.Name));
                }
            }
            return gamesDtoToReturn;
        }
    }
}
