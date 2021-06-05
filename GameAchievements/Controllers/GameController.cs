﻿using Microsoft.AspNetCore.Http;
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

namespace GameAchievements.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public GameController(IRepositoryManager repository,
            ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetGames()
        {
            var games = _repository.Game.GetAllGames();
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(gamesDto);
        }

        [HttpGet("{id}", Name = "GameById")]
        public IActionResult GetGame(long id)
        {
            var game = _repository.Game.GetGame(id);
            if(game == null)
            {
                _logger.LogInfo($"Game with id: {id} doesn't exist in DB.");
                return NotFound();
            }
            else
            {
                var gameDto = _mapper.Map<GameDto>(game);
                return Ok(gameDto);
            }
        }

        [HttpGet("collection/({ids})", Name = "GameCollection")]
        public IActionResult GetGameCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]
            IEnumerable<long> ids)
        {
            if(ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var gameEntities = _repository.Game.GetGamesByIds(ids);
            if(ids.Count() != gameEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var gamesToReturn = _mapper.Map<IEnumerable<GameDto>>(gameEntities);
            return Ok(gamesToReturn);
        }

        [HttpPost]
        public IActionResult CreateGame([FromBody]GameForCreationDto game)
        {
            if(game == null)
            {
                _logger.LogInfo("GameForCreationDto object sent from client is null.");
                return BadRequest("GameForCreationDto object is null.");
            }
            var gameEntity = _mapper.Map<Game>(game);
            _repository.Game.CreateGame(gameEntity);
            _repository.Save();
            var gameToReturn = _mapper.Map<GameDto>(gameEntity);
            return CreatedAtRoute("GameById", new { id = gameToReturn.Id }, gameToReturn);
        }

        [HttpPost("collection")]
        public IActionResult CreateGameCollection([FromBody]IEnumerable<GameForCreationDto> gameCollection)
        {
            if(gameCollection == null)
            {
                _logger.LogInfo("Game collection sent from client is null.");
                return BadRequest("Game collection is null.");
            }
            var gameEntities = _mapper.Map<IEnumerable<Game>>(gameCollection);
            foreach(var game in gameEntities)
            {
                _repository.Game.CreateGame(game);
            }
            _repository.Save();
            var gameCollectionToReturn = _mapper.Map<IEnumerable<GameDto>>(gameEntities);
            var ids = string.Join(",", gameCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("GameCollection", new { ids }, gameCollectionToReturn);
        }

        [HttpPost("{id}/genre")]
        public IActionResult AddGenresForGame(long id, [FromBody]IEnumerable<long> genreIds)
        {
            if(genreIds == null)
            {
                _logger.LogError("Parameter genreIds is null");
                return BadRequest("Parameter genreIds is null");
            }
            var game = _repository.Game.GetGame(id);
            if (game == null)
            {
                _logger.LogInfo($"Game with id: {id} doesn't exist in DB.");
                return NotFound();
            }
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
            _repository.Save();
            game = _repository.Game.GetGame(id);
            var gameToReturn = _mapper.Map<GameDto>(game);
            return CreatedAtRoute("GameById", new { id = gameToReturn.Id }, gameToReturn);
        }
    }
}
