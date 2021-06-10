using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameAchievements.Models;
using GameAchievements.Models.Entities;
using GameAchievements.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace GameAchievements.Repository.Impl
{
    public class GenreRepository : RepositoryBase<Genre>, IGenreRepository
    {
        public GenreRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
        public async Task<PagedList<Genre>> GetAllGenresAsync(GenreParameters genreParemeters, bool trackChanges = false)
        {
            var genres = await FindAll(trackChanges)
                .OrderBy(g => g.Name)
                .ToListAsync();
            return PagedList<Genre>
                .ToPagedList(genres, genreParemeters.PageNumber, genreParemeters.PageSize);
        }
        public async Task<Genre> GetGenreAsync(long id, bool trackChanges = false) =>
            await FindByCondition(g => g.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();
        public async Task<PagedList<Genre>> GetGenresByIdsAsync(IEnumerable<long> ids, GenreParameters genreParemeters, bool trackChanges = false)
        {
            var genres = await FindByCondition(g => ids.Contains(g.Id), trackChanges)
                .OrderBy(g => g.Name)
                .ToListAsync();
            return PagedList<Genre>
                .ToPagedList(genres, genreParemeters.PageNumber, genreParemeters.PageSize);
        }
        public void CreateGenre(Genre genre) => Create(genre);
        public void DeleteGenre(Genre genre) => Delete(genre);
    }
}
