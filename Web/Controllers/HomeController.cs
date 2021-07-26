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
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public static long gameId { get; set; }

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
            gameId = gameEntity.Id;
            return Content("Success!");
        }

        [Route("games/addGenres")]
        [HttpPost]
        public async Task<ActionResult> UpdateGenresForGame(DataTransferModel data)
        {
            var gameGenres = await _repository.GameGenres.GetAllGameGenresAsync(gameId);
            foreach(GameGenres gg in gameGenres)
            {
                 _repository.GameGenres.DeleteGenreFromGame(gg);
            }
            await _repository.SaveAsync();
            if (data.GenreIds != null)
            {
                System.Threading.Thread.Sleep(1500);
                var genreIds = data.GenreIds.Split(' ');
                foreach (string genreId in genreIds)
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
        public async Task<ActionResult> DeleteGame(DataTransferModel data)
        {
            var id = Convert.ToInt64(data.GameId);
            _repository.Game.DeleteGame(new Game { Id = id });
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
        public async Task<ActionResult> UpdateGame(GameDto game)
        {
            var gameEntityId = game.Id;
            gameId = gameEntityId;
            var gameEntity = await _repository.Game.GetGameAsync(gameEntityId, true);
            var gameForUpdate = new GameForUpdateDto
            {
                Name = game.Name,
                Description = game.Description,
                Rating = game.Rating
            };
            _mapper.Map(gameForUpdate, gameEntity);
            await _repository.SaveAsync();
            return Content("Success!");
        }
    }
}
