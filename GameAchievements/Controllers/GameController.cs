using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository;
using Logging;
using Entities.DataTransferObjects;
using AutoMapper;
using Entities.Models;
using BusinessLogic.ModelBinders;
using Microsoft.AspNetCore.JsonPatch;
using BusinessLogic.ActionFilters;
using DataAccess.RequestFeatures;
using Newtonsoft.Json;
using DataAccess.RequestFeatures.Extensions.DataShaper;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Route("api/game")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
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

        /// <summary>
        /// Gets the list of all games
        /// </summary>
        /// <param name="gameParameters"></param>
        /// <returns>The games list</returns>
        /// <response code="200">If everything fine</response>
        /// <response code="400">If query parameters are wrong</response>
        [HttpGet, Authorize]
        [HttpHead]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
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

        /// <summary>
        /// Gets single game by gameId
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="gameParameters"></param>
        /// <returns>Game by gameId</returns>
        /// <response code="200">If everything fine</response>
        /// <response code="404">If game with gameId doesn't exist</response>
        [HttpGet("{gameId}", Name = "GameById"), Authorize]
        [HttpHead("{id}")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetGame(long gameId, [FromQuery]GameParameters gameParameters)
        {
            var game = HttpContext.Items["game"] as Game;
            var gameDto = _mapper.Map<GameDto>(game);
            return Ok(_dataShaper.ShapeData(gameDto, gameParameters.Fields));
        }

        /// <summary>
        /// Gets the list of games by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="gameParameters"></param>
        /// <returns>The list of games by ids</returns>
        /// <response code="200">If everything fine</response>
        /// <response code="400">If parameter ids is null or game parameters are wrong</response>
        /// <response code="404">If some ids are not valid</response>
        [HttpGet("collection/({ids})", Name = "GameCollection"), Authorize]
        [HttpHead("collection/({ids})")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Creates a newly created game
        /// </summary>
        /// <param name="game"></param>
        /// <returns>A newly created game</returns>
        /// <response code="201">Returns the newly created game</response>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost, Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateGame([FromBody]GameForCreationDto game)
        {
            var gameEntity = _mapper.Map<Game>(game);
            _repository.Game.CreateGame(gameEntity);
            await _repository.SaveAsync();
            var gameToReturn = _mapper.Map<GameDto>(gameEntity);
            return CreatedAtRoute("GameById", new { id = gameToReturn.Id }, gameToReturn);
        }

        /// <summary>
        /// Creates a collection of a newly created games
        /// </summary>
        /// <param name="gameCollection"></param>
        /// <returns>A collection of a newly created games</returns>
        /// <response code="201">Returns a collection of a newly created games</response>
        /// <response code="400">If one of the items is null</response>
        /// <response code="422">If one of the models is invalid</response>
        [HttpPost("collection"), Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
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

        /// <summary>
        /// Adds a list of genres with ids to game
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="genreIds"></param>
        /// <returns>Game that added genres</returns>
        /// <response code="200">If genres with ids are already contains in game</response>
        /// <response code="201">Returns a game that added genres</response>
        /// <response code="400">If parameter ids is null</response>
        /// <response code="404">If game with gameId doesn't exist in DB or one of genre ids is invalid</response>
        [HttpPost("{gameId}/genre"), Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddGenresForGame(long gameId, [FromBody]IEnumerable<long> genreIds)
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
                if (await _repository.Genre.GetGenreAsync(genreId) == null)
                    return NotFound($"Genre with id: {genreId} doesnt exists in DB");
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
                var gameGenre = new GameGenres { GameId = gameId, GenreId = genreId };
                _repository.GameGenres.AddGenreForGame(gameGenre);
            }
            await _repository.SaveAsync();
            game = await _repository.Game.GetGameAsync(gameId);
            var gameToReturn = _mapper.Map<GameDto>(game);
            return CreatedAtRoute("GameById", new { id = gameToReturn.Id }, gameToReturn);
        }

        /// <summary>
        /// Delete a game with gameId
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        /// <response code="204">If deletion successfully</response>
        /// <response code="404">If game with gameId doesn't exists in DB</response>
        [HttpDelete("{gameId}"), Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteGame(long gameId)
        {
            var game = HttpContext.Items["game"] as Game;
            _repository.Game.DeleteGame(game);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Remove genres with ids from game
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="genreIds"></param>
        /// <returns></returns>
        /// <response code="200">If genres with ids doesn't contains in game</response>
        /// <response code="201">Returns a game that removed genres</response>
        /// <response code="400">If parameter ids is null</response>
        /// <response code="404">If game with gameId doesn't exist in DB or one of genre ids are invalid</response>
        [HttpDelete("{gameId}/genre"), Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteGenreFromGame(long gameId, [FromBody]IEnumerable<long> genreIds)
        {
            if (genreIds == null)
            {
                _logger.LogError("Parameter genreIds is null");
                return BadRequest("Parameter genreIds is null");
            }
            var game = HttpContext.Items["game"] as Game;
            var gameGenreIds = game.Genres.Select(g => g.GenreId);
            var gameGenreIdsToRemove = new List<long>();
            foreach (var genreId in genreIds)
            {
                if (await _repository.Genre.GetGenreAsync(genreId) == null)
                    return NotFound($"Genre with id: {genreId} doesnt exists in DB");
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
                var gameGenre = await _repository.GameGenres.GetGameGenreAsync(gameId, genreId);
                _repository.GameGenres.DeleteGenreFromGame(gameGenre);
            }
            await _repository.SaveAsync();
            game = await _repository.Game.GetGameAsync(gameId);
            var gameToReturn = _mapper.Map<GameDto>(game);
            return CreatedAtRoute("GameById", new { id = gameToReturn.Id }, gameToReturn);
        }

        /// <summary>
        /// Update a game with gameId
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        /// <response code="204">If updating of game successfull</response>
        /// <response code="400">If the item is null</response>
        /// <response code="404">If game with gameId doesn't exist in DB</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPut("{gameId}"), Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> UpdateGame(long gameId, [FromBody]GameForUpdateDto game)
        {
            var gameEntity = HttpContext.Items["game"] as Game;
            _mapper.Map(game, gameEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Partially update game with gameId
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        /// <response code="204">If updating of game successfull</response>
        /// <response code="400">If the item is null</response>
        /// <response code="404">If game with gameId doesn't exist in DB</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPatch("{gameId}"), Authorize(Roles = "Manager,Admin")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> PartiallyUpdateGame(long gameId, [FromBody]JsonPatchDocument<GameForUpdateDto> patchDoc)
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

        /// <summary>
        /// Gets /api/game options
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Response containing header with allowed methods</response>
        [HttpOptions]
        [ProducesResponseType(200)]
        public IActionResult GetGamesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }

        /// <summary>
        /// Gets /api/game/gameId options
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        /// <response code="200">Response containing header with allowed methods</response>
        /// <response code="404">If game with gameId doesn't exists in DB</response>
        [HttpOptions("{gameId}")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetGameOptions(long gameId)
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, PUT, DELETE, PATCH");
            return Ok();
        }

        /// <summary>
        /// Gets /api/game/gameId/genre options
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        /// <response code="200">Response containing header with allowed methods</response>
        /// <response code="404">If game with gameId doesn't exists in DB</response>
        [HttpOptions("{gameId}/genre")]
        [ServiceFilter(typeof(ValidateGameExistsAttribute))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetGameGenresOptions(long gameId)
        {
            Response.Headers.Add("Allow", "OPTIONS, POST, DELETE");
            return Ok();
        }
    }
}
