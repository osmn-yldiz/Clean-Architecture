using MediatR;
using TS.Result;

namespace CleanArchitecture_2025.Application.Features.Employees.DeleteEmployeeById;

public sealed record DeleteEmployeeByIdCommand(
    Guid Id) : IRequest<Result<string>>;
