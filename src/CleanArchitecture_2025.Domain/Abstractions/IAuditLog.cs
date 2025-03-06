namespace CleanArchitecture_2025.Domain.Abstractions;

internal interface IAuditLog
{
    Guid Id { get; set; }
    bool IsActive { get; set; }
    DateTimeOffset CreateAt { get; set; }
    Guid CreateUserId { get; set; }
    string CreateUserName { get; }
    DateTimeOffset? UpdateAt { get; set; }
    Guid? UpdateUserId { get; set; }
    string? UpdateUserName { get; }
    bool IsDeleted { get; set; }
    DateTimeOffset? DeleteAt { get; set; }
}
