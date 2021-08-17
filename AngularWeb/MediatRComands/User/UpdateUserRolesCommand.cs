using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AngularWeb.MediatRComands.User
{
    public class UpdateUserRolesCommand : IRequest<IdentityResult>
    {
        public string userName { get; set; }
        public string roleIds { get; set; }
    }
    public class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, IdentityResult>
    {
        private readonly UserManager<Entities.Models.User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UpdateUserRolesCommandHandler(UserManager<Entities.Models.User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IdentityResult> Handle(UpdateUserRolesCommand command, CancellationToken token)
        {
            if (!string.IsNullOrEmpty(command.userName) && !string.IsNullOrEmpty(command.roleIds))
            {
                var user = new Entities.Models.User();
                do
                {
                    Thread.Sleep(20);
                    user = await _userManager.FindByNameAsync(command.userName);
                } while (user == null);
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }
                var ids = command.roleIds.Split(' ');
                foreach (string id in ids)
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                return IdentityResult.Success;
            }
            return IdentityResult.Failed();
        }
    }
}
