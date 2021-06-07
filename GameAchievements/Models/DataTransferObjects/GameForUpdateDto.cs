using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameAchievements.Models.DataTransferObjects
{
    public class GameForUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public IEnumerable<AchievementForCreationDto> Achievements { get; set; }
    }
}
