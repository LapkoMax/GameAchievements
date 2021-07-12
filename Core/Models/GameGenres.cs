using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class GameGenres
    {
        [Column("Id")]
        public long Id { get; set; }

        [ForeignKey(nameof(Game))]
        public long GameId { get; set; }
        public Game Game { get; set; }

        [ForeignKey(nameof(Genre))]
        public long GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
