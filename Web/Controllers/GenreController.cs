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
using Web.Models;

namespace Web.Controllers
{
    public class GenreController : Controller
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public GenreController(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var genres = await _repository.Genre.GetAllGenresAsync(new GenreParameters { });
            var genresDto = _mapper.Map<IEnumerable<GenreDto>>(genres);
            return View(genresDto);
        }

        [Route("genres")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> Genres()
        {
            var genres = await _repository.Genre.GetAllGenresAsync(new GenreParameters { });
            var genreDtos = _mapper.Map<IEnumerable<GenreDto>>(genres);
            return Json(genreDtos);
        }

        [Route("genres/new")]
        [HttpPost]
        public async Task<ActionResult> AddGenre(GenreForCreationDto genre)
        {
            var genreEntity = _mapper.Map<Genre>(genre);
            _repository.Genre.CreateGenre(genreEntity);
            await _repository.SaveAsync();
            return Content("Success!");
        }

        [Route("genres/delete")]
        [HttpPost]
        public async Task<ActionResult> DeleteGenre(DataTransferModel data)
        {
            var id = Convert.ToInt64(data.GenreId);
            _repository.Genre.DeleteGenre(new Genre { Id = id });
            await _repository.SaveAsync();
            return Content("Success!");
        }

        [Route("genres/edit")]
        public async Task<IActionResult> EditGenre([FromQuery] string id)
        {
            var genreEntityId = Convert.ToInt64(id);
            var genre = await _repository.Genre.GetGenreAsync(genreEntityId);
            var genreDto = _mapper.Map<GenreDto>(genre);
            return View(genreDto);
        }

        [Route("genres/update")]
        [HttpPost]
        public async Task<ActionResult> UpdateGenre(GenreDto genre)
        {
            var genreEntityId = genre.Id;
            var genreEntity = await _repository.Genre.GetGenreAsync(genreEntityId, true);
            var genreForUpdate = new GenreForUpdateDto
            {
                Name = genre.Name,
                Description = genre.Description
            };
            _mapper.Map(genreForUpdate, genreEntity);
            await _repository.SaveAsync();
            return Content("Success!");
        }
    }
}
