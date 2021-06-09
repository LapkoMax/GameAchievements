using GameAchievements.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameAchievements.Repository
{
    public interface IGenreRepository
    {
        Task<IEnumerable<Genre>> GetAllGenresAsync(bool trackChanges = false);
        Task<Genre> GetGenreAsync(long id, bool trackChanges = false);
        Task<IEnumerable<Genre>> GetGenresByIdsAsync(IEnumerable<long> ids, bool trackChanges = false);
        void CreateGenre(Genre genre);
        void DeleteGenre(Genre genre);
    }
}
