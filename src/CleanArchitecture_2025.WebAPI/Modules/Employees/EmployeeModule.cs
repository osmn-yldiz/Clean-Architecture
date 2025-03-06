using CleanArchitecture_2025.Application.Features.Employees.CreateEmployee;
using CleanArchitecture_2025.Application.Features.Employees.DeleteEmployeeById;
using CleanArchitecture_2025.Application.Features.Employees.GetByIdEmployee;
using CleanArchitecture_2025.Application.Features.Employees.UpdateEmployee;
using CleanArchitecture_2025.Domain.Entities;
using MediatR;
using TS.Result;

namespace CleanArchitecture_2025.WebAPI.Modules.Employees;

public static class EmployeeModule
{
    public static void RegisterEmployeeRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/employees").WithTags("Employees").RequireAuthorization();

        // GetById Employee
        group.MapGet("{id}",
            async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                if (id == Guid.Empty)
                {
                    //throw new ProblemException("Bad Request", "Geçersiz Guid değeri sağlandı.");
                    return Results.BadRequest(new { Message = "Geçersiz Guid değeri sağlandı." });
                }

                var response = await sender.Send(new GetByIdEmployeeQuery(id), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.BadRequest(response);
            })
            .Produces<Result<Employee>>()
            .WithName("GetById");

        // Create Employee
        group.MapPost(string.Empty,
            async (ISender sender, CreateEmployeeCommand request, CancellationToken cancellationToken) =>
            {
                if (request is null)
                {
                    //throw new ProblemException("Bad Request", "İstek boş olamaz.");
                    return Results.BadRequest(new { Message = "İstek boş olamaz." });
                }

                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.BadRequest(response);
            })
            .Produces<Result<string>>()
            .WithName("Create");

        // Update Employee
        group.MapPut(string.Empty,
            async (ISender sender, UpdateEmployeeCommand request, CancellationToken cancellationToken) =>
            {
                if (request is null)
                {
                    //throw new ProblemException("Bad Request", "İstek boş olamaz.");
                    return Results.BadRequest(new { Message = "İstek boş olamaz." });
                }

                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.BadRequest(response);
            })
            .Produces<Result<string>>()
            .WithName("Update");

        // Delete Employee
        group.MapDelete("{id}",
            async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                if (id == Guid.Empty)
                {
                    //throw new ProblemException("Bad Request", "Geçersiz Guid değeri sağlandı.");
                    return Results.BadRequest(new { Message = "Geçersiz Guid değeri sağlandı." });
                }

                var response = await sender.Send(new DeleteEmployeeByIdCommand(id), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.BadRequest(response);
            })
            .Produces<Result<string>>()
            .WithName("Delete");

    }
}
