using FluentValidation;
using MediatR;
using TS.Result;

namespace CleanArchitecture_2025.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, Result<TResponse>>
        where TRequest : class, IRequest<Result<TResponse>>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<Result<TResponse>> Handle(TRequest request, RequestHandlerDelegate<Result<TResponse>> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var errorDictionary = _validators
            .Select(s => s.Validate(context))
            .SelectMany(s => s.Errors)
            .Where(s => s != null)
            .GroupBy(
            s => s.PropertyName,
            s => s.ErrorMessage, (propertyName, errorMessage) => new
            {
                Key = propertyName,
                Values = errorMessage.Distinct().ToArray()
            })
            .ToDictionary(s => s.Key, s => s.Values[0]);

        if (errorDictionary.Any())
        {
            var errors = errorDictionary
                 .Select(s => $"{s.Key}: {s.Value}")
                 .ToList();
            return Result<TResponse>.Failure(400, errors);
        }

        return await next();
    }
}
