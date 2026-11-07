using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public class ElseDoAsyncTests
{
    [Fact]
    public async Task CallingElseDoAsync_WhenIsError_ShouldInvokeGivenAction()
    {
        // Arrange
        Result<string> resultString = Error.Validation();

        // Act
        int errorCounter = 0;
        Result<string> result = await resultString
            .ElseDoAsync(error =>
            {
                errorCounter += error.Count;
                return Task.CompletedTask;
            });

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.FirstError.Type);
        Assert.Equal(1, errorCounter);
    }

    [Fact]
    public async Task CallingElseDoAsync_WhenIsSuccess_ShouldNotInvokeGivenAction()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        int errorCounter = 0;
        Result<string> result = await resultString
            .ElseDoAsync(error =>
            {
                errorCounter += error.Count;
                return Task.CompletedTask;
            });

        // Assert
        Assert.False(result.IsError);
        Assert.Equal("5", result.Value);
        Assert.Equal(0, errorCounter);
    }

    [Fact]
    public async Task CallingElseDoAsync_WhenIsError_ShouldInvokeNextElse()
    {
        // Arrange
        Result<string> resultString = Error.Validation();

        // Act
        int errorCounter = 0;
        Result<string> result = await resultString
            .ElseDoAsync(error =>
            {
                errorCounter += error.Count;
                return Task.CompletedTask;
            })
            .Else(error => $"error count is {errorCounter}");

        // Assert
        Assert.False(result.IsError);
        Assert.Equal("error count is 1", result.Value);
        Assert.Equal(1, errorCounter);
    }

    [Fact]
    public async Task CallingElseDoAsync_WhenIsSuccess_ShouldInvokeNextElse()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        int errorCounter = 0;
        Result<string> result = await resultString
            .ElseDoAsync(error =>
            {
                errorCounter += error.Count;
                return Task.CompletedTask;
            })
            .Else(error => $"is not called");

        // Assert
        Assert.False(result.IsError);
        Assert.Equal("5", result.Value);
        Assert.Equal(0, errorCounter);
    }

    [Fact]
    public async Task CallingElseDoAsync_AfterThenAsync_WhenIsError_ShouldInvokeGivenAction()
    {
        // Arrange
        Result<string> resultString = Error.Validation();

        // Act
        int errorCounter = 0;
        Result<int> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ElseDoAsync(error =>
            {
                errorCounter += error.Count;
                return Task.CompletedTask;
            });

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(1, errorCounter);
    }

    [Fact]
    public async Task CallingElseDoAsync_AfterThenAsync_WhenIsSuccess_ShouldNotInvokeGivenAction()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        int errorCounter = 0;
        Result<int> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ElseDoAsync(error =>
            {
                errorCounter += error.Count;
                return Task.CompletedTask;
            });

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(5, result.Value);
        Assert.Equal(0, errorCounter);
    }
}
