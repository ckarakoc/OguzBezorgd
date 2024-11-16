using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Server.Core.DTOs;
using Server.Core.Entities;
using Server.Core.Interfaces;

namespace Server.Core.Controllers;

/// <summary>
/// Controller responsible for managing user api calls.
/// </summary>
public class UsersController(IUnitOfWork unitOfWork) : BaseApiController
{
    /// <summary>
    /// Retrieves a list of all users.
    /// </summary>
    /// <returns>An <see cref="IEnumerable"/> of <see cref="UserDto"/> containing all user details.</returns>
    [HttpGet, Authorize]
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
    [HttpGet("{userName}")]
    public async Task<ActionResult<UserDto>> GetUser(string userName)
    {
        //todo
        var user = await unitOfWork.UserRepository.GetUserByUserNameAsync(userName);
        if (user == null) return NotFound("User not found");
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
    [HttpPut("{userId:int}")]
    public async Task<ActionResult> UpdateUser(int userId, UserDto userDto)
    {
        //todo
        return NoContent();
    }

    /// <summary>
    /// Deletes a user by their unique ID.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the delete operation.</returns>
    [HttpDelete("{userId:int}")]
    public async Task<ActionResult> DeleteUser(int userId)
    {
        //todo
        return NoContent();
    }

    /// <summary>
    /// Retrieves the roles associated with a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve roles for.</param>
    /// <returns>An <see cref="IEnumerable{Store}"/> containing the user's roles, or <see cref="NotFound"/> if the user is not found.</returns>
    [HttpGet("{userId:int}/roles")]
    public async Task<ActionResult<IEnumerable<Store>>> GetUserRoles(int userId)
    {
        //todo
        return NotFound("User not found");
    }

    /// <summary>
    /// Retrieves the stores associated with a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve stores for.</param>
    /// <returns>An <see cref="IEnumerable{Store}"/> of <see cref="Store"/> containing the user's associated stores, or <see cref="NotFound"/> if the user is not found.</returns>
    [HttpGet("{userId:int}/stores")] // api/stores
    public async Task<ActionResult<IEnumerable<Store>>> GetUserStores(int userId)
    {
        //todo
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);
        if (user != null) return Ok(user.Stores);
        return NotFound("User not found");
    }
}