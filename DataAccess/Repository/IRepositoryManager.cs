using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IRepositoryManager
    {
        IGameRepository Game { get; }
        IGenreRepository Genre { get; }
        IAchievementsRepository Achievements { get; }
        IGameGenresRepository GameGenres { get; }
        Task SaveAsync();
    }
}
