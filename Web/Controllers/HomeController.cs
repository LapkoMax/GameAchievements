using AutoMapper;
using DataAccess.Repository;
using DataAccess.RequestFeatures;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public HomeController(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
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
            Console.WriteLine($"{game.Name} {game.Description} {game.Rating}");
            var gameEntity = _mapper.Map<Game>(game);
            _repository.Game.CreateGame(gameEntity);
            await _repository.SaveAsync();
            return Content("Success!");
        }
    }
}
