﻿using GameAchievements.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameAchievements.Repository
{
    public interface IGameGenresRepository
    {
        GameGenres GetGameGenre(long gameId, long genreId, bool trackChanges = false);
        void AddGenreForGame(GameGenres gameGenres);
        void DeleteGenreFromGame(GameGenres gameGenres);
    }
}
