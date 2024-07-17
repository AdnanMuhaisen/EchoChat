using EchoChat.Domain.UserAggregates;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EchoChat.Infrastructure.DataAccess;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>(entity => entity.ToTable("Users"));
        builder.Entity<IdentityRole<int>>(entity => entity.ToTable("Roles"));
        builder.Entity<IdentityUserRole<int>>(entity => entity.ToTable("UserRoles"));
        builder.Entity<IdentityUserClaim<int>>(entity => entity.ToTable("UserClaims"));
        builder.Entity<IdentityUserLogin<int>>(entity => entity.ToTable("UserLogins"));
        builder.Entity<IdentityRoleClaim<int>>(entity => entity.ToTable("RoleClaims"));
        builder.Entity<IdentityUserToken<int>>(entity => entity.ToTable("UserTokens"));
        //builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}