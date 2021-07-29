using DataAccess.Repository;
using Entities.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web.MediatRComands.Game
{
    public class UpdateGenresForGameCommand : IRequest<long>
    {
        public string genreIds { get; set; }
        public long gameId { get; set; }
        public class UpdateGenresForGameCommandHandler : IRequestHandler<UpdateGenresForGameCommand, long>
        {
            private readonly IRepositoryManager _repository;
            public UpdateGenresForGameCommandHandler(IRepositoryManager repository)
            {
                _repository = repository;
            }
            public async Task<long> Handle(UpdateGenresForGameCommand command, CancellationToken token)
            {
                var gameGenres = await _repository.GameGenres.GetAllGameGenresAsync(command.gameId);
                foreach (GameGenres gg in gameGenres)
                {
                    _repository.GameGenres.DeleteGenreFromGame(gg);
                }
                await _repository.SaveAsync();
                if (command.genreIds != null)
                {
                    var genreIdsList = command.genreIds.Split(' ');
                    foreach (string genreId in genreIdsList)
                    {
                        var id = Convert.ToInt64(genreId);
                        _repository.GameGenres.AddGenreForGame(new GameGenres { GameId = command.gameId, GenreId = id });
                    }
                    await _repository.SaveAsync();
                }
                return (command.gameId);
            }
        }
    }
}
