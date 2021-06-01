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
    }
}
