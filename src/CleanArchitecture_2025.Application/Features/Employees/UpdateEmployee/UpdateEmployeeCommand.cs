using CleanArchitecture_2025.Domain.ValueObjects;
using MediatR;
using TS.Result;

namespace CleanArchitecture_2025.Application.Features.Employees.UpdateEmployee;

public sealed record UpdateEmployeeCommand(
    Guid Id,
    string FirstName,
    string LastName,
    DateOnly BirthOfDate,
    decimal Salary,
    PersonelInformation PersonelInformation,
    Address? Address) : IRequest<Result<string>>;
