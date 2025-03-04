using CleanArchitecture_2025.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture_2025.WebAPI.Middlewares;

public static class ExtensionsMiddleware
{
    public static void CreateFirstUser(WebApplication app)
    {
        using (var scoped = app.Services.CreateScope())
        {
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            if (!userManager.Users.Any(p => p.UserName == "admin"))
            {
                AppUser user = new()
                {
                    Id = Guid.Parse("6aa4945c-1b67-4906-bc18-fa7a2e17e275"),
                    UserName = "admin",
                    Email = "admin@admin.com",
                    FirstName = "Osman",
                    LastName = "Yıldız",
                    EmailConfirmed = true,
                    CreateAt = DateTimeOffset.Now
                };

                user.CreateUserId = user.Id;

                userManager.CreateAsync(user, "1").Wait();
            }
        }
    }
}
