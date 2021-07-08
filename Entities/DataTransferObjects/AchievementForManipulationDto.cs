using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public abstract class AchievementForManipulationDto
    {
        [Required(ErrorMessage = "Achievement name is a required field.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the Name is 20 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Achievement description is a required field.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Description is 40 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Achievement obtaining condition is a required field.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Condition is 100 characters.")]
        public string Condition { get; set; }
    }
}
