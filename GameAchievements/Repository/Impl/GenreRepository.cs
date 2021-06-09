using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameAchievements.Models;
using GameAchievements.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameAchievements.Repository.Impl
{
    public class GenreRepository : RepositoryBase<Genre>, IGenreRepository
    {
        public GenreRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
        public async Task<IEnumerable<Genre>> GetAllGenresAsync(bool trackChanges = false) =>
            await FindAll(trackChanges)
            .OrderBy(g => g.Name)
            .ToListAsync();
        public async Task<Genre> GetGenreAsync(long id, bool trackChanges = false) =>
            await FindByCondition(g => g.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();
        public async Task<IEnumerable<Genre>> GetGenresByIdsAsync(IEnumerable<long> ids, bool trackChanges = false) =>
            await FindByCondition(g => ids.Contains(g.Id), trackChanges)
            .ToListAsync();
        public void CreateGenre(Genre genre) => Create(genre);
        public void DeleteGenre(Genre genre) => Delete(genre);
    }
}
