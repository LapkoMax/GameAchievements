using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameAchievements.Models.Entities
{
    public class Achievement
    {
        [Column("AchievementId")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Achievement name is a required field.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the Name is 20 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Achievement description is a required field.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Description is 40 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Achievement obtaining condition is a required field.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Condition is 100 characters.")]
        public string Condition { get; set; }

        [ForeignKey(nameof(Game))]
        public long GameId { get; set; }
        public Game Game { get; set; }
    }
}
