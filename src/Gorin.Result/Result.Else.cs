namespace Gorin.Result.Results;

public readonly partial record struct Result<TValue> : IResult<TValue>
{
    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public Result<TValue> Else(Func<List<Error>, Error> onError)
    {
        if (IsSuccess)
        {
            return Value;
        }

        return onError(Errors);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public Result<TValue> Else(Func<List<Error>, List<Error>> onError)
    {
        if (IsSuccess)
        {
            return Value;
        }

        return onError(Errors);
    }

    /// <summary>
    /// If the state is error, the provided <paramref name="error"/> is returned.
    /// </summary>
    /// <param name="error">The error to return.</param>
    /// <returns>The given <paramref name="error"/>.</returns>
    public Result<TValue> Else(Error error)
    {
        if (IsSuccess)
        {
            return Value;
        }

        return error;
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public Result<TValue> Else(Func<List<Error>, TValue> onError)
    {
        if (IsSuccess)
        {
            return Value;
        }

        return onError(Errors);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public Result<TValue> Else(Func<List<Error>, Result<TValue>> onError)
    {
        if (IsSuccess)
        {
            return Value;
        }

        return onError(Errors);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The value to return if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public Result<TValue> Else(TValue onError)
    {
        if (IsSuccess)
        {
            return Value;
        }

        return onError;
    }

    /// <summary>
    /// If the state is error, the provided <paramref name="action"/> is invoked.
    /// </summary>
    /// <param name="action">The action to execute if the state is error.</param>
    /// <returns>The original <see cref="Result"/> instance.</returns>
    public Result<TValue> ElseDo(Action<List<Error>> action)
    {
        if (IsSuccess)
        {
            return Value;
        }

        action(Errors);

        return this;
    }

    /// <summary>
    /// If the state is error, the provided <paramref name="action"/> is invoked asynchronously.
    /// </summary>
    /// <param name="action">The action to execute if the state is error.</param>
    /// <returns>The original <see cref="Result"/> instance.</returns>
    public async Task<Result<TValue>> ElseDoAsync(Func<List<Error>, Task> action)
    {
        if (IsSuccess)
        {
            return Value;
        }

        await action(Errors).ConfigureAwait(false);

        return this;
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<Result<TValue>> ElseAsync(Func<List<Error>, Task<TValue>> onError)
    {
        if (IsSuccess)
        {
            return Value;
        }

        return await onError(Errors).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<Result<TValue>> ElseAsync(Func<List<Error>, Task<Result<TValue>>> onError)
    {
        if (IsSuccess)
        {
            return Value;
        }

        return await onError(Errors).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<Result<TValue>> ElseAsync(Func<List<Error>, Task<Error>> onError)
    {
        if (IsSuccess)
        {
            return Value;
        }

        return await onError(Errors).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<Result<TValue>> ElseAsync(Func<List<Error>, Task<List<Error>>> onError)
    {
        if (IsSuccess)
        {
            return Value;
        }

        return await onError(Errors).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided <paramref name="error"/> is awaited and returned.
    /// </summary>
    /// <param name="error">The error to return if the state is error.</param>
    /// <returns>The result from awaiting the given <paramref name="error"/>.</returns>
    public async Task<Result<TValue>> ElseAsync(Task<Error> error)
    {
        if (IsSuccess)
        {
            return Value;
        }

        return await error.ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<Result<TValue>> ElseAsync(Task<TValue> onError)
    {
        if (IsSuccess)
        {
            return Value;
        }

        return await onError.ConfigureAwait(false);
    }
}
