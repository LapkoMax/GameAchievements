using AutoMapper;
using GameAchievements.ActionFilters;
using GameAchievements.LoggerService;
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
    [Route("api/game/{gameId}/achievement")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<AchievementDto> _dataShaper;
        public AchievementController(IRepositoryManager repository, ILoggerManager logger,
       IMapper mapper, IDataShaper<AchievementDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        [HttpGet, Authorize]
        [HttpHead]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        public async Task<IActionResult> GetAchievementsForGame(long gameId, [FromQuery] AchievementParameters achievementParameters)
        {
            var game = HttpContext.Items["game"] as Game;
            var achievements = await _repository.Achievements.GetAllAchievementsAsync(gameId, achievementParameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(achievements.MetaData));
            var achievementsDto = _mapper.Map<IEnumerable<AchievementDto>>(achievements);
            return Ok(_dataShaper.ShapeData(achievementsDto, achievementParameters.Fields));
        }

        [HttpGet("{id}", Name = "GetAchievementForGame"), Authorize]
        [HttpHead("{id}")]
        [ServiceFilter(typeof(ValidateAchievementExistsAttribute))]
        public IActionResult GetAchievementForGame(long gameId, long id, [FromQuery]AchievementParameters achievementParameters)
        {
            var achievement = HttpContext.Items["achievement"] as Achievement;
            var achievementDto = _mapper.Map<AchievementDto>(achievement);
            return Ok(_dataShaper.ShapeData(achievementDto, achievementParameters.Fields));
        }

        [HttpPost, Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        public async Task<IActionResult> CreateAcievementForGame(long gameId, [FromBody]AchievementForCreationDto achievement)
        {
            var game = HttpContext.Items["game"] as Game;
            var achievementEntity = _mapper.Map<Achievement>(achievement);
            _repository.Achievements.CreateAchievementForGame(gameId, achievementEntity);
            await _repository.SaveAsync();
            var achievementToReturn = _mapper.Map<AchievementDto>(achievementEntity);
            return CreatedAtRoute("GetAchievementForGame", new { gameId, id = achievementToReturn.Id }, achievementToReturn);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidateAchievementExistsAttribute))]
        public async Task<IActionResult> DeleteAchievementFromGame(long gameId, long id)
        {
            var achievement = HttpContext.Items["achievement"] as Achievement;
            _repository.Achievements.DeleteAchievementFromGame(achievement);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}"), Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        public async Task<IActionResult> UpdateAchievementForGame(long gameId, long id, [FromBody]AchievementForUpdateDto achievement)
        {
            var game = HttpContext.Items["game"] as Game;
            var achievementEntity = await _repository.Achievements.GetAchievementAsync(gameId, id, true);
            _mapper.Map(achievement, achievementEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}"), Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidateAchievementExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateAchievementForGame(long gameId, long id, [FromBody]JsonPatchDocument<AchievementForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogInfo("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null.");
            }
            var achievement = HttpContext.Items["achievement"] as Achievement;
            var achievementToPatch = _mapper.Map<AchievementForUpdateDto>(achievement);
            patchDoc.ApplyTo(achievementToPatch);
            TryValidateModel(achievementToPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(achievementToPatch, achievement);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetAchievementsOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }

        [HttpOptions("{id}")]
        [ServiceFilter(typeof(ValidateAchievementExistsAttribute))]
        public IActionResult GetAchievementOptions(long id)
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, PUT, DELETE, PATCH");
            return Ok();
        }
    }
}
