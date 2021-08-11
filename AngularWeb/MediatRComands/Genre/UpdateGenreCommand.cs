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
    public class UpdateGenreCommand : IRequest<long>
    {
        public long genreId { get; set; }
        public GenreForUpdateDto genre { get; set; }
    }
    public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, long>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public UpdateGenreCommandHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<long> Handle(UpdateGenreCommand command, CancellationToken token)
        {
            var genreEntity = await _repository.Genre.GetGenreAsync(command.genreId, true);
            _mapper.Map(command.genre, genreEntity);
            await _repository.SaveAsync();
            return (command.genreId);
        }
    }
}
