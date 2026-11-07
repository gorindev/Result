using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public class ThenAsyncTests
{
    [Fact]
    public async Task CallingThenAsync_WhenIsSuccess_ShouldInvokeNextThen()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(num => Task.FromResult(num * 2))
            .ThenDoAsync(num => Task.Run(() => { _ = 5; }))
            .ThenAsync(Convert.ToStringAsync);

        // Assert
        Assert.False(result.IsError);
        Assert.True(result.IsSuccess);
        Assert.Equivalent("10", result.Value);
    }

    [Fact]
    public async Task CallingThenAsync_WhenIsError_ShouldReturnErrors()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(num => Task.FromResult(num * 2))
            .ThenDoAsync(num => Task.Run(() => { _ = 5; }))
            .ThenAsync(Convert.ToStringAsync);

        // Assert
        Assert.True(result.IsError);
        Assert.False(result.IsSuccess);
        Assert.Equivalent(resultString.FirstError, result.FirstError);
    }
}
