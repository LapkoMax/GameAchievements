using AutoMapper;
using GameAchievements.LoggerService;
using GameAchievements.ModelBinders;
using GameAchievements.Models.DataTransferObjects;
using GameAchievements.Models.Entities;
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

        [HttpGet("{id}", Name = "GenreById")]
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

        [HttpGet("collection/({ids})", Name = "GenreCollection")]
        public IActionResult GetGenreCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]
            IEnumerable<long> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var genreEntities = _repository.Genre.GetGenresByIds(ids);
            if (ids.Count() != genreEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var genresToReturn = _mapper.Map<IEnumerable<GenreDto>>(genreEntities);
            return Ok(genresToReturn);
        }

        [HttpPost]
        public IActionResult CreateGenre([FromBody]GenreForCreationDto genre)
        {
            if (genre == null)
            {
                _logger.LogInfo("GenreForCreationDto object sent from client is null.");
                return BadRequest("GenreForCreationDto object is null.");
            }
            var genreEntity = _mapper.Map<Genre>(genre);
            _repository.Genre.CreateGenre(genreEntity);
            _repository.Save();
            var genreToreturn = _mapper.Map<GenreDto>(genreEntity);
            return CreatedAtRoute("GenreById", new { id = genreToreturn.Id }, genreToreturn);
        }

        [HttpPost("collection")]
        public IActionResult CreateGenreCollection([FromBody] IEnumerable<GenreForCreationDto> genreCollection)
        {
            if (genreCollection == null)
            {
                _logger.LogInfo("Genre collection sent from client is null.");
                return BadRequest("Genre collection is null.");
            }
            var genreEntities = _mapper.Map<IEnumerable<Genre>>(genreCollection);
            foreach (var genre in genreEntities)
            {
                _repository.Genre.CreateGenre(genre);
            }
            _repository.Save();
            var genreCollectionToReturn = _mapper.Map<IEnumerable<GenreDto>>(genreEntities);
            var ids = string.Join(",", genreCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("GenreCollection", new { ids }, genreCollectionToReturn);
        }
    }
}

