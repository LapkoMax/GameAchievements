using GameAchievements.Models;
using GameAchievements.Models.Entities;
using Microsoft.EntityFrameworkCore;
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
        public async Task<GameGenres> GetGameGenreAsync(long gameId, long genreId, bool trackChanges = false) => 
            await FindByCondition(gg => gg.GameId.Equals(gameId) && gg.GenreId.Equals(genreId), trackChanges)
            .SingleOrDefaultAsync();
        public void AddGenreForGame(GameGenres gameGenres) => Create(gameGenres);
        public void DeleteGenreFromGame(GameGenres gameGenres) => Delete(gameGenres);
    }
}
