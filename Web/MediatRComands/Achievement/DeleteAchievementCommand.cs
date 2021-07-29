using AutoMapper;
using DataAccess.Repository;
using Entities.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web.MediatRComands.Achievement
{
    public class DeleteAchievementCommand : IRequest<long>
    {
        public long gameId { get; set; }
        public long achievementId { get; set; }
        public class DeleteAchievementCommandHandler : IRequestHandler<DeleteAchievementCommand, long>
        {
            private readonly IRepositoryManager _repository;
            public DeleteAchievementCommandHandler(IRepositoryManager repository)
            {
                _repository = repository;
            }
            public async Task<long> Handle(DeleteAchievementCommand command, CancellationToken token)
            {
                _repository.Achievements.DeleteAchievementFromGame(new Entities.Models.Achievement { Id = command.achievementId, GameId = command.gameId });
                await _repository.SaveAsync();
                return (command.achievementId);
            }
        }
    }
}