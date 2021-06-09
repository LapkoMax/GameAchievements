using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameAchievements.Models;
using GameAchievements.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameAchievements.Repository.Impl
{
    public class AchievementRepository : RepositoryBase<Achievement>, IAchievementsRepository
    {
        public AchievementRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
        public async Task<IEnumerable<Achievement>> GetAllAchievementsAsync(long gameId, bool trackChanges = false) =>
            await FindByCondition(a => a.GameId.Equals(gameId), trackChanges)
            .OrderBy(a => a.Name)
            .ToListAsync();
        public async Task<Achievement> GetAchievementAsync(long gameId, long Id, bool trackChanges = false) =>
            await FindByCondition(a => a.GameId.Equals(gameId) && a.Id.Equals(Id), trackChanges)
            .SingleOrDefaultAsync();
        public void CreateAchievementForGame(long gameId, Achievement achievement)
        {
            achievement.GameId = gameId;
            Create(achievement);
        }
        public void DeleteAchievementFromGame(Achievement achievement) => Delete(achievement);
    }
}
