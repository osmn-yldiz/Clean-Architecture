﻿using CleanArchitecture_2025.WebAPI.Modules.Auth;
using CleanArchitecture_2025.WebAPI.Modules.Employees;

namespace CleanArchitecture_2025.WebAPI.Modules;

public static class RouteRegistrar
{
    public static void RegisterRoutes(this IEndpointRouteBuilder app)
    {
        app.RegisterEmployeeRoutes();
        app.RegisterAuthRoutes();
    }
}
