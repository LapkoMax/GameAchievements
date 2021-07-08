using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using DataAccess.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using DataAccess.RequestFeatures.Extensions;

namespace DataAccess.Repository.Impl
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
        public async Task<PagedList<Game>> GetAllGamesAsync(GameParameters gameParameters, bool trackChanges = false)
        {
            var games = await FindAll(trackChanges)
                .FilterGames(gameParameters.MinRating, gameParameters.MaxRating)
                .Search(gameParameters.SearchTerm)
                .Sort(gameParameters.OrderBy)
                .ToListAsync();
            return PagedList<Game>
                .ToPagedList(games, gameParameters.PageNumber, gameParameters.PageSize);
        }
        public async Task<Game> GetGameAsync(long Id, bool trackChanges = false) =>
            await FindByCondition(g => g.Id.Equals(Id), trackChanges)
            .SingleOrDefaultAsync();
        public async Task<PagedList<Game>> GetGamesByIdsAsync(IEnumerable<long> ids, GameParameters gameParameters, bool trackChanges = false)
        {
            var games = await FindByCondition(g => ids.Contains(g.Id), trackChanges)
                .FilterGames(gameParameters.MinRating, gameParameters.MaxRating)
                .Search(gameParameters.SearchTerm)
                .Sort(gameParameters.OrderBy)
                .ToListAsync();
            return PagedList<Game>
                .ToPagedList(games, gameParameters.PageNumber, gameParameters.PageSize);
        }
        public void CreateGame(Game game) => Create(game);
        public void DeleteGame(Game game) => Delete(game);
    }
}
