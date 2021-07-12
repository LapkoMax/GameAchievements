using Core.Models;
using Core.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IAchievementsRepository
    {
        Task<PagedList<Achievement>> GetAllAchievementsAsync(long gameId, AchievementParameters achievementParameters, bool trackChanges = false);
        Task<Achievement> GetAchievementAsync(long gameId, long Id, bool trackChanges = false);
        void CreateAchievementForGame(long gameId, Achievement achievement);
        void DeleteAchievementFromGame(Achievement achievement);
    }
}
