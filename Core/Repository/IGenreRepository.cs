using Core.Models;
using Core.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IGenreRepository
    {
        Task<PagedList<Genre>> GetAllGenresAsync(GenreParameters genreParemeters, bool trackChanges = false);
        Task<Genre> GetGenreAsync(long id, bool trackChanges = false);
        Task<PagedList<Genre>> GetGenresByIdsAsync(IEnumerable<long> ids, GenreParameters genreParemeters, bool trackChanges = false);
        void CreateGenre(Genre genre);
        void DeleteGenre(Genre genre);
    }
}
