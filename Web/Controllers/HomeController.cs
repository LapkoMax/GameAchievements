using AutoMapper;
using DataAccess.Repository;
using DataAccess.RequestFeatures;
using Entities.DataTransferObjects;
using Entities.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.MediatRComands.Game;
using Web.MediatRComands.Genre;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public static long NewGameId { get; set; }

        public HomeController(IRepositoryManager repository, IMapper mapper, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var games = await _repository.Game.GetAllGamesAsync(new GameParameters { });
            ViewBag.MetaData = games.MetaData;
            var genresDto = await _mediator.Send(new GetGenresCommand { genreParameters = new GenreParameters { PageSize = int.MaxValue } }, CancellationToken.None);
            ViewBag.Genres = genresDto;
            var gamesDto = await _mediator.Send(new GetGamesCommand { }, CancellationToken.None);
            return View(gamesDto);
        }

        [Route("games")]
        public async Task<ActionResult> Games([FromQuery]GameParameters gameParameters)
        {
            var gamesDto = await _mediator.Send(new GetGamesCommand { gameParameters = gameParameters }, CancellationToken.None);
            return Json(gamesDto);
        }

        [Route("games/metaData")]
        public async Task<ActionResult> MetaData([FromQuery] GameParameters gameParameters)
        {
            var games = await _repository.Game.GetAllGamesAsync(gameParameters);
            return Json(games.MetaData);
        }

        [Route("games/new")]
        [HttpPost]
        public async Task<ActionResult> AddGame(GameForCreationDto game)
        {
            NewGameId = await _mediator.Send(new AddNewGameCommand { game = game }, CancellationToken.None);
            return Content("Success!");
        }

        [Route("games/addGenres")]
        [HttpPost]
        public async Task<ActionResult> UpdateGenresForGame([FromQuery]string genreIds, [FromQuery] long gameId = 0)
        {
            var gameToUpdId = gameId;
            if (gameToUpdId == 0) 
            {
                while (NewGameId == 0) Thread.Sleep(10);
                gameToUpdId = NewGameId; 
            }
            await _mediator.Send(new UpdateGenresForGameCommand { gameId = gameToUpdId, genreIds = genreIds }, CancellationToken.None);
            return Content("Success!");
        }

        [Route("games/delete")]
        [HttpPost]
        public async Task<ActionResult> DeleteGame([FromQuery]long id)
        {
            await _mediator.Send(new DeleteGameCommand { gameId = id }, CancellationToken.None);
            return Content("Success!");
        }

        [Route("games/edit")]
        public async Task<IActionResult> EditGame([FromQuery]long id)
        {
            var genresDto = await _mediator.Send(new GetGenresCommand { genreParameters = new GenreParameters { PageSize = int.MaxValue } }, CancellationToken.None);
            ViewBag.Genres = genresDto;
            var gameGenres = await _repository.GameGenres.GetAllGameGenresAsync(id);
            var gameGenreDtos = new List<GenreDto>();
            foreach(GameGenres gg in gameGenres)
            {
                var genre = await _repository.Genre.GetGenreAsync(gg.GenreId);
                gameGenreDtos.Add(_mapper.Map<GenreDto>(genre));
            }
            ViewBag.GameGenres = gameGenreDtos;
            var gameDto = await _mediator.Send(new GetGameCommand { gameId = id }, CancellationToken.None);
            ViewBag.GameId = gameDto.Id;
            return View(gameDto);
        }

        [Route("games/update")]
        [HttpPost]
        public async Task<ActionResult> UpdateGame([FromQuery]long id, GameForUpdateDto game)
        {
            await _mediator.Send(new UpdateGameCommand { gameId = id, game = game }, CancellationToken.None);
            return Content("Success!");
        }

        [Route("achievements")]
        public IActionResult GetAchievements([FromQuery]long id)
        {
            var gameId = Convert.ToInt64(id);
            return RedirectToAction("Index", "Achievement", new { gameId = gameId });
        }
    }
}
