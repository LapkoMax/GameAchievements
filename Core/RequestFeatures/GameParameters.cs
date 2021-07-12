using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.RequestFeatures
{
    public class GameParameters : RequestParameters
    {
        public GameParameters()
        {
            OrderBy = "name";
        }
        public double MinRating { get; set; }
        public double MaxRating { get; set; } = 10.0;
        public bool ValidRatingRange => MaxRating > MinRating && MinRating >= 0.0 && MaxRating <= 10.0;
        public string SearchTerm { get; set; }
    }
}
