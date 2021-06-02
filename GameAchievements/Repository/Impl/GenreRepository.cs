using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameAchievements.Models;
using GameAchievements.Models.Entities;

namespace GameAchievements.Repository.Impl
{
    public class GenreRepository : RepositoryBase<Genre>, IGenreRepository
    {
        public GenreRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
        public IEnumerable<Genre> GetGenres(bool trackChanges = false) =>
            FindAll(trackChanges)
            .OrderBy(g => g.Name)
            .ToList();
        public Genre GetGenre(long id, bool trackChanges = false) =>
            FindByCondition(g => g.Id.Equals(id), trackChanges)
            .SingleOrDefault();
    }
}
