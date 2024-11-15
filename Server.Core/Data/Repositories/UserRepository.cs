using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Server.Core.DTOs;
using Server.Core.Entities;
using Server.Core.Interfaces;

namespace Server.Core.Data.Repositories;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public void AddUser(User user)
    {
        context.Users.Add(user);
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        var users = await context.Users
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .ToListAsync();
        return users;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        return user;
    }

    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        var user = await context.Users.Include(u => u.Stores).SingleOrDefaultAsync(u => u.NormalizedUserName == userName.ToUpper());
        return user;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var user = await context.Users.SingleOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public void UpdateUser(User user)
    {
        context.Entry(user).State = EntityState.Modified;
    }

    public void DeleteUser(User user)
    {
        context.Users.Remove(user);
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}