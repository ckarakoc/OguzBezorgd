using Server.Core.Interfaces;

namespace Server.Core.Data;

public class UnitOfWork(
    ApplicationDbContext context,
    IUserRepository userRepository,
    IStoreRepository storeRepository) : IUnitOfWork
{
    public IUserRepository UserRepository => userRepository;
    public IStoreRepository StoreRepository => storeRepository;

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return context.ChangeTracker.HasChanges();
    }
}