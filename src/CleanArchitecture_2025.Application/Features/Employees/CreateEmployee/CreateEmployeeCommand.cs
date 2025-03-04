using CleanArchitecture_2025.Domain.ValueObjects;
using MediatR;
using TS.Result;

namespace CleanArchitecture_2025.Application.Features.Employees.CreateEmployee;

public sealed record CreateEmployeeCommand(
    string FirstName,
    string LastName,
    DateOnly BirthOfDate,
    decimal Salary,
    PersonelInformation PersonelInformation,
    Address Address,
    bool IsActive) : IRequest<Result<string>>;
