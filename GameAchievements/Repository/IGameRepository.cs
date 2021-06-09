using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameAchievements.Models.Entities;

namespace GameAchievements.Repository
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAllGamesAsync(bool trackChanges = false);
        Task<Game> GetGameAsync(long Id, bool trackChanges = false);
        Task<IEnumerable<Game>> GetGamesByIdsAsync(IEnumerable<long> ids, bool trackChanges = false);
        void CreateGame(Game game);
        void DeleteGame(Game game);
    }
}
