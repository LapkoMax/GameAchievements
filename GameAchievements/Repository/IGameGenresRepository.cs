using GameAchievements.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameAchievements.Repository
{
    public interface IGameGenresRepository
    {
        void AddGenreForGame(GameGenres gameGenres);
    }
}
