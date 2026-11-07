using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public class ThenEnsureAsyncTests
{
    [Fact]
    public async Task CallingThenEnsureAsync_WhenEnsureSucceeds_ShouldReturnOriginalValue()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = await resultInt
            .ThenEnsureAsync(num => Task.FromResult<Result<int>>(num > 3 ? num + 10 : Error.Validation()));

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(5, result.Value);
    }

    [Fact]
    public async Task CallingThenEnsureAsync_WhenEnsureReturnsError_ShouldReturnEnsureErrors()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = await resultInt
            .ThenEnsureAsync(num => Task.FromResult<Result<int>>(num > 3 ? Error.Validation(description: $"{num} is too big") : num));

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.FirstError.Type);
        Assert.Equal("5 is too big", result.FirstError.Description);
    }

    [Fact]
    public async Task CallingThenEnsureAsync_WhenIsError_ShouldNotInvokeEnsure()
    {
        // Arrange
        Result<int> resultInt = Error.NotFound();
        bool called = false;

        // Act
        Result<int> result = await resultInt
            .ThenEnsureAsync(num =>
            {
                called = true;
                return Task.FromResult<Result<int>>(num);
            });

        // Assert
        Assert.False(called);
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.NotFound, result.FirstError.Type);
    }

    [Fact]
    public async Task CallingThenEnsureAsyncAfterThenAsync_WhenEnsureSucceeds_ShouldContinueChainWithOriginalValue()
    {
        // Arrange
        Result<string> resultString = "5";
        int valueFromThenDo = 0;

        // Act
        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .Then(num => num * 2)
            .ThenEnsureAsync(num => Task.FromResult<Result<int>>(num > 3 ? num + 100 : Error.Validation()))
            .ThenDoAsync(num =>
            {
                valueFromThenDo = num;
                return Task.CompletedTask;
            })
            .Then(Convert.ToString);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal("10", result.Value);
        Assert.Equal(10, valueFromThenDo);
    }

    [Fact]
    public async Task CallingThenEnsureAsyncAfterThenAsync_WhenEnsureReturnsError_ShouldNotInvokeLaterChain()
    {
        // Arrange
        Result<string> resultString = "5";
        bool thenDoCalled = false;
        bool thenCalled = false;

        // Act
        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .Then(num => num + 1)
            .ThenEnsureAsync(num => Task.FromResult<Result<int>>(num > 3 ? Error.Validation(description: $"{num} is too big") : num))
            .ThenDoAsync(_ =>
            {
                thenDoCalled = true;
                return Task.CompletedTask;
            })
            .Then(num =>
            {
                thenCalled = true;
                return num.ToString();
            });

        // Assert
        Assert.False(thenDoCalled);
        Assert.False(thenCalled);
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.FirstError.Type);
        Assert.Equal("6 is too big", result.FirstError.Description);
    }

    [Fact]
    public async Task CallingThenEnsureAsyncExtensionMethod_WhenEnsureSucceeds_ShouldReturnOriginalValue()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = await Task.FromResult(resultInt)
            .ThenEnsureAsync(num => Task.FromResult<Result<int>>(num > 3 ? num + 100 : Error.Validation()));

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(5, result.Value);
    }
}
