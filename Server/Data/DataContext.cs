using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Entities;

namespace Server.Data;

public class DataContext(DbContextOptions options)
    : IdentityDbContext<
        User,
        IdentityRole<int>,
        int,
        IdentityUserClaim<int>,
        IdentityUserRole<int>,
        IdentityUserLogin<int>,
        IdentityRoleClaim<int>,
        IdentityUserToken<int>>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<User>()
            .ToTable("Users");
        builder.Entity<IdentityRole<int>>()
            .ToTable("Roles");
        builder.Entity<IdentityUserRole<int>>()
            .ToTable("UserRoles");
        builder.Entity<IdentityRoleClaim<int>>()
            .ToTable("RoleClaims");
        builder.Entity<IdentityUserClaim<int>>()
            .ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<int>>()
            .ToTable("UserLogins");
        builder.Entity<IdentityUserToken<int>>()
            .ToTable("UserTokens");
        builder.Entity<Order>()
            .ToTable("Orders");
        builder.Entity<OrderItem>()
            .ToTable("OrderItems");
        builder.Entity<Store>()
            .ToTable("Stores");
        builder.Entity<StoreAddress>()
            .ToTable("StoreAddresses");
        builder.Entity<StoreProduct>()
            .ToTable("StoreProducts");
        builder.Entity<UserAddress>()
            .ToTable("UserAddresses");
    }
}