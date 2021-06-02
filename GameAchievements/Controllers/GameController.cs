using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameAchievements.Repository;
using GameAchievements.LoggerService;
using GameAchievements.Models.DataTransferObjects;
using AutoMapper;

namespace GameAchievements.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public GameController(IRepositoryManager repository,
            ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetGames()
        {
            var games = _repository.Game.GetAllGames();
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(gamesDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetGame(long id)
        {
            var game = _repository.Game.GetGame(id);
            if(game == null)
            {
                _logger.LogInfo($"Game with id: {id} doesn't exist in DB.");
                return NotFound();
            }
            else
            {
                var gameDto = _mapper.Map<GameDto>(game);
                return Ok(gameDto);
            }
        }
    }
}
