using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameAchievements.RequestFeatures
{
    public class GameParameters : RequestParameters
    {
        public double MinRating { get; set; }
        public double MaxRating { get; set; } = 10.0;
        public bool ValidRatingRange => MaxRating > MinRating && MinRating > 0;
    }
}
