using Gorin.Result.Results;

namespace Gorin.Result;

/// <summary>
/// Contains methods supporting collection expressions.
/// </summary>
public static class CollectionExpression
{
    /// <summary>
    /// Creates <see cref="Result{TValue}"/> from read-only span of errors.
    /// </summary>
    /// <typeparam name="TValue">Type of value.</typeparam>
    /// <param name="errors">Read-only span of errors.</param>
    /// <returns>Error or vale.</returns>
    /// <remarks>Enables support for collection expressions.</remarks>
    public static Result<TValue> CreateResult<TValue>(ReadOnlySpan<Error> errors)
    {
        return errors.ToArray();
    }

    /// <summary>
    /// Creates <see cref="IResult{TValue}"/> from read-only span of errors.
    /// </summary>
    /// <typeparam name="TValue">Type of value.</typeparam>
    /// <param name="errors">Read-only span of errors.</param>
    /// <returns>Error or vale.</returns>
    /// <remarks>Enables support for collection expressions.</remarks>
    public static IResult<TValue> CreateIResultValue<TValue>(ReadOnlySpan<Error> errors)
    {
        return CreateResult<TValue>(errors);
    }

    /// <summary>
    /// Creates <see cref="IResult"/> from read-only span of errors.
    /// </summary>
    /// <param name="errors">Read-only span of errors.</param>
    /// <returns>Error or vale.</returns>
    /// <remarks>Enables support for collection expressions.</remarks>
    public static IResult CreateIResult(ReadOnlySpan<Error> errors)
    {
        return CreateResult<object>(errors);
    }
}
