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
using GameAchievements.Models.Entities;
using GameAchievements.ModelBinders;
using Microsoft.AspNetCore.JsonPatch;
using GameAchievements.ActionFilters;
using GameAchievements.RequestFeatures;
using Newtonsoft.Json;
using GameAchievements.RequestFeatures.Extensions.DataShaper;

namespace GameAchievements.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<GameDto> _dataShaper;
        public GameController(IRepositoryManager repository,
            ILoggerManager logger, IMapper mapper, IDataShaper<GameDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        [HttpGet]
        public async Task<IActionResult> GetGames([FromQuery]GameParameters gameParameters)
        {
            if (!gameParameters.ValidRatingRange)
            {
                return BadRequest("Max and min ratings must be greater then 0 and max rating must be greater then min rating.");
            }
            var games = await _repository.Game.GetAllGamesAsync(gameParameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(games.MetaData));
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(_dataShaper.ShapeData(gamesDto, gameParameters.Fields));
        }

        [HttpGet("{id}", Name = "GameById")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        public IActionResult GetGame(long id, [FromQuery]GameParameters gameParameters)
        {
            var game = HttpContext.Items["game"] as Game;
            var gameDto = _mapper.Map<GameDto>(game);
            return Ok(_dataShaper.ShapeData(gameDto, gameParameters.Fields));
        }

        [HttpGet("collection/({ids})", Name = "GameCollection")]
        public async Task<IActionResult> GetGameCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]
            IEnumerable<long> ids, [FromQuery] GameParameters gameParameters)
        {
            if(ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            if (!gameParameters.ValidRatingRange)
            {
                return BadRequest("Max and min ratings must be greater then 0 and max rating must be greater then min rating.");
            }
            var gameEntities = await _repository.Game.GetGamesByIdsAsync(ids, gameParameters);
            if(ids.Count() != gameEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(gameEntities.MetaData));
            var gamesToReturn = _mapper.Map<IEnumerable<GameDto>>(gameEntities);
            return Ok(_dataShaper.ShapeData(gamesToReturn, gameParameters.Fields));
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateGame([FromBody]GameForCreationDto game)
        {
            var gameEntity = _mapper.Map<Game>(game);
            _repository.Game.CreateGame(gameEntity);
            await _repository.SaveAsync();
            var gameToReturn = _mapper.Map<GameDto>(gameEntity);
            return CreatedAtRoute("GameById", new { id = gameToReturn.Id }, gameToReturn);
        }

        [HttpPost("collection")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateGameCollection([FromBody]IEnumerable<GameForCreationDto> gameCollection)
        {
            var gameEntities = _mapper.Map<IEnumerable<Game>>(gameCollection);
            foreach(var game in gameEntities)
            {
                _repository.Game.CreateGame(game);
            }
            await _repository.SaveAsync();
            var gameCollectionToReturn = _mapper.Map<IEnumerable<GameDto>>(gameEntities);
            var ids = string.Join(",", gameCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("GameCollection", new { ids }, gameCollectionToReturn);
        }

        [HttpPost("{id}/genre")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        public async Task<IActionResult> AddGenresForGame(long id, [FromBody]IEnumerable<long> genreIds)
        {
            if(genreIds == null)
            {
                _logger.LogError("Parameter genreIds is null");
                return BadRequest("Parameter genreIds is null");
            }
            var game = HttpContext.Items["game"] as Game;
            var gameGenreIds = game.Genres.Select(g => g.Id);
            var gameGenreIdsToAdd = new List<long>();
            foreach(var genreId in genreIds)
            {
                if (!gameGenreIds.Contains(genreId))
                {
                    gameGenreIdsToAdd.Add(genreId);
                }
            }
            if(gameGenreIdsToAdd.Count() == 0)
            {
                _logger.LogInfo("Genres with this ids already contains in game");
                return Ok("Genres with this ids already contains in game");
            }
            foreach(var genreId in gameGenreIdsToAdd)
            {
                var gameGenre = new GameGenres { GameId = id, GenreId = genreId };
                _repository.GameGenres.AddGenreForGame(gameGenre);
            }
            await _repository.SaveAsync();
            game = await _repository.Game.GetGameAsync(id);
            var gameToReturn = _mapper.Map<GameDto>(game);
            return CreatedAtRoute("GameById", new { id = gameToReturn.Id }, gameToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        public async Task<IActionResult> DeleteGame(long id)
        {
            var game = HttpContext.Items["game"] as Game;
            _repository.Game.DeleteGame(game);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{id}/genre")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        public async Task<IActionResult> DeleteGenreFromGame(long id, [FromBody]IEnumerable<long> genreIds)
        {
            if (genreIds == null)
            {
                _logger.LogError("Parameter genreIds is null");
                return BadRequest("Parameter genreIds is null");
            }
            var game = HttpContext.Items["game"] as Game;
            var gameGenreIds = game.Genres.Select(g => g.GenreId);
            var gameGenreIdsToRemove = new List<long>();
            foreach (var ggId in gameGenreIds)
                Console.WriteLine(ggId);
            foreach (var genreId in genreIds)
            {
                Console.WriteLine(genreId);
                if (gameGenreIds.Contains(genreId))
                {
                    gameGenreIdsToRemove.Add(genreId);
                }
            }
            if (gameGenreIdsToRemove.Count() == 0)
            {
                _logger.LogInfo("Genres with this ids doesn't contains in game genres list");
                return Ok("Genres with this ids doesn't contains in game genres list");
            }
            foreach (var genreId in gameGenreIdsToRemove)
            {
                var gameGenre = await _repository.GameGenres.GetGameGenreAsync(id, genreId);
                _repository.GameGenres.DeleteGenreFromGame(gameGenre);
            }
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        public async Task<IActionResult> UpdateGame(long id, [FromBody]GameForUpdateDto game)
        {
            var gameEntity = HttpContext.Items["game"] as Game;
            _mapper.Map(game, gameEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateGame(long id, [FromBody]JsonPatchDocument<GameForUpdateDto> patchDoc)
        {
            if(patchDoc == null)
            {
                _logger.LogInfo("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null.");
            }
            var game = HttpContext.Items["game"] as Game;
            var gameToPatch = _mapper.Map<GameForUpdateDto>(game);
            patchDoc.ApplyTo(gameToPatch);
            TryValidateModel(gameToPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(gameToPatch, game);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
