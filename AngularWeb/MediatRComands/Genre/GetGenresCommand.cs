using AutoMapper;
using DataAccess.Repository;
using DataAccess.RequestFeatures;
using Entities.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AngularWeb.MediatRComands.Genre
{
    public class GetGenresCommand : IRequest<IEnumerable<GenreDto>>
    {
        public GenreParameters genreParameters { get; set; }
    }
    public class GetGenresCommandHandler : IRequestHandler<GetGenresCommand, IEnumerable<GenreDto>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public GetGenresCommandHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GenreDto>> Handle(GetGenresCommand command, CancellationToken token)
        {
            GenreParameters genreParameters = command.genreParameters;
            if (genreParameters == null) genreParameters = new GenreParameters { };
            var genres = await _repository.Genre.GetAllGenresAsync(genreParameters);
            var genresDto = _mapper.Map<IEnumerable<GenreDto>>(genres);
            return (genresDto);
        }
    }
}
