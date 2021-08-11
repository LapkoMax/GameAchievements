using AngularWeb.MediatRComands.Achievement;
using AutoMapper;
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
    [Route("api/games/{gameId}/achievements")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public AchievementController(IRepositoryManager repository, IMapper mapper, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAchievements(long gameId, [FromQuery] AchievementParameters achievementParameters)
        {
            var achievementsDto = await _mediator.Send(new GetAchievementsCommand { gameId = gameId, achievementParameters = achievementParameters }, CancellationToken.None);
            return Ok(achievementsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAchievement(long gameId, long id)
        {
            var achievementDto = await _mediator.Send(new GetAchievementCommand { gameId = gameId, achievementId = id }, CancellationToken.None);
            return Ok(achievementDto);
        }

        [HttpGet("metaData")]
        public async Task<IActionResult> GetAchievementsMetaData(long gameId, [FromQuery] AchievementParameters achievementParameters)
        {
            var achievements = await _repository.Achievements.GetAllAchievementsAsync(gameId, achievementParameters);
            return Ok(achievements.MetaData);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAchievement(long gameId, long id, [FromBody] AchievementForUpdateDto achievement)
        {
            var updAchievementId = await _mediator.Send(new UpdateAchievementCommand { gameId = gameId, achievementId = id, achievement = achievement }, CancellationToken.None);
            if (updAchievementId != id) return BadRequest();
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAchievement(long gameId, [FromBody] AchievementForCreationDto achievement)
        {
            var achievementId = await _mediator.Send(new AddNewAchievementCommand { gameId = gameId, achievement = achievement }, CancellationToken.None);
            if (achievementId == 0) return BadRequest();
            return CreatedAtAction("GetAchievement", new { id = achievementId });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAchievement(long gameId, long id)
        {
            var delAchievementId = await _mediator.Send(new DeleteAchievementCommand { gameId = gameId, achievementId = id }, CancellationToken.None);
            if (delAchievementId != id) return BadRequest();
            return Ok();
        }
    }
}
