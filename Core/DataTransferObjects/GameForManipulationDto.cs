using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.DataTransferObjects
{
    public abstract class GameForManipulationDto
    {
        [Required(ErrorMessage = "Game name is a required field.")]
        [MaxLength(40, ErrorMessage = "Maximum length for the Name is 40 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Game description is a required field.")]
        [MaxLength(200, ErrorMessage = "Maximum length for the Description is 200 characters.")]
        public string Description { get; set; }

        [Range(0, 10.0, ErrorMessage = "Rating is required and it can't be lower than 0 and greater than 10")]
        public double Rating { get; set; }
        public IEnumerable<AchievementForCreationDto> Achievements { get; set; }
    }
}
