using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameAchievements.Models.DataTransferObjects
{
    public class GameForCreationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        //public string Genres { get; set; }
        public IEnumerable<AchievementForCreationDto> Achievements { get; set; }
    }
}
