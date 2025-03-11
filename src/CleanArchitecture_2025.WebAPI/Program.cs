using CleanArchitecture_2025.Application;
using CleanArchitecture_2025.Application.Behaviors;
using CleanArchitecture_2025.Infrastructure;
using CleanArchitecture_2025.WebAPI.Controllers;
using CleanArchitecture_2025.WebAPI.Middlewares;
using CleanArchitecture_2025.WebAPI.Modules;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

builder.AddServiceDefaults();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers().AddOData(opt =>
        opt
        .Select()
        .Filter()
        .Count()
        .Expand()
        .OrderBy()
        .SetMaxTop(null)
        .AddRouteComponents("odata", AppODataController.GetEdmModel())
);

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", options =>
    {
        options.QueueLimit = 100;
        options.PermitLimit = 100;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // Sýrasý
        options.Window = TimeSpan.FromSeconds(1);
    });
});

builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();  // Minimal API'ler için OpenAPI desteði

//builder.Services.AddOpenApi();
builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });

// JWT Kimlik Doðrulama Ayarlarý
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:7217")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .SetIsOriginAllowed(t => true)
        .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });
});

// Serilog
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

// Keycloak Ýþlemleri
//builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
//builder.Services.AddAuthorization().AddKeycloakAuthorization(builder.Configuration);

var app = builder.Build();

// OpenAPI ve Scalar API Desteði
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // OpenAPI endpointlerini oluþtur
    //app.MapScalarApiReference(); // Scalar API dokümantasyonu ekle
    app.MapScalarApiReference(options =>
    {
        //options.Title = "My Scalar UI";
        //options.Theme = ScalarTheme.BluePlanet;
        //options.Favicon = "path";
        //options.DefaultHttpClient = new KeyValuePair<ScalarTarget, ScalarClient>(ScalarTarget.CSharp, ScalarClient.HttpClient);
        //options.HideModels = false;
        //options.Layout = ScalarLayout.Modern;
        //options.ShowSidebar = true;
        //options.CustomCss = "";

        options
        .WithTitle("My Scalar UI")
        .WithTheme(ScalarTheme.BluePlanet)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
        .WithLayout(ScalarLayout.Modern)
        .WithModels(true)
        .WithCustomCss("")
        .WithSidebar(true);
    });
}

app.MapDefaultEndpoints();

app.UseHttpsRedirection(); // Gelen HTTP isteklerini otomatik olarak HTTPS'ye yönlendirir.

app.UseCors("CorsPolicy");
//app.UseCors(x => x
//.AllowAnyHeader()
//.AllowCredentials()
//.AllowAnyMethod()
//.SetIsOriginAllowed(t => true));

app.UseRateLimiter();

app.RegisterRoutes();

app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCompression(); // Uygulamanýn yanýtlarýný(response) sýkýþtýrarak daha küçük boyutlarda iletilmesini saðlar.

app.UseExceptionHandler();

app.MapControllers().RequireRateLimiting("fixed").RequireAuthorization();

ExtensionsMiddleware.CreateFirstUser(app);

app.UseSerilogRequestLogging(); // HTTP request loglarý için

app.MapHealthChecks("/health-check", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
    }
});

app.Run();