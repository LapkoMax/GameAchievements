using AutoMapper;
using DataAccess.Repository;
using Entities.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AngularWeb.MediatRComands.Genre
{
    public class GetGenreCommand : IRequest<GenreDto>
    {
        public long genreId { get; set; }
    }
    public class GetGenreCommandHandler : IRequestHandler<GetGenreCommand, GenreDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public GetGenreCommandHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<GenreDto> Handle(GetGenreCommand command, CancellationToken token)
        {
            var genre = await _repository.Genre.GetGenreAsync(command.genreId);
            var genreDto = _mapper.Map<GenreDto>(genre);
            return (genreDto);
        }
    }
}
