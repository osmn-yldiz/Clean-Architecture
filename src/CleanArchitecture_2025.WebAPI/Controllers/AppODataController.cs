using CleanArchitecture_2025.Application.Features.Employees.GetAllEmployees;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace CleanArchitecture_2025.WebAPI.Controllers;

[Route("odata")]
[ApiController]
[EnableQuery]
//[Authorize("Bearer")]
public class AppODataController(
    ISender sender) : ODataController
{

    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnableLowerCamelCase();
        builder.EntitySet<GetAllEmployeesQueryResponse>("employees");
        return builder.GetEdmModel();
    }

    //[HttpGet("employees")]
    //public async Task<IActionResult> GetAllEmployees(CancellationToken cancellationToken)
    //{
    //    GetAllEmployeesQuery request = new();
    //    var response = await sender.Send(request, cancellationToken);
    //    return Ok(response);
    //}

    [HttpGet("employees")]
    public async Task<IQueryable<GetAllEmployeesQueryResponse>> GetAllEmployees(CancellationToken cancellationToken)
    {
        GetAllEmployeesQuery request = new();
        var response = await sender.Send(request, cancellationToken);
        return response;
    }

}
