using CleanArchitecture_2025.Domain.Abstractions;

namespace CleanArchitecture_2025.Application.Features.Employees.GetAllEmployees;

public sealed class GetAllEmployeesQueryResponse : EntityDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateOnly BirthOfDate { get; set; }
    public decimal Salary { get; set; }
    public string TCNo { get; set; } = default!;
}
