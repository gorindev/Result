using System.Text.Json;
using System.Text.Json.Serialization;

using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public class ResultRecordableTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
    };

    [Fact]
    public void GetRecording_WithSerializer_WhenIsValue_ShouldReturnJson()
    {
        // Arrange
        var person = new PersonRecord(
            "Alice",
            null,
            30,
            PersonStatus.Active,
            new AddressRecord("123 Main St", "Springfield", null),
            ["Developer", "Admin"]);

        IResult result = ResultFactory.From(person);
        var serializer = new SystemTextJsonRecordingSerializer();

        // Act
        var recording = result.GetRecording(serializer);

        // Assert
        Assert.Equal(JsonSerializer.Serialize(person, JsonOptions), recording);
    }

    [Fact]
    public void GetRecording_WithSerializer_WhenIsError_ShouldReturnJsonErrors()
    {
        // Arrange
        Error error = Error.Unexpected("Test.Error", "Oops.");
        IRecordable result = ResultFactory.From<PersonRecord>(error);
        var serializer = new SystemTextJsonRecordingSerializer();

        // Act
        var recording = result.GetRecording(serializer);

        // Assert
        Assert.Equal(JsonSerializer.Serialize(new[] { error }, JsonOptions), recording);
    }

    [Fact]
    public void GetRecording_WithSerializer_ViaIResult_WhenIsValue_ShouldReturnJson()
    {
        // Arrange
        var person = new PersonRecord(
            "Bob",
            null,
            25,
            PersonStatus.Inactive,
            null,
            null);

        IResult result = ResultFactory.From(person);
        var serializer = new SystemTextJsonRecordingSerializer();

        // Act
        var recording = result.GetRecording(serializer);

        // Assert
        Assert.Equal(JsonSerializer.Serialize(person, JsonOptions), recording);
    }

    [Fact]
    public void GetRecording_WithSerializer_ViaIResult_WhenIsError_ShouldReturnJsonErrors()
    {
        // Arrange
        Error error = Error.Validation("Val.Error", "Invalid.");
        IRecordable result = ResultFactory.From<PersonRecord>(error);
        var serializer = new SystemTextJsonRecordingSerializer();

        // Act
        var recording = result.GetRecording(serializer);

        // Assert
        Assert.Equal(JsonSerializer.Serialize(new[] { error }, JsonOptions), recording);
    }

    [Fact]
    public void GetRecording_WithSerializer_NullSerializer_ShouldThrow_NullReferenceException()
    {
        // Arrange
        Result<PersonRecord> result = ResultFactory.From(new PersonRecord(
            "Alice",
            null,
            30,
            PersonStatus.Suspended,
            null,
            null));

        // Act
        var act = () => result.GetRecording<string>(null!);

        // Assert
        Assert.Throws<NullReferenceException>(act);
    }

    [Fact]
    public void GetRecording_WithSerializer_That_ShouldThrow_NotImplementedException()
    {
        // Arrange
        Result<PersonRecord> result = ResultFactory.From(new PersonRecord(
            "Alice",
            null,
            30,
            PersonStatus.Suspended,
            null,
            null));

        var serializer = new ErrorThrowingRecordingSerializer();

        // Act
        var act = () => result.GetRecording<string>(serializer);

        // Assert
        Assert.Throws<NotImplementedException>(act);
    }

    private enum PersonStatus
    {
        Active,
        Inactive,
        Suspended,
    }

    private record AddressRecord(string Street, string City, string? PostalCode);

    private record PersonRecord(
        string Name,
        string? MiddleName,
        int Age,
        PersonStatus Status,
        AddressRecord? Address,
        List<string>? Tags);

    private sealed class SystemTextJsonRecordingSerializer : IRecordingSerializer<string>
    {
        public string SerializeValue<TValue>(TValue value) => JsonSerializer.Serialize(value, JsonOptions);

        public string SerializeErrors(List<Error> errors) => JsonSerializer.Serialize(errors, JsonOptions);
    }

    private sealed class ErrorThrowingRecordingSerializer : IRecordingSerializer<string>
    {
        public string SerializeValue<TValue>(TValue value) => throw new NotImplementedException();

        public string SerializeErrors(List<Error> errors) => throw new NotImplementedException();
    }
}
