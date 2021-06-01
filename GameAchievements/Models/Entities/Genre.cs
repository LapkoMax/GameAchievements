using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameAchievements.Models.Entities
{
    public class Genre
    {
        [Column("GenreId")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Genre name is a required field.")]
        [MaxLength(40, ErrorMessage = "Maximum length for the Name is 40 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Genre description is a required field.")]
        [MaxLength(200, ErrorMessage = "Maximum length for the Description is 200 characters.")]
        public string Description { get; set; }
    }
}
