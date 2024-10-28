using Microsoft.EntityFrameworkCore;
using Server.Entities;
using Server.Interfaces;

namespace Server.Data.Repositories;

public class UserRepository(DataContext context) : IUserRepository
{
    public void AddUser(User user)
    {
        context.Users.Add(user);
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var users = await context.Users
            .Include(u => u.Stores)
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
        var user = await context.Users.SingleOrDefaultAsync(u => u.NormalizedUserName == userName.ToUpper());
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

    public async void Save()
    {
        await context.SaveChangesAsync();
    }
}