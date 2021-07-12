using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Genre
    {
        [Column("GenreId")]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
