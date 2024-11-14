using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Core.DTOs;
using Server.Core.Entities;
using Server.Core.Interfaces;

namespace Server.Core.Controllers;

public class UsersController(
    IUnitOfWork unitOfWork) : BaseApiController
{
    /// <summary>
    /// Retrieves a list of all users.
    /// </summary>
    /// <returns>A list of UserDto objects.</returns>
    [HttpGet] 
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        //todo
        var users = await unitOfWork.UserRepository.GetUsersAsync();
        return Ok(users);
    }

    [HttpGet("{userName}")] 
    public async Task<ActionResult<UserDto>> GetUser(string userName)
    {
        //todo
        var user = await unitOfWork.UserRepository.GetUserByUserNameAsync(userName);
        if (user == null) return NotFound("User not found");
        return Ok(user);
    }

    [HttpGet("{userId:int}")] 
    public async Task<ActionResult<UserDto>> GetUser(int userId)
    {
        //todo
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);
        if (user == null) return NotFound("User not found");
        return Ok(user);
    }

    [HttpPut("{userId:int}")]
    public async Task<ActionResult> UpdateUser(int userId, UserDto userDto)
    {
        //todo
        return NoContent();
    }
    
    [HttpDelete("{userId:int}")]
    public async Task<ActionResult> DeleteUser(int userId)
    {
        //todo
        return NoContent(); 
    }

    [HttpGet("{userId:int}/roles")]
    public async Task<ActionResult<IEnumerable<Store>>> GetUserRoles(int userId)
    {
        //todo
        return NotFound("User not found");
    }
    
    [HttpGet("{userId:int}/stores")]
    public async Task<ActionResult<IEnumerable<Store>>> GetUserStores(int userId)
    {
        //todo
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);
        if (user != null) return Ok(user.Stores);
        return NotFound("User not found");
    }
}