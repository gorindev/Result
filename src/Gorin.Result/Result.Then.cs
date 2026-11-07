namespace Gorin.Result.Results;

public readonly partial record struct Result<TValue> : IResult<TValue>
{
    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original <see cref="Errors"/>.</returns>
    public Result<TNextValue> Then<TNextValue>(Func<TValue, Result<TNextValue>> onValue)
    {
        if (IsError)
        {
            return Errors;
        }

        return onValue(Value);
    }

    /// <summary>
    /// If the state is a value, the provided <paramref name="action"/> is invoked.
    /// </summary>
    /// <param name="action">The action to execute if the state is a value.</param>
    /// <returns>The original <see cref="Result"/> instance.</returns>
    public Result<TValue> ThenDo(Action<TValue> action)
    {
        if (IsError)
        {
            return Errors;
        }

        action(Value);

        return this;
    }

    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original <see cref="Errors"/>.</returns>
    public Result<TNextValue> Then<TNextValue>(Func<TValue, TNextValue> onValue)
    {
        if (IsError)
        {
            return Errors;
        }

        return onValue(Value);
    }

    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original <see cref="Errors"/>.</returns>
    public async Task<Result<TNextValue>> ThenAsync<TNextValue>(Func<TValue, Task<Result<TNextValue>>> onValue)
    {
        if (IsError)
        {
            return Errors;
        }

        return await onValue(Value).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is a value, the provided <paramref name="action"/> is invoked asynchronously.
    /// </summary>
    /// <param name="action">The action to execute if the state is a value.</param>
    /// <returns>The original <see cref="Result"/> instance.</returns>
    public async Task<Result<TValue>> ThenDoAsync(Func<TValue, Task> action)
    {
        if (IsError)
        {
            return Errors;
        }

        await action(Value).ConfigureAwait(false);

        return this;
    }

    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original <see cref="Errors"/>.</returns>
    public async Task<Result<TNextValue>> ThenAsync<TNextValue>(Func<TValue, Task<TNextValue>> onValue)
    {
        if (IsError)
        {
            return Errors;
        }

        return await onValue(Value).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its errors are returned.
    /// If no errors are returned, the original value is returned.
    /// </summary>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The errors from calling <paramref name="onValue"/> if state is value and errors are returned; otherwise the original <see cref="Result"/> instance.</returns>
    public Result<TValue> ThenEnsure(Func<TValue, Result<TValue>> onValue)
    {
        if (IsError)
        {
            return Errors;
        }

        Result<TValue> result = onValue(Value);

        return result.IsError ? result.Errors : this;
    }

    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its errors are returned.
    /// If no errors are returned, the original value is returned.
    /// </summary>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The errors from calling <paramref name="onValue"/> if state is value and errors are returned; otherwise the original <see cref="Result"/> instance.</returns>
    public async Task<Result<TValue>> ThenEnsureAsync(Func<TValue, Task<Result<TValue>>> onValue)
    {
        if (IsError)
        {
            return Errors;
        }

        Result<TValue> result = await onValue(Value).ConfigureAwait(false);

        return result.IsError ? result.Errors : this;
    }
}
