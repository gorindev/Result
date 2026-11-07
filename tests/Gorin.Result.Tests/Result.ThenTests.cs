using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public class ThenTests
{
    [Fact]
    public void CallingThen_WhenIsSuccess_ShouldInvokeGivenFunc()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        Result<string> result = resultString
            .Then(Convert.ToInt)
            .Then(num => num * 2)
            .Then(Convert.ToString);

        // Assert
        Assert.False(result.IsError);
        Assert.True(result.IsSuccess);
        Assert.Equivalent("10", result.Value);
    }

    [Fact]
    public void CallingThen_WhenIsSuccess_ShouldInvokeGivenAction()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        Result<int> result = resultString
            .ThenDo(str => { _ = 5; })
            .Then(Convert.ToInt)
            .ThenDo(str => { _ = 5; });

        // Assert
        Assert.False(result.IsError);
        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Value);
    }

    [Fact]
    public void CallingThen_WhenIsError_ShouldReturnErrors()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = resultString
            .Then(Convert.ToInt)
            .Then(num => num * 2)
            .ThenDo(str => { _ = 5; })
            .Then(Convert.ToString);

        // Assert
        Assert.True(result.IsError);
        Assert.False(result.IsSuccess);
        Assert.Equivalent(resultString.FirstError, result.FirstError);
    }

    [Fact]
    public async Task CallingThenAfterThenAsync_WhenIsSuccess_ShouldInvokeGivenFunc()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .Then(num => num * 2)
            .ThenAsync(Convert.ToStringAsync)
            .Then(Convert.ToInt)
            .ThenAsync(Convert.ToStringAsync)
            .ThenDo(num => { _ = 5; });

        // Assert
        Assert.False(result.IsError);
        Assert.True(result.IsSuccess);
        Assert.Equal("10", result.Value);
    }

    [Fact]
    public async Task CallingThenAfterThenAsync_WhenIsError_ShouldReturnErrors()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .Then(Convert.ToString);

        // Assert
        Assert.True(result.IsError);
        Assert.False(result.IsSuccess);
        Assert.Equivalent(resultString.FirstError, result.FirstError);
    }
}
