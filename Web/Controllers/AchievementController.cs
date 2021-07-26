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
    public class AchievementController : Controller
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public AchievementController(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [Route("game/achievements")]
        public async Task<IActionResult> Index([FromQuery]long gameId)
        {
            var achievements = await _repository.Achievements.GetAllAchievementsAsync(gameId, new AchievementParameters { });
            var achievementsDto = _mapper.Map<IEnumerable<AchievementDto>>(achievements);
            ViewBag.GameId = gameId;
            return View(achievementsDto);
        }

        [Route("toGames")]
        public IActionResult GoToGames()
        {
            return RedirectToAction("Index", "Home");
        }

        [Route("game/getAchievements")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> Achievements([FromQuery]long gameId)
        {
            var achievements = await _repository.Achievements.GetAllAchievementsAsync(gameId, new AchievementParameters { });
            var achievementsDto = _mapper.Map<IEnumerable<AchievementDto>>(achievements);
            return Json(achievementsDto);
        }

        [Route("game/newAchievement")]
        [HttpPost]
        public async Task<ActionResult> AddAchievement(AchievementForCreationDto achievement, [FromQuery] long gameId)
        {
            Console.WriteLine("Here");
            var achievementEntity = _mapper.Map<Achievement>(achievement);
            _repository.Achievements.CreateAchievementForGame(gameId, achievementEntity);
            await _repository.SaveAsync();
            return Content("Success!");
        }

        [Route("game/deleteAchievement")]
        [HttpPost]
        public async Task<ActionResult> DeleteAchievement([FromQuery]long gameId, [FromQuery]long achievementId)
        {
            _repository.Achievements.DeleteAchievementFromGame(new Achievement { Id = achievementId, GameId = gameId });
            await _repository.SaveAsync();
            return Content("Success!");
        }

        [Route("game/editAchievement")]
        public async Task<IActionResult> EditAchievement([FromQuery]long gameId, [FromQuery] string id)
        {
            var achievementEntityId = Convert.ToInt64(id);
            var achievement = await _repository.Achievements.GetAchievementAsync(gameId, achievementEntityId, true);
            var genreDto = _mapper.Map<AchievementDto>(achievement);
            ViewBag.GameId = gameId;
            return View(genreDto);
        }

        [Route("game/updateAchievement")]
        [HttpPost]
        public async Task<ActionResult> UpdateAchievement([FromQuery]long gameId, AchievementDto achievement)
        {
            var achievementEntityId = achievement.Id;
            var achievementEntity = await _repository.Achievements.GetAchievementAsync(gameId, achievementEntityId, true);
            var achievementForUpdate = new AchievementForUpdateDto
            {
                Name = achievement.Name,
                Description = achievement.Description,
                Condition = achievement.Condition
            };
            _mapper.Map(achievementForUpdate, achievementEntity);
            await _repository.SaveAsync();
            return Content("Success!");
        }
    }
}
