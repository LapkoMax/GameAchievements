﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameAchievements.Models.DataTransferObjects
{
    public class AchievementForCreationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Condition { get; set; }
    }
}
