using Api.ActionFilters;
using AutoMapper;
using Core.DataTransferObjects;
using Core.Logger;
using Core.Models;
using Core.Repository;
using Core.RequestFeatures;
using Core.RequestFeatures.Extensions.DataShaper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/game/{gameId}/achievement")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
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

        /// <summary>
        /// Gets achievements for game with gameId
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="achievementParameters"></param>
        /// <returns>All achievements for game with id</returns>
        /// <response code="200">Returns the achievements list</response>
        /// <response code="404">If game with id doesn't exists in DB</response>
        [HttpGet, Authorize]
        [HttpHead]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAchievementsForGame(long gameId, [FromQuery] AchievementParameters achievementParameters)
        {
            var game = HttpContext.Items["game"] as Game;
            var achievements = await _repository.Achievements.GetAllAchievementsAsync(gameId, achievementParameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(achievements.MetaData));
            var achievementsDto = _mapper.Map<IEnumerable<AchievementDto>>(achievements);
            return Ok(_dataShaper.ShapeData(achievementsDto, achievementParameters.Fields));
        }

        /// <summary>
        /// Gets a single achievement with id for game with gameId
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="id"></param>
        /// <param name="achievementParameters"></param>
        /// <returns>Achievement with id</returns>\
        /// <response code="200">Returns the achievement with id</response>
        /// <response code="404">If game with gameId or achievement with id doesn't exists in DB</response>
        [HttpGet("{id}", Name = "GetAchievementForGame"), Authorize]
        [HttpHead("{id}")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        [ServiceFilter(typeof(ValidateAchievementExistsAttribute))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetAchievementForGame(long gameId, long id, [FromQuery]AchievementParameters achievementParameters)
        {
            var achievement = HttpContext.Items["achievement"] as Achievement;
            var achievementDto = _mapper.Map<AchievementDto>(achievement);
            return Ok(_dataShaper.ShapeData(achievementDto, achievementParameters.Fields));
        }

        /// <summary>
        /// Creates a newly created achievement for game with gameId
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="achievement"></param>
        /// <returns>A newly created achievement</returns>
        /// <response code="201">Returns the newly created achievement</response>
        /// <response code="400">If the item is null</response>
        /// <response code="404">If game with gameId doesn't exists in DB</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost, Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateAchievementForGame(long gameId, [FromBody]AchievementForCreationDto achievement)
        {
            var game = HttpContext.Items["game"] as Game;
            var achievementEntity = _mapper.Map<Achievement>(achievement);
            _repository.Achievements.CreateAchievementForGame(gameId, achievementEntity);
            await _repository.SaveAsync();
            var achievementToReturn = _mapper.Map<AchievementDto>(achievementEntity);
            return CreatedAtRoute("GetAchievementForGame", new { gameId, id = achievementToReturn.Id }, achievementToReturn);
        }

        /// <summary>
        /// Delete achievement with id from game with gameId
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">If deletion successfully</response>
        /// <response code="404">If game with gameId or achievement with id doesn't exists in DB</response>
        [HttpDelete("{id}"), Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        [ServiceFilter(typeof(ValidateAchievementExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAchievementFromGame(long gameId, long id)
        {
            var achievement = HttpContext.Items["achievement"] as Achievement;
            _repository.Achievements.DeleteAchievementFromGame(achievement);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Update achievement with id from game with gameId
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="id"></param>
        /// <param name="achievement"></param>
        /// <returns></returns>
        /// <response code="204">If updating of achievement successfull</response>
        /// <response code="400">If the item is null</response>
        /// <response code="404">If game with gameId or achievement with id doesn't exist in DB</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPut("{id}"), Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        [ServiceFilter(typeof(ValidateAchievementExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> UpdateAchievementForGame(long gameId, long id, [FromBody]AchievementForUpdateDto achievement)
        {
            var game = HttpContext.Items["game"] as Game;
            var achievementEntity = await _repository.Achievements.GetAchievementAsync(gameId, id, true);
            _mapper.Map(achievement, achievementEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Partially update achievement with id from game with gameId
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        /// <response code="204">If updating of achievement successfull</response>
        /// <response code="400">If the item is null</response>
        /// <response code="404">If game with gameId or achievement with id doesn't exist in DB</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPatch("{id}"), Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        [ServiceFilter(typeof(ValidateAchievementExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
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

        /// <summary>
        /// Gets /api/achievement options
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Response containing header with allowed methods</response>
        /// <response code="404">If game with gameId doesn't exist in DB</response>
        [HttpOptions]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetAchievementsOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }

        /// <summary>
        /// Gets /api/achievement options
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Response containing header with allowed methods</response>
        /// <response code="404">If game with gameId or achievement with id doesn't exist in DB</response>
        [HttpOptions("{id}")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        [ServiceFilter(typeof(ValidateAchievementExistsAttribute))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetAchievementOptions(long id)
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, PUT, DELETE, PATCH");
            return Ok();
        }
    }
}
