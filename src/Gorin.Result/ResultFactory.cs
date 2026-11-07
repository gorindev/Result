using Gorin.Result.Results;

namespace Gorin.Result;

/// <summary>
/// Provides factory methods for creating instances of <see cref="Result{TValue}"/>.
/// </summary>
public static class ResultFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="Result{TValue}"/> from a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to wrap.</param>
    /// <returns>An instance of <see cref="Result{TValue}"/> containing the provided value.</returns>
    public static Result<TValue> From<TValue>(TValue value)
    {
        return value;
    }

    /// <summary>
    /// Creates a new awaitable instance of <see cref="Result{TValue}"/> from value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to wrap.</param>
    /// <returns>An awaitable instance of <see cref="Result{TValue}"/> containing provided value.</returns>
    public static Task<Result<TValue>> FromAsync<TValue>(TValue value)
    {
        return Task.FromResult(From(value));
    }

    /// <summary>
    /// Creates a new instance of <see cref="Result{TValue}"/> from single error.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="error">Single error instance to wrap.</param>
    /// <returns>An instance of <see cref="Result{TValue}"/> containing the provided error.</returns>
    public static Result<TValue> From<TValue>(Error error)
    {
        return error;
    }

    /// <summary>
    /// Creates a new awaitable instance of <see cref="Result{TValue}"/> from single error.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="error">Single error instance to wrap.</param>
    /// <returns>An awaitable instance of <see cref="Result{TValue}"/> containing the provided error.</returns>
    public static Task<Result<TValue>> FromAsync<TValue>(Error error)
    {
        return Task.FromResult(From<TValue>(error));
    }

    /// <summary>
    /// Creates an <see cref="Result{TValue}"/> from a list of errors.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="errors">List of errors.</param>
    /// <returns>An instance of <see cref="Result{TValue}"/> containing provided list of errors.</returns>
    public static Result<TValue> From<TValue>(List<Error> errors)
    {
        return errors;
    }

    /// <summary>
    /// Creates an awaitable instance of<see cref="Result{TValue}"/> from a list of errors.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="errors">List of errors.</param>
    /// <returns>An awaitable instance of <see cref="Result{TValue}"/> containing provided list of errors.</returns>
    public static Task<Result<TValue>> FromAsync<TValue>(List<Error> errors)
    {
        return Task.FromResult(From<TValue>(errors));
    }

    /// <summary>
    /// Creates an <see cref="Result{TValue}"/> from an enumeration of errors.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="errors">Enumeration of errors.</param>
    /// <returns>An instance of <see cref="Result{TValue}"/> containing provided enumeration of errors.</returns>
    public static Result<TValue> From<TValue>(IEnumerable<Error> errors)
    {
        return errors.ToList();
    }

    /// <summary>
    /// Creates an awaitable instance of<see cref="Result{TValue}"/> from an enumeration of errors.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="errors">Enumeration of errors.</param>
    /// <returns>An awaitable instance of <see cref="Result{TValue}"/> containing provided enumeration of errors.</returns>
    public static Task<Result<TValue>> FromAsync<TValue>(IEnumerable<Error> errors)
    {
        return Task.FromResult(From<TValue>(errors));
    }
}
