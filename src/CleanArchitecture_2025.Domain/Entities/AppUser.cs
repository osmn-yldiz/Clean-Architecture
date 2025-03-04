using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture_2025.Domain.Entities;

public sealed class AppUser : IdentityUser<Guid>
{
    public AppUser()
    {
        Id = Guid.CreateVersion7();
    }

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}";
    public string? RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpires { get; set; }
    public bool IsAdmin { get; set; } = false;

    #region Audit Log
    public DateTimeOffset CreateAt { get; set; }
    public Guid CreateUserId { get; set; } = default!;
    public DateTimeOffset? UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeleteAt { get; set; }
    public Guid? DeleteUserId { get; set; }
    #endregion

}
