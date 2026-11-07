namespace Gorin.Result.Results;

public static partial class ResultExtensions
{
    /// <summary>
    /// If the state is value, the provided function <paramref name="onValue"/> is invoked asynchronously.
    /// If <paramref name="onValue"/> returns true, the given <paramref name="error"/> will be returned, and the state will be error.
    /// </summary>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <param name="error">The <see cref="Error"/> to return if the given <paramref name="onValue"/> function returned true..</param>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <returns>The given <paramref name="error"/> if <paramref name="onValue"/> returns true; otherwise, the original <see cref="Result"/> instance.</returns>
    public static async Task<Result<TValue>> FailIf<TValue>(
        this Task<Result<TValue>> result,
        Func<TValue, bool> onValue,
        Error error)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return awaitedResult.FailIf(onValue, error);
    }

    /// <summary>
    /// If the state is value, the provided function <paramref name="onValue"/> is invoked asynchronously.
    /// If <paramref name="onValue"/> returns true, the given <paramref name="errorBuilder"/> will be returned, and the state will be error.
    /// </summary>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <param name="errorBuilder">The error builder function to execute and return if the given <paramref name="onValue"/> function returned true.</param>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <returns>The given <paramref name="errorBuilder"/> functions return value if <paramref name="onValue"/> returns true; otherwise, the original <see cref="Result"/> instance.</returns>
    public static async Task<Result<TValue>> FailIf<TValue>(
        this Task<Result<TValue>> result,
        Func<TValue, bool> onValue,
        Func<TValue, Error> errorBuilder)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return awaitedResult.FailIf(onValue, errorBuilder);
    }

    /// <summary>
    /// If the state is value, the provided function <paramref name="onValue"/> is invoked asynchronously.
    /// If <paramref name="onValue"/> returns true, the given <paramref name="error"/> will be returned, and the state will be error.
    /// </summary>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the statement is value.</param>
    /// <param name="error">The <see cref="Error"/> to return if the given <paramref name="onValue"/> function returned true.</param>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <returns>The given <paramref name="error"/> if <paramref name="onValue"/> returns true; otherwise, the original <see cref="Result"/> instance.</returns>
    public static async Task<Result<TValue>> FailIfAsync<TValue>(
        this Task<Result<TValue>> result,
        Func<TValue, Task<bool>> onValue,
        Error error)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return await awaitedResult.FailIfAsync(onValue, error);
    }

    /// <summary>
    /// If the state is value, the provided function <paramref name="onValue"/> is invoked asynchronously.
    /// If <paramref name="onValue"/> returns true, the given <paramref name="errorBuilder"/> will be returned, and the state will be error.
    /// </summary>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the statement is value.</param>
    /// <param name="errorBuilder">The error builder function to execute and return if the given <paramref name="onValue"/> function returned true.</param>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <returns>The given <paramref name="errorBuilder"/> functions return value if <paramref name="onValue"/> returns true; otherwise, the original <see cref="Result"/> instance.</returns>
    public static async Task<Result<TValue>> FailIfAsync<TValue>(
        this Task<Result<TValue>> result,
        Func<TValue, Task<bool>> onValue,
        Func<TValue, Task<Error>> errorBuilder)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return await awaitedResult.FailIfAsync(onValue, errorBuilder);
    }
}
