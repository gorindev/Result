namespace Gorin.Result.Results;

public readonly partial record struct Result<TValue> : IResult<TValue>
{
    /// <summary>
    /// Creates an <see cref="Result{TValue}"/> from a value.
    /// </summary>
    public static implicit operator Result<TValue>(TValue value) => new(value);

    /// <summary>
    /// Creates an <see cref="Result{TValue}"/> from an error.
    /// </summary>
    public static implicit operator Result<TValue>(Error error) => new(error);

    /// <summary>
    /// Creates an <see cref="Result{TValue}"/> from a list of errors.
    /// </summary>
    public static implicit operator Result<TValue>(List<Error> errors) => errors?.Count > 0 ? new(errors) : new(KnownErrors.CachedInvalidInitialErrorsList);

    /// <summary>
    /// Creates an <see cref="Result{TValue}"/> from a list of errors.
    /// </summary>
    public static implicit operator Result<TValue>(Error[] errors) => errors?.Length > 0 ? new([.. errors]) : new(KnownErrors.CachedInvalidInitialErrorsList);
}
