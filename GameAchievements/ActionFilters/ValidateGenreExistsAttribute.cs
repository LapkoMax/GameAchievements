using Core.Logger;
using Core.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ActionFilters
{
    public class ValidateGenreExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public ValidateGenreExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT") || context.HttpContext.Request.Method.Equals("PATCH");
            var id = (long)context.ActionArguments["id"];
            var genre = await _repository.Genre.GetGenreAsync(id, trackChanges);
            if (genre == null)
            {
                _logger.LogInfo($"Genre with id: {id} doesn't exists in DB.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("genre", genre);
                await next();
            }
        }
    }
}
