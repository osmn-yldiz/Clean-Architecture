using MediatR;
using Microsoft.Extensions.Logging;
using TS.Result;

namespace CleanArchitecture_2025.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest).Name;
        var requestTime = DateTime.UtcNow;

        _logger.LogInformation(
            "[START] HTTP {RequestType} started at {RequestTime}",
            requestType, requestTime);

        try
        {
            var result = await next();

            if (result is Result<object> responseResult && !responseResult.IsSuccessful)
            {
                _logger.LogError(
                    "[FAIL] HTTP {RequestType} failed at {RequestTime}, Errors: {ErrorMessages}, StatusCode: {StatusCode}",
                    requestType, requestTime, responseResult.ErrorMessages, responseResult.StatusCode);
            }
            else
            {
                _logger.LogInformation(
                    "[SUCCESS] HTTP {RequestType} completed at {RequestTime}",
                    requestType, requestTime);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ERROR] HTTP {RequestType} failed at {RequestTime}", requestType, requestTime);
            throw;
        }
    }
}

