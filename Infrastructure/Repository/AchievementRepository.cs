using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repository;
using Core.Models;
using Core.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Core.RequestFeatures.Extensions;

namespace Infrastructure.Repository
{
    public class AchievementRepository : RepositoryBase<Achievement>, IAchievementsRepository
    {
        public AchievementRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
        public async Task<PagedList<Achievement>> GetAllAchievementsAsync(long gameId, AchievementParameters achievementParameters, bool trackChanges = false)
        {
            var achievements = await FindByCondition(a => a.GameId.Equals(gameId), trackChanges)
                .Search(achievementParameters.SearchTerm)
                .Sort(achievementParameters.OrderBy)
                .ToListAsync();
            return PagedList<Achievement>
                .ToPagedList(achievements, achievementParameters.PageNumber, achievementParameters.PageSize);
        }
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
