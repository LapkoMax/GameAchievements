using Logging;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.ActionFilters
{
    public class ValidateAchievementExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public ValidateAchievementExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT") || context.HttpContext.Request.Method.Equals("PATCH");
            var gameId = (long)context.ActionArguments["gameId"];
            var game = await _repository.Game.GetGameAsync(gameId);
            if(game == null)
            {
                _logger.LogInfo($"Game with id: {gameId} doesn't exists in DB.");
                context.Result = new NotFoundResult();
                return;
            }
            var id = (long)context.ActionArguments["id"];
            var achievement = await _repository.Achievements.GetAchievementAsync(gameId, id, trackChanges);
            if(achievement == null)
            {
                _logger.LogInfo($"Achievement with id: {id} doesn't exists in DB.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("achievement", achievement);
                await next();
            }
        }
    }
}
