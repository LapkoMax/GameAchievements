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
    public class AddNewGenreCommand : IRequest<long>
    {
        public GenreForCreationDto genre { get; set; }
    }
    public class AddNewGenreCommandHandler : IRequestHandler<AddNewGenreCommand, long>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public AddNewGenreCommandHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<long> Handle(AddNewGenreCommand command, CancellationToken token)
        {
            var genreEntity = _mapper.Map<Entities.Models.Genre>(command.genre);
            _repository.Genre.CreateGenre(genreEntity);
            await _repository.SaveAsync();
            return (genreEntity.Id);
        }
    }
}
