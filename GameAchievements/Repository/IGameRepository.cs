using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameAchievements.Models.Entities;

namespace GameAchievements.Repository
{
    public interface IGameRepository
    {
        IEnumerable<Game> GetAllGames(bool trackChanges = false);
        Game GetGame(long Id, bool trackChanges = false);
    }
}
