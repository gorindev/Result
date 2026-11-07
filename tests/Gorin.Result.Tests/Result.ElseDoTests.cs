using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public class ElseDoTests
{
    [Fact]
    public async Task CallingElseDo_WhenIsError_ShouldInvokeSyncAction()
    {
        // Arrange
        Task<Result<string>> resultTask = Task.FromResult(ResultFactory.From<string>(Error.Validation()));

        // Act
        int errorCounter = 0;
        Result<string> result = await resultTask
            .ElseDo(error => errorCounter += error.Count);

        // Assert
        Assert.Equal(1, errorCounter);
        Assert.True(result.IsError);
    }

    [Fact]
    public async Task CallingElseDo_WhenIsSuccess_ShouldInvokeSyncAction()
    {
        // Arrange
        Task<Result<string>> resultTask = Task.FromResult(ResultFactory.From<string>("5"));

        // Act
        int errorCounter = 0;
        Result<string> result = await resultTask
            .ElseDo(error => errorCounter += error.Count);

        // Assert
        Assert.Equal(0, errorCounter);
        Assert.False(result.IsError);
        Assert.Equal("5", result.Value);
    }

    [Fact]
    public void CallingElseDo_WhenIsError_ShouldInvokeGivenAction()
    {
        // Arrange
        Result<string> resultString = Error.Validation();

        // Act
        int errorCounter = 0;
        Result<int> result = resultString
            .Then(Convert.ToInt)
            .ElseDo(error => errorCounter += error.Count())
            .Then(_ => 10);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.FirstError.Type);
        Assert.Equal(1, errorCounter);
    }

    [Fact]
    public void CallingElseDo_WhenIsSuccess_ShouldInvokeGivenAction()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        int errorCounter = 0;
        Result<int> result = resultString
            .Then(Convert.ToInt)
            .ElseDo(error => errorCounter += error.Count())
            .Then(_ => 10);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(0, errorCounter);
        Assert.Equal(10, result.Value);
    }

    [Fact]
    public void CallingElseDo_WhenIsSuccess_ShouldNotInvokeGivenAction()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        int errorCounter = 0;
        Result<int> result = resultString
            .Then(Convert.ToInt)
            .ElseDo(error => errorCounter += error.Count());

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(5, result.Value);
        Assert.Equal(0, errorCounter);
    }

    [Fact]
    public void CallingElseDo_WhenIsError_ShouldInvokeNextElse()
    {
        // Arrange
        Result<string> resultString = Error.Failure();

        // Act
        int errorCounter = 0;
        Result<string> result = resultString
            .ElseDo(error => errorCounter += error.Count())
            .Else(error => $"error count is {errorCounter}");

        // Assert
        Assert.False(result.IsError);
        Assert.Equal("error count is 1", result.Value);
        Assert.Equal(1, errorCounter);
    }
}
