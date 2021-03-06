using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using DataAccess.RequestFeatures;

namespace DataAccess.Repository
{
    public interface IGameRepository
    {
        Task<PagedList<Game>> GetAllGamesAsync(GameParameters gameParameters, bool trackChanges = false);
        Task<Game> GetGameAsync(long Id, bool trackChanges = false);
        Task<PagedList<Game>> GetGamesByIdsAsync(IEnumerable<long> ids, GameParameters gameParameters, bool trackChanges = false);
        void CreateGame(Game game);
        void DeleteGame(Game game);
    }
}
