using AutoMapper;
using Entities;
using Entities.Authentication;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AngularWeb.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthenticationManager _authManager;
        public IConfiguration Configuration { get; }
        public AuthenticationController(IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IAuthenticationManager authManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _authManager = authManager;
            Configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto
            userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _authManager.ValidateUser(user))
            {
                return Unauthorized("Authentication failed. Wrong username or password.");
            }
            var token = await _authManager.CreateToken();
            return Ok(new { Token = token });
        }

        [HttpGet("users"), Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userManager.Users;
            var usersToReturn = new List<UserForRegistrationDto>();
            foreach (User user in users) {
                usersToReturn.Add(new UserForRegistrationDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Password = "Password123",
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }
            return Ok(usersToReturn);
        }

        [HttpPut("{userName}"), Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateUser(string userName, [FromBody]UserForRegistrationDto user)
        {
            var userEntity = await _userManager.FindByNameAsync(userName);
            userEntity.FirstName = user.FirstName;
            userEntity.LastName = user.LastName;
            userEntity.UserName = user.UserName;
            userEntity.Email = user.Email;
            userEntity.PhoneNumber = user.PhoneNumber;
            var result = await _userManager.UpdateAsync(userEntity);
            return Ok(result.Errors);
        }

        [HttpDelete("{userName}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            await _userManager.DeleteAsync(user);
            return Ok("Success");
        }

        [HttpGet("roles"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserRoles([FromQuery] string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var roles= await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpGet("allRoles"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(_roleManager.Roles.ToList());
        }

        [HttpPost("allRoles"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole(IdentityRole role)
        {
            await _roleManager.CreateAsync(role);
            return Ok();
        }

        [HttpPut("allRoles/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(string id, IdentityRole role)
        {
            var roleEntity = await _roleManager.FindByIdAsync(id);
            roleEntity.Name = role.Name;
            await _roleManager.UpdateAsync(roleEntity);
            return Ok();
        }

        [HttpPut("user/{userName}/roles/{roleIds}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRoles([FromRoute]string userName, [FromRoute] string roleIds)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(roleIds))
            {
                var user = new User();
                do
                {
                    Thread.Sleep(20);
                    user = await _userManager.FindByNameAsync(userName);
                } while (user == null);
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }
                var ids = roleIds.Split(' ');
                foreach (string id in ids)
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
            }
            return Ok();
        }

        [HttpDelete("allRoles/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(role);
            return Ok();
        }
    }
}
