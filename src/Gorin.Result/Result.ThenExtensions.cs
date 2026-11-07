namespace Gorin.Result.Results;

public static partial class ResultExtensions
{
    /// <summary>
    /// If the state of <paramref name="result"/> is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onValue"/> function.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original errors.</returns>
    public static async Task<Result<TNextValue>> Then<TValue, TNextValue>(this Task<Result<TValue>> result, Func<TValue, Result<TNextValue>> onValue)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return awaitedResult.Then(onValue);
    }

    /// <summary>
    /// If the state of <paramref name="result"/> is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onValue"/> function.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original errors.</returns>
    public static async Task<Result<TNextValue>> Then<TValue, TNextValue>(this Task<Result<TValue>> result, Func<TValue, TNextValue> onValue)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return awaitedResult.Then(onValue);
    }

    /// <summary>
    /// If the state of <paramref name="result"/> is a value, the provided <paramref name="action"/> is invoked.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="action">The action to execute if the state is a value.</param>
    /// <returns>The original <paramref name="result"/>.</returns>
    public static async Task<Result<TValue>> ThenDo<TValue>(this Task<Result<TValue>> result, Action<TValue> action)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return awaitedResult.ThenDo(action);
    }

    /// <summary>
    /// If the state of <paramref name="result"/> is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onValue"/> function.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original errors.</returns>
    public static async Task<Result<TNextValue>> ThenAsync<TValue, TNextValue>(this Task<Result<TValue>> result, Func<TValue, Task<Result<TNextValue>>> onValue)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return await awaitedResult.ThenAsync(onValue).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state of <paramref name="result"/> is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onValue"/> function.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original errors.</returns>
    public static async Task<Result<TNextValue>> ThenAsync<TValue, TNextValue>(this Task<Result<TValue>> result, Func<TValue, Task<TNextValue>> onValue)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return await awaitedResult.ThenAsync(onValue).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state of <paramref name="result"/> is a value, the provided <paramref name="action"/> is executed asynchronously.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="action">The action to execute if the state is a value.</param>
    /// <returns>The original <paramref name="result"/>.</returns>
    public static async Task<Result<TValue>> ThenDoAsync<TValue>(this Task<Result<TValue>> result, Func<TValue, Task> action)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return await awaitedResult.ThenDoAsync(action).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state of <paramref name="result"/> is a value, the provided function <paramref name="onValue"/> is executed and its errors are returned.
    /// If no errors are returned, the original value is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The errors from calling <paramref name="onValue"/> if state is value and errors are returned; otherwise the original <paramref name="result"/>.</returns>
    public static async Task<Result<TValue>> ThenEnsure<TValue>(this Task<Result<TValue>> result, Func<TValue, Result<TValue>> onValue)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return awaitedResult.ThenEnsure(onValue);
    }

    /// <summary>
    /// If the state of <paramref name="result"/> is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its errors are returned.
    /// If no errors are returned, the original value is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="result"/>.</typeparam>
    /// <param name="result">The <see cref="Result"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The errors from calling <paramref name="onValue"/> if state is value and errors are returned; otherwise the original <paramref name="result"/>.</returns>
    public static async Task<Result<TValue>> ThenEnsureAsync<TValue>(this Task<Result<TValue>> result, Func<TValue, Task<Result<TValue>>> onValue)
    {
        var awaitedResult = await result.ConfigureAwait(false);

        return await awaitedResult.ThenEnsureAsync(onValue).ConfigureAwait(false);
    }
}
