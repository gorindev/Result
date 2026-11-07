namespace Gorin.Result.Results;

public readonly partial record struct Result<TValue>
{
    /// <inheritdoc/>
    public TOutput GetRecording<TOutput>(IRecordingSerializer<TOutput> serializer)
    {
        return IsError ? serializer.SerializeErrors(Errors) : serializer.SerializeValue(Value);
    }
}
