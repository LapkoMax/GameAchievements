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
using Web.MediatRComands.Achievement;

namespace Web.Controllers
{
    public class AchievementController : Controller
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
        [Route("game/achievements")]
        public async Task<IActionResult> Index([FromQuery]long gameId)
        {
            var achievementsDto = await _mediator.Send(new GetAchievementsCommand { gameId = gameId }, CancellationToken.None);
            ViewBag.GameId = gameId;
            return View(achievementsDto);
        }

        [Route("toGames")]
        public IActionResult GoToGames()
        {
            return RedirectToAction("Index", "Home");
        }

        [Route("game/getAchievements")]
        public async Task<ActionResult> Achievements([FromQuery]long gameId, [FromQuery]AchievementParameters achievementParameters)
        {
            var achievementsDto = await _mediator.Send(new GetAchievementsCommand { gameId = gameId, achievementParameters = achievementParameters }, CancellationToken.None);
            return Json(achievementsDto);
        }

        [Route("game/newAchievement")]
        [HttpPost]
        public async Task<ActionResult> AddAchievement(AchievementForCreationDto achievement, [FromQuery] long gameId)
        {
            await _mediator.Send(new AddNewAchievementCommand { gameId = gameId, achievement = achievement }, CancellationToken.None);
            return Content("Success!");
        }

        [Route("game/deleteAchievement")]
        [HttpPost]
        public async Task<ActionResult> DeleteAchievement([FromQuery]long gameId, [FromQuery]long achievementId)
        {
            await _mediator.Send(new DeleteAchievementCommand { gameId = gameId, achievementId = achievementId }, CancellationToken.None);
            return Content("Success!");
        }

        [Route("game/editAchievement")]
        public async Task<IActionResult> EditAchievement([FromQuery]long gameId, [FromQuery] long id)
        {
            var genreDto = await _mediator.Send(new GetAchievementCommand { gameId = gameId, achievementId = id }, CancellationToken.None);
            ViewBag.GameId = gameId;
            return View(genreDto);
        }

        [Route("game/updateAchievement")]
        [HttpPost]
        public async Task<ActionResult> UpdateAchievement([FromQuery]long gameId, [FromQuery]long achievementId, AchievementForUpdateDto achievement)
        {
            await _mediator.Send(new UpdateAchievementCommand { gameId = gameId, achievementId = achievementId, achievement = achievement }, CancellationToken.None);
            return Content("Success!");
        }
    }
}
