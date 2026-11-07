using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public class FailIfTests
{
    [Fact]
    public void CallingFailIf_WhenFailsIf_ShouldReturnError()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = resultInt
            .FailIf(num => num > 3, Error.Failure());

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Failure, result.FirstError.Type);
    }

    [Fact]
    public async Task CallingFailIfExtensionMethod_WhenFailsIf_ShouldReturnError()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = await resultInt
            .ThenAsync(num => Task.FromResult(num))
            .FailIf(num => num > 3, Error.Failure());

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Failure, result.FirstError.Type);
    }

    [Fact]
    public void CallingFailIf_WhenDoesNotFailIf_ShouldReturnValue()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = resultInt
            .FailIf(num => num > 10, Error.Failure());

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(5, result.Value);
    }

    [Fact]
    public void CallingFailIf_WhenIsError_ShouldNotInvokeFailIfFunc()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = resultString
            .FailIf(str => str == string.Empty, Error.Failure());

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.NotFound, result.FirstError.Type);
    }

    [Fact]
    public void CallingFailIfWithErrorBuilder_WhenFailsIf_ShouldReturnError()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = resultInt
            .FailIf(num => num > 3, (num) => Error.Failure(description: $"{num} is greater than 3"));

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Failure, result.FirstError.Type);
        Assert.Equal("5 is greater than 3", result.FirstError.Description);
    }

    [Fact]
    public async Task CallingFailIfExtensionMethodWithErrorBuilder_WhenFailsIf_ShouldReturnError()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = await resultInt
            .ThenAsync(num => Task.FromResult(num))
            .FailIf(num => num > 3, (num) => Error.Failure(description: $"{num} is greater than 3"));

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Failure, result.FirstError.Type);
        Assert.Equal("5 is greater than 3", result.FirstError.Description);
    }

    [Fact]
    public void CallingFailIfWithErrorBuilder_WhenDoesNotFailIf_ShouldReturnValue()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = resultInt
            .FailIf(num => num > 10, (num) => Error.Failure(description: $"{num} is greater than 10"));

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(5, result.Value);
    }

    [Fact]
    public void CallingFailIfWithErrorBuilder_WhenIsError_ShouldNotInvokeFailIfFunc()
    {
        // Arrange
        Result<int> resultInt = Error.NotFound();

        // Act
        Result<int> result = resultInt
            .FailIf(num => num > 3, (num) => Error.Failure(description: $"{num} is greater than 3"));

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.NotFound, result.FirstError.Type);
    }
}
