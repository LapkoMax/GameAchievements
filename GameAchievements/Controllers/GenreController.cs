using AutoMapper;
using GameAchievements.LoggerService;
using GameAchievements.Models.DataTransferObjects;
using GameAchievements.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameAchievements.Controllers
{
    [Route("api/genre")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public GenreController(IRepositoryManager repository, ILoggerManager logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetGenres()
        {
            var genres = _repository.Genre.GetGenres();
            var genresDto = _mapper.Map<IEnumerable<GenreDto>>(genres);
            return Ok(genresDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetGenre(long id)
        {
            var genre = _repository.Genre.GetGenre(id);
            if (genre == null)
            {
                _logger.LogInfo($"Genre with id: {id} doesn't exist in DB.");
                return NotFound();
            }
            else
            {
                var genreDto = _mapper.Map<GenreDto>(genre);
                return Ok(genreDto);
            }
        }
    }
}
