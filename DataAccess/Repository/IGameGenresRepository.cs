﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IGameGenresRepository
    {
        Task<GameGenres> GetGameGenreAsync(long gameId, long genreId, bool trackChanges = false);
        void AddGenreForGame(GameGenres gameGenres);
        void DeleteGenreFromGame(GameGenres gameGenres);
    }
}
