using DataAccess.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AngularWeb.MediatRComands.Genre
{
    public class DeleteGenreCommand : IRequest<long>
    {
        public long genreId { get; set; }
    }
    public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, long>
    {
        private readonly IRepositoryManager _repository;
        public DeleteGenreCommandHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }
        public async Task<long> Handle(DeleteGenreCommand command, CancellationToken token)
        {
            _repository.Genre.DeleteGenre(new Entities.Models.Genre { Id = command.genreId });
            await _repository.SaveAsync();
            return (command.genreId);
        }
    }
}
