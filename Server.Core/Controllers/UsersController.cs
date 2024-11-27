using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Core.DTOs;
using Server.Core.Entities;
using Server.Core.Interfaces;

namespace Server.Core.Controllers;

/// <summary>
/// Controller responsible for managing user api calls.
/// </summary>
public class UsersController(IUnitOfWork unitOfWork, UserManager<User> userManager) : BaseApiController
{
    /// <summary>
    /// Retrieves a list of all users.
    /// </summary>
    /// <returns>An <see cref="IEnumerable"/> of <see cref="UserDto"/> containing all user details.</returns>
    [HttpGet, Authorize(Roles = "Admin, Superman")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await unitOfWork.UserRepository.GetUsersAsync();
        return Ok(users);
    }

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="userName">The username of the user to retrieve.</param>
    /// <returns>A <see cref="UserDto"/> object if found; otherwise, <see cref="StatusCodes.Status404NotFound"/>.</returns>
    [HttpGet("{userName}"), Authorize]
    public async Task<ActionResult<UserDto>> GetUser(string userName)
    {
        var user = await unitOfWork.UserRepository.GetUserByUserNameAsync(userName);
        if (user == null) return NotFound("User not found");

        var username = User.FindFirstValue(ClaimTypes.Name);
        if (user.UserName != username)
        {
            return Forbid("Access denied.");
        }

        return Ok(user);
    }

    /// <summary>
    /// Retrieves a user by their unique ID.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <returns>A <see cref="UserDto"/> object if found; otherwise, <see cref="StatusCodes.Status404NotFound"/>.</returns>
    [HttpGet("{userId:int}"), Authorize]
    public async Task<ActionResult> GetUserById([FromRoute] int userId)
    {
        var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (id != userId)
        {
            return Forbid("Access denied.");
        }

        var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);
        if (user == null) return NotFound("User not found");
        return Ok(user);
    }

    /// <summary>
    /// Updates the details of an existing user.
    /// </summary>
    /// <param name="userId">The ID of the user to update.</param>
    /// <param name="userDto">The updated user details.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the update operation.</returns>
    [HttpPut("{userId:int}"), Authorize]
    public async Task<ActionResult> UpdateUser(UserDto userDto)
    {
        var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (id != userDto.Id)
        {
            return Forbid("Access denied.");
        }

        var user = await unitOfWork.UserRepository.GetUserByIdAsync(userDto.Id);
        if (user == null) return NotFound("User not found");
        
        user.Email = userDto.Email ?? user.Email;
        user.PhoneNumber = userDto.PhoneNumber ?? user.PhoneNumber;
        user.UserName = userDto.UserName ?? user.UserName;
        
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded) return BadRequest("Failed to update user");
        
        return Ok();
    }

    /// <summary>
    /// Deletes a user by their unique ID.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the delete operation.</returns>
    [HttpDelete("{userId:int}"), Authorize]
    public async Task<ActionResult> DeleteUser(int userId)
    {
        var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (id != userId)
        {
            return Forbid("Access denied.");
        }
        
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);
        if (user == null) return NotFound("User not found");

        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded) return BadRequest("Failed to delete user");
        
        return Ok();
    }

    /// <summary>
    /// Retrieves the roles associated with a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve roles for.</param>
    /// <returns>An <see cref="IEnumerable{Store}"/> containing the user's roles, or <see cref="NotFound"/> if the user is not found.</returns>
    [HttpGet("{userId:int}/roles"), Authorize]
    public async Task<ActionResult<IEnumerable<Store>>> GetUserRoles(int userId)
    {
        var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (id != userId)
        {
            return Forbid("Access denied.");
        }
        
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);
        if (user == null) return NotFound("User not found");
        
        var roles = await userManager.GetRolesAsync(user);
        return Ok(roles);
    }

    /// <summary>
    /// Retrieves the stores associated with a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve stores for.</param>
    /// <returns>An <see cref="IEnumerable{Store}"/> of <see cref="Store"/> containing the user's associated stores, or <see cref="NotFound"/> if the user is not found.</returns>
    [HttpGet("{userId:int}/stores"), Authorize] // api/stores
    public async Task<ActionResult<IEnumerable<Store>>> GetUserStores(int userId)
    {
        var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (id != userId)
        {
            return Forbid("Access denied.");
        }
        
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);
        if (user != null) return Ok(user.Stores);
        return NotFound("User not found");
    }
}