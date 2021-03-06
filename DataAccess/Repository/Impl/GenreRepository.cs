using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using DataAccess.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using DataAccess.RequestFeatures.Extensions;

namespace DataAccess.Repository.Impl
{
    public class GenreRepository : RepositoryBase<Genre>, IGenreRepository
    {
        public GenreRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
        public async Task<PagedList<Genre>> GetAllGenresAsync(GenreParameters genreParemeters, bool trackChanges = false)
        {
            var genres = await FindAll(trackChanges)
                .Search(genreParemeters.SearchTerm)
                .Sort(genreParemeters.OrderBy)
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
                .Search(genreParemeters.SearchTerm)
                .Sort(genreParemeters.OrderBy)
                .ToListAsync();
            return PagedList<Genre>
                .ToPagedList(genres, genreParemeters.PageNumber, genreParemeters.PageSize);
        }
        public void CreateGenre(Genre genre) => Create(genre);
        public void DeleteGenre(Genre genre) => Delete(genre);
    }
}
