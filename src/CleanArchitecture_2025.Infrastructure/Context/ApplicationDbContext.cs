﻿using CleanArchitecture_2025.Domain.Abstractions;
using CleanArchitecture_2025.Domain.Entities;
using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture_2025.Infrastructure.Context;

internal sealed class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InfrastructureRegistrar).Assembly);

        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();
        modelBuilder.Ignore<IdentityUserRole<Guid>>();
        modelBuilder.Ignore<IdentityUserClaim<Guid>>();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();

        HttpContextAccessor httpContextAccessor = new();
        //string? userIdString = httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(p => p.Type == "Id")?.Value;
        Guid userId = Guid.Parse("6aa4945c-1b67-4906-bc18-fa7a2e17e275");

        //string userIdString =
        //    httpContextAccessor
        //    .HttpContext!
        //    .User
        //    .Claims
        //    .First(p => p.Type == ClaimTypes.NameIdentifier).Value;
        //Guid userId = Guid.Parse(userIdString);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreateAt)
                    .CurrentValue = DateTimeOffset.Now;
                entry.Property(p => p.CreateUserId)
                    .CurrentValue = userId;
            }

            if (entry.State == EntityState.Modified)
            {
                if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entry.Property(p => p.DeleteAt)
                                        .CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.DeleteUserId)
                    .CurrentValue = userId;
                }
                else
                {
                    entry.Property(p => p.UpdateAt)
                                        .CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.UpdateUserId)
                    .CurrentValue = userId;
                }
            }

            if (entry.State == EntityState.Deleted)
            {
                throw new ArgumentException("Db'den direkt silme işlemi yapamazsınız.");
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
