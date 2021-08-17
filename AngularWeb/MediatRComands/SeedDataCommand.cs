using AngularWeb.MediatRComands.Achievement;
using AngularWeb.MediatRComands.Game;
using AngularWeb.MediatRComands.Genre;
using AngularWeb.MediatRComands.User;
using DataAccess.Repository;
using Entities.DataTransferObjects;
using Entities.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AngularWeb.MediatRComands
{
    public class SeedDataCommand : IRequest<int>
    {
        public string tableName { get; set; }
        public int count { get; set; }
        public long gameId { get; set; }
    }
    public class SeedDataCommandHandler : IRequestHandler<SeedDataCommand, int>
    {
        private readonly IMediator _mediator;
        RoleManager<IdentityRole> _roleManager;
        public SeedDataCommandHandler(IMediator mediator, RoleManager<IdentityRole> roleManager)
        {
            _mediator = mediator;
            _roleManager = roleManager;
        }
        public async Task<int> Handle(SeedDataCommand command, CancellationToken token)
        {
            int count = 0;
            switch (command.tableName.ToLower())
            {
                case "game":
                    var rand = new Random();
                    for(int i = 0; i < command.count; i++)
                    {
                        await _mediator.Send(new AddNewGameCommand { game = new GameForCreationDto { Name = "Game" + i, Description = "This is Game" + i, Rating = Math.Round(rand.NextDouble() * 10, 1) } }, CancellationToken.None);
                        count++;
                    }
                    break;
                case "genre":
                    for (int i = 0; i < command.count; i++)
                    {
                        await _mediator.Send(new AddNewGenreCommand { genre = new GenreForCreationDto { Name = "Genre" + i, Description = "This is Genre" + i } }, CancellationToken.None);
                        count++;
                    }
                    break;
                case "achievement":
                    for (int i = 0; i < command.count; i++)
                    {
                        await _mediator.Send(new AddNewAchievementCommand { gameId = command.gameId, achievement = new AchievementForCreationDto { Name = "Achievement" + i, Description = "This is Achievement" + i, Condition = "This is Achievement" + i } }, CancellationToken.None);
                        count++;
                    }
                    break;
                case "user":
                    for (int i = 0; i < command.count; i++)
                    {
                        await _mediator.Send(new AddNewUserCommand { user = new UserForRegistrationDto { FirstName = "User", LastName = "User", UserName = "NewUser" + i, Email = "newuser" + i + "@mail.com", PhoneNumber = "123-456-789", Password = "NewPassword123", Roles = new List<string> { "User" } } }, CancellationToken.None);
                        count++;
                    }
                    break;
                case "role":
                    for (int i = 0; i < command.count; i++)
                    {
                        await _roleManager.CreateAsync(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Role" + i });
                        count++;
                    }
                    break;
            }
            return count;
        }
    }
}
