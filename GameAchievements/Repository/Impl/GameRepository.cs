using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameAchievements.Models;
using GameAchievements.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameAchievements.Repository.Impl
{
    public class GameRepository : RepositoryBase<Game>, IGameRepository
    {
        public GameRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
        public new IQueryable<Game> FindAll(bool trackChanges = false) =>
            !trackChanges ?
            RepositoryContext.Games.Include(g => g.Genres).ThenInclude(genre => genre.Genre).Include(g => g.Achievements)
            .AsNoTracking() :
            RepositoryContext.Games.Include(g => g.Genres).ThenInclude(genre => genre.Genre).Include(g => g.Achievements);
        public new IQueryable<Game> FindByCondition(Expression<Func<Game, bool>> expression, bool trackChanges = false) =>
            !trackChanges ?
            RepositoryContext.Games.Include(g => g.Genres).ThenInclude(genre => genre.Genre).Include(g => g.Achievements)
            .Where(expression)
            .AsNoTracking() :
            RepositoryContext.Games.Include(g => g.Genres).ThenInclude(genre => genre.Genre).Include(g => g.Achievements)
            .Where(expression);
        public IEnumerable<Game> GetAllGames(bool trackChanges = false) =>
            FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToList();
        public Game GetGame(long Id, bool trackChanges = false) =>
            FindByCondition(g => g.Id.Equals(Id), trackChanges)
            .SingleOrDefault();
        public IEnumerable<Game> GetGamesByIds(IEnumerable<long> ids, bool trackChanges = false) =>
            FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToList();
        public void CreateGame(Game game) => Create(game);
    }
}
