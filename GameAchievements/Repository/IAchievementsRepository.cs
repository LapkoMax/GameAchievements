using GameAchievements.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameAchievements.Repository
{
    public interface IAchievementsRepository
    {
        IEnumerable<Achievement> GetAchievements(long gameId, bool trackChanges = false);
        Achievement GetAchievement(long gameId, long Id, bool trackChanges = false);
        void CreateAchievementForGame(long gameId, Achievement achievement);
        void DeleteAchievementFromGame(Achievement achievement);
    }
}
