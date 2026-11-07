using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public class FailIfAsyncTests
{
    private record Person(string Name);

    [Fact]
    public async Task CallingFailIfAsync_WhenFailsIf_ShouldReturnError()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = await resultInt
            .FailIfAsync(num => Task.FromResult(num > 3), Error.Failure());

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Failure, result.FirstError.Type);
    }

    [Fact]
    public async Task CallingFailIfAsyncExtensionMethod_WhenFailsIf_ShouldReturnError()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = await resultInt
            .ThenAsync(num => Task.FromResult(num))
            .FailIfAsync(num => Task.FromResult(num > 3), Error.Failure());

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Failure, result.FirstError.Type);
    }

    [Fact]
    public async Task CallingFailIfAsync_WhenDoesNotFailIf_ShouldReturnValue()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = await resultInt
            .FailIfAsync(num => Task.FromResult(num > 10), Error.Failure());

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(5, result.Value);
    }

    [Fact]
    public async Task CallingFailIf_WhenIsError_ShouldNotInvokeFailIfFunc()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = await resultString
            .FailIfAsync(str => Task.FromResult(str == string.Empty), Error.Failure());

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.NotFound, result.FirstError.Type);
    }

    [Fact]
    public async Task CallingFailIfAsyncWithErrorBuilder_WhenFailsIf_ShouldReturnError()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = await resultInt
            .FailIfAsync(num => Task.FromResult(num > 3), (num) => Task.FromResult(Error.Failure(description: $"{num} is greater than 3.")));

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Failure, result.FirstError.Type);
        Assert.Equal("5 is greater than 3.", result.FirstError.Description);
    }

    [Fact]
    public async Task CallingFailIfAsyncExtensionMethodWithErrorBuilder_WhenFailsIf_ShouldReturnError()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = await resultInt
            .ThenAsync(num => Task.FromResult(num))
            .FailIfAsync(num => Task.FromResult(num > 3), (num) => Task.FromResult(Error.Failure(description: $"{num} is greater than 3.")));

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Failure, result.FirstError.Type);
        Assert.Equal("5 is greater than 3.", result.FirstError.Description);
    }

    [Fact]
    public async Task CallingFailIfAsyncWithErrorBuilder_WhenDoesNotFailIf_ShouldReturnValue()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = await resultInt
            .FailIfAsync(num => Task.FromResult(num > 10), (num) => Task.FromResult(Error.Failure(description: $"{num} is greater than 10.")));

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(5, result.Value);
    }

    [Fact]
    public async Task CallingFailIfWithErrorBuilder_WhenIsError_ShouldNotInvokeFailIfFunc()
    {
        // Arrange
        Result<int> resultInt = Error.NotFound();

        // Act
        Result<int> result = await resultInt
            .FailIfAsync(num => Task.FromResult(num > 3), (num) => Task.FromResult(Error.Failure(description: $"{num} is greater than 3.")));

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.NotFound, result.FirstError.Type);
    }
}
