using CleanArchitecture_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture_2025.Infrastructure.Configurations;

internal sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasIndex(i => i.UserName).IsUnique();
        builder.HasIndex(i => i.Email).IsUnique();

        builder.Property(p => p.FirstName).HasColumnType("varchar(50)").IsRequired();
        builder.Property(p => p.LastName).HasColumnType("varchar(50)").IsRequired();
        builder.Property(p => p.UserName).HasColumnType("varchar(50)").IsRequired();
        builder.Property(p => p.Email).HasColumnType("varchar(350)").IsRequired();

        builder.Property(p => p.EmailConfirmed).HasDefaultValue(false);
        builder.Property(p => p.LockoutEnabled).HasDefaultValue(true);
        builder.Property(p => p.AccessFailedCount).HasDefaultValue(0);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
