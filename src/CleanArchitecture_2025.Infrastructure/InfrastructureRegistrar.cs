using CleanArchitecture_2025.Application.Services;
using CleanArchitecture_2025.Domain.Entities;
using CleanArchitecture_2025.Infrastructure.Context;
using CleanArchitecture_2025.Infrastructure.Options;
using CleanArchitecture_2025.Infrastructure.Services;
using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Scrutor;

namespace CleanArchitecture_2025.Infrastructure;

public static class InfrastructureRegistrar
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            string connectionString = configuration.GetConnectionString("SqlServer")!;
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());

        services.AddMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();

        services
            .AddIdentity<AppUser, IdentityRole<Guid>>(cfr =>
            {
                cfr.Password.RequiredLength = 1;             // Şifrenin en az 1 karakter uzunluğunda olması gerekir.
                cfr.Password.RequireNonAlphanumeric = false;  // Şifrede özel karakter (!@#$%^&*) zorunluluğu kaldırıldı.
                cfr.Password.RequireUppercase = false;        // Şifrede en az bir büyük harf (A-Z) zorunluluğu kaldırıldı.
                cfr.Password.RequireLowercase = false;        // Şifrede en az bir küçük harf (a-z) zorunluluğu kaldırıldı.
                cfr.Password.RequireDigit = false;            // Şifrede en az bir rakam (0-9) zorunluluğu kaldırıldı.

                cfr.SignIn.RequireConfirmedEmail = true;  // Kullanıcının giriş yapabilmesi için e-posta doğrulaması zorunludur.

                cfr.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // 3 başarısız giriş denemesi sonrası hesap 5 dakika kilitlenir.
                cfr.Lockout.MaxFailedAccessAttempts = 3; // Maksimum 3 başarısız giriş denemesine izin verilir.
                cfr.Lockout.AllowedForNewUsers = true;  // Yeni oluşturulan kullanıcılar için de kilitleme politikası geçerli olur.

                cfr.User.RequireUniqueEmail = true; // Aynı e-posta adresiyle birden fazla hesap oluşturulamaz.
                cfr.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                // Kullanıcı adında yalnızca belirtilen karakterler kullanılabilir.
            })
            .AddEntityFrameworkStores<ApplicationDbContext>() // Identity işlemleri için Entity Framework tabanlı veri deposu kullanılır.
            .AddDefaultTokenProviders(); // Şifre sıfırlama, e-posta doğrulama gibi işlemler için varsayılan token sağlayıcıları eklenir.

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.ConfigureOptions<JwtTokenOptionsSetup>();

        // Keycloak İşlemleri
        //services.Configure<KeycloakConfiguration>(configuration.GetSection("KeycloakConfiguration"));
        //services.AddScoped<KeycloakService>();

        services.Scan(action =>
        {
            action
            .FromAssemblies(typeof(InfrastructureRegistrar).Assembly)
            .AddClasses(publicOnly: false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsMatchingInterface()
            .AsImplementedInterfaces()
            .WithScopedLifetime();
        });

        services.AddHealthChecks()
        .AddCheck("health-check", () => HealthCheckResult.Healthy())
        .AddDbContextCheck<ApplicationDbContext>();

        return services;
    }
}
