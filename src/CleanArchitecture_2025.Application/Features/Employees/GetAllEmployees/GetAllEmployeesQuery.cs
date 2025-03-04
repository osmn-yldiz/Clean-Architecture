using MediatR;

namespace CleanArchitecture_2025.Application.Features.Employees.GetAllEmployees;

public sealed record GetAllEmployeesQuery() : IRequest<IQueryable<GetAllEmployeesQueryResponse>>;
