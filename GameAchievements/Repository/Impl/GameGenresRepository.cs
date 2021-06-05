using GameAchievements.Models;
using GameAchievements.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameAchievements.Repository.Impl
{
    public class GameGenresRepository : RepositoryBase<GameGenres>, IGameGenresRepository
    {
        public GameGenresRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
        public void AddGenreForGame(GameGenres gameGenres) => Create(gameGenres);
    }
}
