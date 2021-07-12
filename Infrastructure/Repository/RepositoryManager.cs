using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;
        private IGameRepository _gameRepository;
        private IGenreRepository _genreRepository;
        private IAchievementsRepository _achievementsRepository;
        private IGameGenresRepository _gameGenresRepository;
        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public IGameRepository Game
        {
            get
            {
                if (_gameRepository == null)
                    _gameRepository = new GameRepository(_repositoryContext);
                return _gameRepository;
            }
        }

        public IGenreRepository Genre
        {
            get
            {
                if (_genreRepository == null)
                    _genreRepository = new GenreRepository(_repositoryContext);
                return _genreRepository;
            }
        }

        public IAchievementsRepository Achievements
        {
            get
            {
                if (_achievementsRepository == null)
                    _achievementsRepository = new AchievementRepository(_repositoryContext);
                return _achievementsRepository;
            }
        }
        public IGameGenresRepository GameGenres
        {
            get
            {
                if (_gameGenresRepository == null)
                    _gameGenresRepository = new GameGenresRepository(_repositoryContext);
                return _gameGenresRepository;
            }
        }

        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
    }
}
