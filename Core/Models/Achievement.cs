using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Achievement
    {
        [Column("AchievementId")]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Condition { get; set; }

        [ForeignKey(nameof(Game))]
        public long GameId { get; set; }
        public Game Game { get; set; }
    }
}
