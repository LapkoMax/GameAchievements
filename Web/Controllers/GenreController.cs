using AutoMapper;
using DataAccess.Repository;
using DataAccess.RequestFeatures;
using Entities.DataTransferObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.MediatRComands.Genre;

namespace Web.Controllers
{
    public class GenreController : Controller
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public GenreController(IRepositoryManager repository, IMapper mapper, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<IActionResult> Index()
        {
            var genres = await _repository.Genre.GetAllGenresAsync(new GenreParameters { });
            ViewBag.MetaData = genres.MetaData;
            var genresDto = await _mediator.Send(new GetGenresCommand { }, CancellationToken.None);
            return View(genresDto);
        }

        [Route("genres")]
        public async Task<ActionResult> Genres([FromQuery]GenreParameters genreParameters)
        {
            var genreDtos = await _mediator.Send(new GetGenresCommand { genreParameters = genreParameters }, CancellationToken.None);
            return Json(genreDtos);
        }

        [Route("genres/metaData")]
        public async Task<ActionResult> MetaData([FromQuery] GenreParameters genreParameters)
        {
            var genres = await _repository.Genre.GetAllGenresAsync(genreParameters);
            return Json(genres.MetaData);
        }

        [Route("genres/new")]
        [HttpPost]
        public async Task<ActionResult> AddGenre(GenreForCreationDto genre)
        {
            await _mediator.Send(new AddNewGenreCommand { genre = genre }, CancellationToken.None);
            return Content("Success!");
        }

        [Route("genres/delete")]
        [HttpPost]
        public async Task<ActionResult> DeleteGenre([FromQuery] long genreId)
        {
            await _mediator.Send(new DeleteGenreCommand { genreId = genreId }, CancellationToken.None);
            return Content("Success!");
        }

        [Route("genres/edit")]
        public async Task<IActionResult> EditGenre([FromQuery] long genreId)
        {
            var genreDto = await _mediator.Send(new GetGenreCommand { genreId = genreId }, CancellationToken.None);
            ViewBag.GenreId = genreDto.Id;
            return View(genreDto);
        }

        [Route("genres/update")]
        [HttpPost]
        public async Task<ActionResult> UpdateGenre([FromQuery] long genreId, GenreForUpdateDto genre)
        {
            await _mediator.Send(new UpdateGenreCommand { genreId = genreId, genre = genre }, CancellationToken.None);
            return Content("Success!");
        }
    }
}
