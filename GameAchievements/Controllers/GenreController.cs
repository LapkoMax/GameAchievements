using AutoMapper;
using GameAchievements.ActionFilters;
using GameAchievements.LoggerService;
using GameAchievements.ModelBinders;
using GameAchievements.Models.DataTransferObjects;
using GameAchievements.Models.Entities;
using GameAchievements.Repository;
using GameAchievements.RequestFeatures;
using GameAchievements.RequestFeatures.Extensions.DataShaper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameAchievements.Controllers
{
    [Route("api/genre")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class GenreController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<GenreDto> _dataShaper;
        public GenreController(IRepositoryManager repository, ILoggerManager logger,
            IMapper mapper, IDataShaper<GenreDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        /// <summary>
        /// Gets the list of all genres
        /// </summary>
        /// <param name="genreParameters"></param>
        /// <returns>The genres list</returns>
        /// <response code="200">Returns the genres list</response>
        [HttpGet, Authorize]
        [HttpHead]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetGenres([FromQuery]GenreParameters genreParameters)
        {
            var genres = await _repository.Genre.GetAllGenresAsync(genreParameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(genres.MetaData));
            var genresDto = _mapper.Map<IEnumerable<GenreDto>>(genres);
            return Ok(_dataShaper.ShapeData(genresDto, genreParameters.Fields));
        }

        /// <summary>
        /// Gets a single genre with id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="genreParameters"></param>
        /// <returns>Genre with id</returns>
        /// <response code="200">Returns the genre with id</response>
        /// <response code="404">If genre with id doesn't exists in DB</response>
        [HttpGet("{id}", Name = "GenreById"), Authorize]
        [HttpHead("{id}")]
        [ServiceFilter(typeof(ValidateGenreExistsAttribute))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetGenre(long id, [FromQuery]GenreParameters genreParameters)
        {
            var genre = HttpContext.Items["genre"] as Genre;
            var genreDto = _mapper.Map<GenreDto>(genre);
            return Ok(_dataShaper.ShapeData(genreDto, genreParameters.Fields));
        }

        /// <summary>
        /// Gets the list of genres with ids
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="genreParameters"></param>
        /// <returns>The list of genres with ids</returns>
        /// <response code="200">Returns the list of genres with ids</response>
        /// <response code="404">If some ids are not valid</response>
        [HttpGet("collection/({ids})", Name = "GenreCollection"), Authorize]
        [HttpHead("collection/({ids})")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetGenreCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]
            IEnumerable<long> ids, [FromQuery] GenreParameters genreParameters)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var genreEntities = await _repository.Genre.GetGenresByIdsAsync(ids, genreParameters);
            if (ids.Count() != genreEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(genreEntities.MetaData));
            var genresToReturn = _mapper.Map<IEnumerable<GenreDto>>(genreEntities);
            return Ok(_dataShaper.ShapeData(genresToReturn, genreParameters.Fields));
        }

        /// <summary>
        /// Creates a newly created genre
        /// </summary>
        /// <param name="genre"></param>
        /// <returns>A newly created genre</returns>
        /// <response code="201">Returns the newly created genre</response>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost, Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateGenre([FromBody]GenreForCreationDto genre)
        {
            var genreEntity = _mapper.Map<Genre>(genre);
            _repository.Genre.CreateGenre(genreEntity);
            await _repository.SaveAsync();
            var genreToreturn = _mapper.Map<GenreDto>(genreEntity);
            return CreatedAtRoute("GenreById", new { id = genreToreturn.Id }, genreToreturn);
        }

        /// <summary>
        /// Creates a collection of a newly created genres
        /// </summary>
        /// <param name="genreCollection"></param>
        /// <returns>The list of a newly created genres</returns>
        /// <response code="201">Returns a collection of a newly created genres</response>
        /// <response code="400">If one of the items is null</response>
        /// <response code="422">If one of the models is invalid</response>
        [HttpPost("collection"), Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
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

        /// <summary>
        /// Delete a genre with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">If deletion successfully</response>
        /// <response code="404">If genre with id doesn't exists in DB</response>
        [HttpDelete("{id}"), Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidateGenreExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteGenre(long id)
        {
            var genre = HttpContext.Items["genre"] as Genre;
            _repository.Genre.DeleteGenre(genre);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Updete a genre with id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="genre"></param>
        /// <returns></returns>
        /// <response code="204">If updating of genre successfull</response>
        /// <response code="400">If the item is null</response>
        /// <response code="404">If genre with id doesn't exist in DB</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPut("{id}"), Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateGenreExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> UpdateGenre(long id, [FromBody]GenreForUpdateDto genre)
        {
            var genreEntity = HttpContext.Items["genre"] as Genre;
            _mapper.Map(genre, genreEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Partially update a genre with id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        /// <response code="204">If updating of genre successfull</response>
        /// <response code="400">If the item is null</response>
        /// <response code="404">If genre with id doesn't exist in DB</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPatch("{id}"), Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidateGenreExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
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

        /// <summary>
        /// Gets /api/genre options
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Response containing header with allowed methods</response>
        [HttpOptions]
        [ProducesResponseType(200)]
        public IActionResult GetGenresOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }

        /// <summary>
        /// Gets /api/genre/id options
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Response containing header with allowed methods</response>
        [HttpOptions("{id}")]
        [ServiceFilter(typeof(ValidateGenreExistsAttribute))]
        [ProducesResponseType(200)]
        public IActionResult GetGenreOptions(long id)
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, PUT, DELETE, PATCH");
            return Ok();
        }
    }
}

