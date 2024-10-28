using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using Server.Entities;
using Server.Interfaces;

namespace Server.Controllers;

public class UsersController(
    IUnitOfWork unitOfWork) : BaseApiController
{
    [HttpGet] // api/users
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await unitOfWork.UserRepository.GetUsersAsync();
        return Ok(users);
    }

    [HttpGet("{userName}")] // api/users/username
    public async Task<ActionResult<UserDto>> GetUser(string userName)
    {
        var user = await unitOfWork.UserRepository.GetUserByUserNameAsync(userName);
        if (user == null) return NotFound("User not found");
        return Ok(user);
    }

    [HttpGet("{id:int}")] // api/users/id
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(id);
        if (user == null) return NotFound("User not found");
        return Ok(user);
    }

    [HttpGet("{userName}/stores")]
    public async Task<ActionResult<IEnumerable<Store>>> GetUserStoresAsync(string userName)
    {
        var user = await unitOfWork.UserRepository.GetUserByUserNameAsync(userName);
        if (user != null) return Ok(user.Stores);
        return NotFound("User not found");
    }
}