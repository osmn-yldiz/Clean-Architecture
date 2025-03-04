using CleanArchitecture_2025.Domain.Entities;
using MediatR;
using TS.Result;

namespace CleanArchitecture_2025.Application.Features.Employees.GetByIdEmployee;

public sealed record GetByIdEmployeeQuery(
    Guid Id) : IRequest<Result<Employee>>;
