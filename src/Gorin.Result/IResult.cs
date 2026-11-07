using System.Runtime.CompilerServices;

namespace Gorin.Result;

[CollectionBuilder(typeof(CollectionExpression), nameof(CollectionExpression.CreateIResultValue))]
public interface IResult<out TValue> : IResult
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    TValue Value { get; }
}

/// <summary>
/// Type-less interface for the <see cref="Result"/> object.
/// </summary>
/// <remarks>
/// This interface is intended for use when the underlying type of the <see cref="Result"/> object is unknown.
/// </remarks>
[CollectionBuilder(typeof(CollectionExpression), nameof(CollectionExpression.CreateIResult))]
public interface IResult : IRecordable
{
    /// <summary>
    /// Gets the list of errors.
    /// </summary>
    List<Error>? Errors { get; }

    /// <summary>
    /// Gets a value indicating whether the state is error.
    /// </summary>
    bool IsError { get; }

    /// <summary>
    /// Gets a value indicating whether the state is a success.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Gets enumerator with <see cref="Error"/> objects.
    /// </summary>
    /// <returns>Enunerator of <see cref="Error"/> objects.</returns>
    /// <remarks>This method is only for the purpose of collection expression support.</remarks>
    IEnumerator<Error> GetEnumerator();
}
