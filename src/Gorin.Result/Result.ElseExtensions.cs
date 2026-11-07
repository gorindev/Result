namespace Gorin.Result.Results;

public static partial class ResultExtensions
{
    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> Else<TValue>(this Task<Result<TValue>> result, Func<List<Error>, TValue> onError)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return awaitedResult.Else(onError);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> Else<TValue>(this Task<Result<TValue>> result, Func<List<Error>, Result<TValue>> onError)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return awaitedResult.Else(onError);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> Else<TValue>(this Task<Result<TValue>> result, TValue onError)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return awaitedResult.Else(onError);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Func<List<Error>, Task<TValue>> onError)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return await awaitedResult.ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Func<List<Error>, Task<Result<TValue>>> onError)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return await awaitedResult.ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Task<TValue> onError)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return await awaitedResult.ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> Else<TValue>(this Task<Result<TValue>> result, Func<List<Error>, Error> onError)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return awaitedResult.Else(onError);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> Else<TValue>(this Task<Result<TValue>> result, Func<List<Error>, List<Error>> onError)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return awaitedResult.Else(onError);
    }

    /// <summary>
    /// If the state is error, the provided <paramref name="error"/> is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="error">The error to return.</param>
    /// <returns>The given <paramref name="error"/>.</returns>
    public static async Task<Result<TValue>> Else<TValue>(this Task<Result<TValue>> result, Error error)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return awaitedResult.Else(error);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Func<List<Error>, Task<Error>> onError)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return await awaitedResult.ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Func<List<Error>, Task<List<Error>>> onError)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return await awaitedResult.ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> result, Task<Error> onError)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return await awaitedResult.ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state of <paramref name="result"/> is error, the provided <paramref name="action"/> is invoked.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="action">The action to execute if the state is error.</param>
    /// <returns>The original <paramref name="result"/>.</returns>
    public static async Task<Result<TValue>> ElseDo<TValue>(this Task<Result<TValue>> result, Action<List<Error>> action)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return awaitedResult.ElseDo(action);
    }

    /// <summary>
    /// If the state of <paramref name="result"/> is error, the provided <paramref name="action"/> is executed asynchronously.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="action">The action to execute if the state is error.</param>
    /// <returns>The original <paramref name="result"/>.</returns>
    public static async Task<Result<TValue>> ElseDoAsync<TValue>(this Task<Result<TValue>> result, Func<List<Error>, Task> action)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return await awaitedResult.ElseDoAsync(action).ConfigureAwait(false);
    }
}
