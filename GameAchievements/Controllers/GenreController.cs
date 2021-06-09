using AutoMapper;
using GameAchievements.ActionFilters;
using GameAchievements.LoggerService;
using GameAchievements.ModelBinders;
using GameAchievements.Models.DataTransferObjects;
using GameAchievements.Models.Entities;
using GameAchievements.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _repository.Genre.GetAllGenresAsync();
            var genresDto = _mapper.Map<IEnumerable<GenreDto>>(genres);
            return Ok(genresDto);
        }

        [HttpGet("{id}", Name = "GenreById")]
        [ServiceFilter(typeof(ValidateGenreExistsAttribute))]
        public IActionResult GetGenre(long id)
        {
            var genre = HttpContext.Items["genre"] as Genre;
            var genreDto = _mapper.Map<GenreDto>(genre);
            return Ok(genreDto);
        }

        [HttpGet("collection/({ids})", Name = "GenreCollection")]
        public async Task<IActionResult> GetGenreCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]
            IEnumerable<long> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var genreEntities = await _repository.Genre.GetGenresByIdsAsync(ids);
            if (ids.Count() != genreEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var genresToReturn = _mapper.Map<IEnumerable<GenreDto>>(genreEntities);
            return Ok(genresToReturn);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateGenre([FromBody]GenreForCreationDto genre)
        {
            var genreEntity = _mapper.Map<Genre>(genre);
            _repository.Genre.CreateGenre(genreEntity);
            await _repository.SaveAsync();
            var genreToreturn = _mapper.Map<GenreDto>(genreEntity);
            return CreatedAtRoute("GenreById", new { id = genreToreturn.Id }, genreToreturn);
        }

        [HttpPost("collection")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateGenreCollection([FromBody] IEnumerable<GenreForCreationDto> genreCollection)
        {
            var genreEntities = _mapper.Map<IEnumerable<Genre>>(genreCollection);
            foreach (var genre in genreEntities)
            {
                _repository.Genre.CreateGenre(genre);
            }
            await _repository.SaveAsync();
            var genreCollectionToReturn = _mapper.Map<IEnumerable<GenreDto>>(genreEntities);
            var ids = string.Join(",", genreCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("GenreCollection", new { ids }, genreCollectionToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateGenreExistsAttribute))]
        public async Task<IActionResult> DeleteGenre(long id)
        {
            var genre = HttpContext.Items["genre"] as Genre;
            _repository.Genre.DeleteGenre(genre);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateGenreExistsAttribute))]
        public async Task<IActionResult> UpdateGenre(long id, [FromBody]GenreForUpdateDto genre)
        {
            var genreEntity = HttpContext.Items["genre"] as Genre;
            _mapper.Map(genre, genreEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateGenreExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateGenre(long id, [FromBody]JsonPatchDocument<GenreForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogInfo("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null.");
            }
            var genre = HttpContext.Items["genre"] as Genre;
            var genreToPatch = _mapper.Map<GenreForUpdateDto>(genre);
            patchDoc.ApplyTo(genreToPatch);
            TryValidateModel(genreToPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(genreToPatch, genre);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}

