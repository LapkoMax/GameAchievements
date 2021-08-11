using AngularWeb.MediatRComands.Genre;
using DataAccess.Repository;
using DataAccess.RequestFeatures;
using Entities.DataTransferObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AngularWeb.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMediator _mediator;
        public GenreController(IRepositoryManager repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetGenres([FromQuery] GenreParameters genreParameters)
        {
            var genresDto = await _mediator.Send(new GetGenresCommand { genreParameters = genreParameters }, CancellationToken.None);
            return Ok(genresDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenre(long id)
        {
            var genreDto = await _mediator.Send(new GetGenreCommand { genreId = id }, CancellationToken.None);
            return Ok(genreDto);
        }

        [HttpGet("metaData")]
        public async Task<IActionResult> GetGenresMetaData([FromQuery] GenreParameters genreParameters)
        {
            var genres = await _repository.Genre.GetAllGenresAsync(genreParameters);
            return Ok(genres.MetaData);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(long id, [FromBody] GenreForUpdateDto genre)
        {
            var updGenreId = await _mediator.Send(new UpdateGenreCommand { genreId = id, genre = genre }, CancellationToken.None);
            if (updGenreId != id) return BadRequest();
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] GenreForCreationDto genre)
        {
            var genreId = await _mediator.Send(new AddNewGenreCommand { genre = genre }, CancellationToken.None);
            if (genreId == 0) return BadRequest();
            return CreatedAtAction("GetGenre", new { id = genreId });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(long id)
        {
            var delGenreId = await _mediator.Send(new DeleteGenreCommand { genreId = id }, CancellationToken.None);
            if (delGenreId != id) return BadRequest();
            return Ok();
        }
    }
}
