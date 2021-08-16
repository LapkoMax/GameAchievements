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
using AngularWeb.MediatRComands.Game;
using DataAccess.Repository;
using Entities.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace AngularWeb.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public static long NewGameId { get; set; }
        public GameController(IRepositoryManager repository, IMapper mapper, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetGames([FromQuery] GameParameters gameParameters, [FromQuery]string genreIds)
        {
            GameParameters gameParametersToSort = gameParameters;
            if (!string.IsNullOrWhiteSpace(genreIds))
            {
                gameParametersToSort = new GameParameters
                {
                    OrderBy = gameParameters.OrderBy,
                    SearchTerm = gameParameters.SearchTerm,
                    MinRating = gameParameters.MinRating,
                    MaxRating = gameParameters.MaxRating,
                    PageNumber = 1,
                    PageSize = int.MaxValue
                };
            }
            var gamesDto = await _mediator.Send(new GetGamesCommand { gameParameters = gameParametersToSort, genreIds = genreIds }, CancellationToken.None);
            return Ok(gamesDto);
        }

        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> GetGame(long id)
        {
            var gameDto = await _mediator.Send(new GetGameCommand { gameId = id }, CancellationToken.None);
            return Ok(gameDto);
        }

        [HttpGet("{id}/gameGenres")]
        public async Task<IActionResult> GetGameGenres(long id)
        {
            var gameGenres = await _repository.GameGenres.GetAllGameGenresAsync(id);
            var genres = new List<GenreDto>();
            foreach(GameGenres gg in gameGenres)
            {
                genres.Add(_mapper.Map<GenreDto>(await _repository.Genre.GetGenreAsync(gg.GenreId)));
            }
            return Ok(genres);
        }

        [HttpGet("metaData"), Authorize]
        public async Task<IActionResult> GetGamesMetaData([FromQuery]GameParameters gameParameters, [FromQuery] string genreIds)
        {
            var games = await _repository.Game.GetAllGamesAsync(gameParameters);
            if (!string.IsNullOrWhiteSpace(genreIds))
            {
                GameParameters gameParametersToSort = new GameParameters
                {
                    OrderBy = gameParameters.OrderBy,
                    SearchTerm = gameParameters.SearchTerm,
                    MinRating = gameParameters.MinRating,
                    MaxRating = gameParameters.MaxRating,
                    PageNumber = 1,
                    PageSize = int.MaxValue
                };
                var gamesDto = await _mediator.Send(new GetGamesCommand { gameParameters = gameParametersToSort, genreIds = genreIds }, CancellationToken.None);
                games.MetaData.TotalCount = gamesDto.Count();
                games.MetaData.CurrentPage = 1;
                games.MetaData.PageSize = gamesDto.Count();
                games.MetaData.TotalPages = (int)Math.Ceiling(games.MetaData.TotalCount / (double)games.MetaData.PageSize);
            }
            return Ok(games.MetaData);
        }

        [HttpPut("{id}"), Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> UpdateGame(long id, [FromBody]GameForUpdateDto game)
        {
            var updGameId = await _mediator.Send(new UpdateGameCommand { gameId = id, game = game }, CancellationToken.None);
            if (updGameId != id) return BadRequest();
            return NoContent();
        }

        [HttpPut("genres/{gameId}"), Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> UpdateGenresForGame([FromQuery]string genreIds, long gameId)
        {
            var gameToUpdId = gameId;
            if (gameToUpdId == 0)
            {
                while (NewGameId == 0) Thread.Sleep(10);
                gameToUpdId = NewGameId;
            }
            await _mediator.Send(new UpdateGenresForGameCommand { gameId = gameToUpdId, genreIds = genreIds }, CancellationToken.None);
            return Content("Success!");
        }

        [HttpPost, Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> CreateGame([FromBody]GameForCreationDto game)
        {
            var gameId = await _mediator.Send(new AddNewGameCommand { game = game }, CancellationToken.None);
            if (gameId == 0) return BadRequest();
            NewGameId = gameId;
            return CreatedAtAction("GetGame", new { id = gameId });
        }

        [HttpDelete("{id}"), Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> DeleteGame(long id)
        {
            var delGameId = await _mediator.Send(new DeleteGameCommand { gameId = id }, CancellationToken.None);
            if (delGameId != id) return BadRequest();
            return Ok();
        }
    }
}
