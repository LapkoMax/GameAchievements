using GameAchievements.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameAchievements.Repository
{
    public interface IAchievementsRepository
    {
        Task<IEnumerable<Achievement>> GetAllAchievementsAsync(long gameId, bool trackChanges = false);
        Task<Achievement> GetAchievementAsync(long gameId, long Id, bool trackChanges = false);
        void CreateAchievementForGame(long gameId, Achievement achievement);
        void DeleteAchievementFromGame(Achievement achievement);
    }
}
