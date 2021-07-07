using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Game
    {
        [Column("GameId")]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public ICollection<GameGenres> Genres { get; set; }
        public ICollection<Achievement> Achievements { get; set; }
    }
}
