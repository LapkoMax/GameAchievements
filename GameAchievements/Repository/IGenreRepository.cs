using GameAchievements.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameAchievements.Repository
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetGenres(bool trackChanges = false);
        Genre GetGenre(long id, bool trackChanges = false);
        IEnumerable<Genre> GetGenresByIds(IEnumerable<long> ids, bool trackChanges = false);
        void CreateGenre(Genre genre);
        void DeleteGenre(Genre genre);
    }
}
