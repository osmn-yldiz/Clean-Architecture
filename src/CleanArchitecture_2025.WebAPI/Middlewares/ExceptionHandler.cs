using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using TS.Result;

namespace CleanArchitecture_2025.WebAPI.Middlewares;

public sealed class ExceptionHandler : IExceptionHandler
{
    //private readonly ILogger<ExceptionHandler> _logger;
    //private readonly IProblemDetailsService _problemDetailsService;

    //public ExceptionHandler(ILogger<ExceptionHandler> logger, IProblemDetailsService problemDetailsService)
    //{
    //    _logger = logger;
    //    _problemDetailsService = problemDetailsService;
    //}

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        //if (exception is ValidationException validationException)
        //{
        //    _logger.LogError(
        //        exception,
        //        "Exception occurred: {Message} {@Errors} {@Exception}",
        //        exception.Message,
        //        validationException.Errors,
        //        validationException);
        //}
        //else
        //{
        //    _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
        //}

        Result<string> errorResult;

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        if (exception.GetType() == typeof(ValidationException))
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

            errorResult = Result<string>.Failure(StatusCodes.Status403Forbidden, ((ValidationException)exception).Errors.Select(s => s.PropertyName).ToList());

            await httpContext.Response.WriteAsJsonAsync(errorResult, cancellationToken);

            return true;
        }

        errorResult = Result<string>.Failure(exception.Message);

        await httpContext.Response.WriteAsJsonAsync(errorResult, cancellationToken);

        //if (exception is not ProblemException problemException)
        //{
        //    await httpContext.Response.WriteAsJsonAsync(exception.Message, cancellationToken);
        //    return true;
        //}

        //var problemdetails = new ProblemDetails
        //{
        //    Title = problemException.Error,
        //    Detail = problemException.Message,
        //    Status = StatusCodes.Status400BadRequest,
        //    Type = "Bad Request",
        //};

        //await _problemDetailsService.TryWriteAsync(
        //    new ProblemDetailsContext
        //    {
        //        HttpContext = httpContext,
        //        ProblemDetails = problemdetails,
        //    });

        return true;
    }
}
