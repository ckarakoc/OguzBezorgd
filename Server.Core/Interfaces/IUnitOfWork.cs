namespace Server.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IStoreRepository StoreRepository { get; }
    Task<bool> Complete();
    bool HasChanges();
}