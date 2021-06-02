using AutoMapper;
using GameAchievements.LoggerService;
using GameAchievements.Models.DataTransferObjects;
using GameAchievements.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameAchievements.Controllers
{
    [Route("api/game/{gameId}/achievement")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public AchievementController(IRepositoryManager repository, ILoggerManager logger,
       IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAchievementsForGame(long gameId)
        {
            var game = _repository.Game.GetGame(gameId);
            if(game == null)
            {
                _logger.LogInfo($"Game with id: {gameId} doesn't exist in DB.");
                return NotFound();
            }
            else
            {
                var achievements = _repository.Achievements.GetAchievements(gameId);
                var achievementsDto = _mapper.Map<IEnumerable<AchievementDto>>(achievements);
                return Ok(achievementsDto);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetAchievementForGame(long gameId, long id)
        {
            var game = _repository.Game.GetGame(gameId);
            if (game == null)
            {
                _logger.LogInfo($"Game with id: {gameId} doesn't exist in DB.");
                return NotFound();
            }
            var achievement = _repository.Achievements.GetAchievement(gameId, id);
            if(achievement == null)
            {
                _logger.LogInfo($"Achievement with id: {id} doesn't exist in DB.");
                return NotFound();
            }
            var achievementDto = _mapper.Map<AchievementDto>(achievement);
            return Ok(achievementDto);
        }
    }
}
