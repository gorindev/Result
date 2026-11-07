using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public class ThenEnsureTests
{
    [Fact]
    public void CallingThenEnsure_WhenEnsureSucceeds_ShouldReturnOriginalValue()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = resultInt
            .ThenEnsure(num => num > 10 ? Error.Validation() : num + 10);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(5, result.Value);
    }

    [Fact]
    public void CallingThenEnsure_WhenEnsureReturnsError_ShouldReturnEnsureErrors()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = resultInt
            .ThenEnsure(num => num > 3 ? Error.Validation(description: $"{num} is too big") : num);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.FirstError.Type);
        Assert.Equal("5 is too big", result.FirstError.Description);
    }

    [Fact]
    public void CallingThenEnsure_WhenIsError_ShouldNotInvokeEnsure()
    {
        // Arrange
        Result<int> resultInt = Error.NotFound();
        bool called = false;

        // Act
        Result<int> result = resultInt
            .ThenEnsure(num =>
            {
                called = true;
                return num;
            });

        // Assert
        Assert.False(called);
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.NotFound, result.FirstError.Type);
    }

    [Fact]
    public void CallingThenEnsureAfterThen_WhenEnsureSucceeds_ShouldContinueChainWithOriginalValue()
    {
        // Arrange
        Result<string> resultString = "5";
        int valueFromThenDo = 0;

        // Act
        Result<string> result = resultString
            .Then(Convert.ToInt)
            .ThenEnsure(num => num > 3 ? num + 10 : Error.Validation())
            .ThenDo(num => valueFromThenDo = num)
            .Then(Convert.ToString);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal("5", result.Value);
        Assert.Equal(5, valueFromThenDo);
    }

    [Fact]
    public void CallingThenEnsureAfterThen_WhenEnsureReturnsError_ShouldNotInvokeLaterChain()
    {
        // Arrange
        Result<int> resultInt = 5;
        bool thenDoCalled = false;
        bool thenCalled = false;

        // Act
        Result<int> result = resultInt
            .Then(num => num + 1)
            .ThenEnsure(num => num > 3 ? Error.Validation(description: $"{num} is too big") : num)
            .ThenDo(_ => thenDoCalled = true)
            .Then(num =>
            {
                thenCalled = true;
                return num * 2;
            });

        // Assert
        Assert.False(thenDoCalled);
        Assert.False(thenCalled);
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Validation, result.FirstError.Type);
        Assert.Equal("6 is too big", result.FirstError.Description);
    }

    [Fact]
    public async Task CallingThenEnsureExtensionMethod_WhenEnsureSucceeds_ShouldReturnOriginalValue()
    {
        // Arrange
        Result<int> resultInt = 5;

        // Act
        Result<int> result = await Task.FromResult(resultInt)
            .ThenEnsure(num => num > 3 ? num + 100 : Error.Validation());

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(5, result.Value);
    }
}
