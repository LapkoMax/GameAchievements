using AutoMapper;
using DataAccess.Repository;
using DataAccess.RequestFeatures;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public static long NewGameId { get; set; }

        public HomeController(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var genres = await _repository.Genre.GetAllGenresAsync(new GenreParameters { });
            var genresDto = _mapper.Map<IEnumerable<GenreDto>>(genres);
            ViewBag.Genres = genresDto;
            var games = await _repository.Game.GetAllGamesAsync(new GameParameters { });
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);
            return View(gamesDto);
        }

        [Route("games")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> Games()
        {
            var games = await _repository.Game.GetAllGamesAsync(new GameParameters { });
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);
            return Json(gamesDto);
        }

        [Route("games/new")]
        [HttpPost]
        public async Task<ActionResult> AddGame(GameForCreationDto game)
        {
            var gameEntity = _mapper.Map<Game>(game);
            _repository.Game.CreateGame(gameEntity);
            await _repository.SaveAsync();
            NewGameId = gameEntity.Id;
            return Content("Success!");
        }

        [Route("games/addGenres")]
        [HttpPost]
        public async Task<ActionResult> UpdateGenresForGame([FromQuery]string genreIds, [FromQuery] long gameId = 0)
        {
            if (gameId == 0) gameId = NewGameId;
            var gameGenres = await _repository.GameGenres.GetAllGameGenresAsync(gameId);
            foreach(GameGenres gg in gameGenres)
            {
                 _repository.GameGenres.DeleteGenreFromGame(gg);
            }
            await _repository.SaveAsync();
            if (genreIds != null)
            {
                System.Threading.Thread.Sleep(1500);
                var genreIdsList = genreIds.Split(' ');
                foreach (string genreId in genreIdsList)
                {
                    var id = Convert.ToInt64(genreId);
                    _repository.GameGenres.AddGenreForGame(new GameGenres { GameId = gameId, GenreId = id });
                }
                await _repository.SaveAsync();
            }
            return Content("Success!");
        }

        [Route("games/delete")]
        [HttpPost]
        public async Task<ActionResult> DeleteGame([FromQuery]long gameId)
        {
            Console.WriteLine(gameId);
            _repository.Game.DeleteGame(new Game { Id = gameId });
            await _repository.SaveAsync();
            return Content("Success!");
        }

        [Route("games/edit")]
        public async Task<IActionResult> EditGame([FromQuery]string id)
        {
            var genres = await _repository.Genre.GetAllGenresAsync(new GenreParameters { });
            var genresDto = _mapper.Map<IEnumerable<GenreDto>>(genres);
            ViewBag.Genres = genresDto;
            var gameId = Convert.ToInt64(id);
            var game = await _repository.Game.GetGameAsync(gameId);
            var gameDto = _mapper.Map<GameDto>(game);
            return View(gameDto);
        }

        [Route("games/update")]
        [HttpPost]
        public async Task<ActionResult> UpdateGame([FromQuery]long id, GameForUpdateDto game)
        {
            var gameEntity = await _repository.Game.GetGameAsync(id, true);
            _mapper.Map(game, gameEntity);
            await _repository.SaveAsync();
            return Content("Success!");
        }

        [Route("achievements")]
        public IActionResult GetAchievements([FromQuery] string id)
        {
            var gameId = Convert.ToInt64(id);
            return RedirectToAction("Index", "Achievement", new { gameId = gameId });
        }
    }
}
