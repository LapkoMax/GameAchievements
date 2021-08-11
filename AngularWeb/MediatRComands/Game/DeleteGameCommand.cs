using DataAccess.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AngularWeb.MediatRComands.Game
{
    public class DeleteGameCommand : IRequest<long>
    {
        public long gameId { get; set; }
    }

    public class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommand, long>
    {
        private readonly IRepositoryManager _repository;
        public DeleteGameCommandHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }
        public async Task<long> Handle(DeleteGameCommand command, CancellationToken token)
        {
            _repository.Game.DeleteGame(new Entities.Models.Game { Id = command.gameId });
            await _repository.SaveAsync();
            return command.gameId;
        }
    }
}
