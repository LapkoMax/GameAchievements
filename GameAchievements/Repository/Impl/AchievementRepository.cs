using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameAchievements.Models;
using GameAchievements.Models.Entities;

namespace GameAchievements.Repository.Impl
{
    public class AchievementRepository : RepositoryBase<Achievement>, IAchievementsRepository
    {
        public AchievementRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
        public IEnumerable<Achievement> GetAchievements(long gameId, bool trackChanges = false) =>
            FindByCondition(a => a.GameId.Equals(gameId), trackChanges)
            .OrderBy(a => a.Name)
            .ToList();
        public Achievement GetAchievement(long gameId, long Id, bool trackChanges = false) =>
            FindByCondition(a => a.GameId.Equals(gameId) && a.Id.Equals(Id), trackChanges)
            .SingleOrDefault();
        public void CreateAchievementForGame(long gameId, Achievement achievement)
        {
            achievement.GameId = gameId;
            Create(achievement);
        }
    }
}
