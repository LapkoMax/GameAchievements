using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameAchievements.Models;
using GameAchievements.Models.Entities;

namespace GameAchievements.Repository.Impl
{
    public class GameRepository : RepositoryBase<Game>, IGameRepository
    {
        public GameRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
    }
}
