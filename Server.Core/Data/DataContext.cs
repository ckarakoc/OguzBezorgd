using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities;

namespace Server.Core.Data;

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
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<StoreAddress> StoreAddresses { get; set; }
    public DbSet<StoreProduct> StoreProducts { get; set; }
    public DbSet<UserAddress> UserAddresses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // EF.IdentityCore
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

        // Custom Additions
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