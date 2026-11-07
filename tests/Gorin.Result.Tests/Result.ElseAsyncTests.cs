using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public class ElseAsyncTests
{
    [Fact]
    public async Task CallingElseAsyncWithValueFunc_WhenIsSuccess_ShouldNotInvokeElseFunc()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(Convert.ToStringAsync)
            .ElseAsync(errors => Task.FromResult($"Error count: {errors.Count}"));

        // Assert
        Assert.False(result.IsError);
        Assert.Equivalent(resultString.Value, result.Value);
    }

    [Fact]
    public async Task CallingElseAsyncWithValueFunc_WhenIsError_ShouldInvokeElseFunc()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(Convert.ToStringAsync)
            .ElseAsync(errors => Task.FromResult($"Error count: {errors.Count}"));

        // Assert
        Assert.False(result.IsError);
        Assert.Equivalent("Error count: 1", result.Value);
    }

    [Fact]
    public async Task CallingElseAsyncWithValueFunc_WhenIsError_ShouldInvokeElseFunc_ButWithResultReturn()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> GetNewValue() => "OK";

        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(Convert.ToStringAsync)
            .ElseAsync(errors => Task.FromResult(GetNewValue()));

        // Assert
        Assert.False(result.IsError);
        Assert.Equivalent("OK", result.Value);
    }

    [Fact]
    public async Task CallingElseAsyncWithResultValueFunc_WhenIsSuccess_ShouldNotInvokeElseFunc()
    {
        // Arrange
        Result<string> resultString = "5";
        bool elseFuncCalled = false;

        // Act
        Result<string> GetNewValue()
        {
            elseFuncCalled = true;
            return "OK";
        }

        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(Convert.ToStringAsync)
            .ElseAsync(errors => Task.FromResult(GetNewValue()));

        // Assert
        Assert.False(elseFuncCalled);
        Assert.False(result.IsError);
        Assert.Equivalent(resultString.Value, result.Value);
    }

    [Fact]
    public async Task CallingElseAsyncWithResultValueFunc_WhenIsError_ShouldInvokeElseFunc_AndReturnError()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> GetErrorValue() => Error.Unexpected();

        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(Convert.ToStringAsync)
            .ElseAsync(errors => Task.FromResult(GetErrorValue()));

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Unexpected, result.FirstError.Type);
    }

    [Fact]
    public async Task CallingElseAsyncWithValueFunc_WhenIsError_ShouldInvokeElseFunc_ButWithResultReturn_ContinueChaining()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> GetNewValue() => "OK";

        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(Convert.ToStringAsync)
            .ElseAsync(errors => Task.FromResult(GetNewValue()))
            .ThenAsync(value => Task.FromResult(value + "!"));

        // Assert
        Assert.False(result.IsError);
        Assert.Equivalent("OK!", result.Value);
    }

    [Fact]
    public async Task CallingElseAsyncWithValue_WhenIsSuccess_ShouldNotReturnElseValue()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(Convert.ToStringAsync)
            .ElseAsync(Task.FromResult("oh no"));

        // Assert
        Assert.False(result.IsError);
        Assert.Equivalent(resultString.Value, result.Value);
    }

    [Fact]
    public async Task CallingElseAsyncWithValue_WhenIsError_ShouldInvokeElseFunc()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(Convert.ToStringAsync)
            .ElseAsync(Task.FromResult("oh no"));

        // Assert
        Assert.False(result.IsError);
        Assert.Equivalent("oh no", result.Value);
    }

    [Fact]
    public async Task CallingElseAsyncWithError_WhenIsError_ShouldReturnElseError()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(Convert.ToStringAsync)
            .ElseAsync(Task.FromResult(Error.Unexpected()));

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Unexpected, result.FirstError.Type);
    }

    [Fact]
    public async Task CallingElseAsyncWithError_WhenIsSuccess_ShouldNotReturnElseError()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(Convert.ToStringAsync)
            .ElseAsync(Task.FromResult(Error.Unexpected()));

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(resultString.Value, result.Value);
    }

    [Fact]
    public async Task CallingElseAsyncWithErrorFunc_WhenIsError_ShouldReturnElseError()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(Convert.ToStringAsync)
            .ElseAsync(errors => Task.FromResult(Error.Unexpected()));

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Unexpected, result.FirstError.Type);
    }

    [Fact]
    public async Task CallingElseAsyncWithErrorFunc_WhenIsSuccess_ShouldNotReturnElseError()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(Convert.ToStringAsync)
            .ElseAsync(errors => Task.FromResult(Error.Unexpected()));

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(resultString.Value, result.Value);
    }

    [Fact]
    public async Task CallingElseAsyncWithErrorFunc_WhenIsError_ShouldReturnElseErrors()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(Convert.ToStringAsync)
            .ElseAsync(errors => Task.FromResult(new List<Error> { Error.Unexpected() }));

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Unexpected, result.FirstError.Type);
    }

    [Fact]
    public async Task CallingElseAsyncWithErrorFunc_WhenIsSuccess_ShouldNotReturnElseErrors()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        Result<string> result = await resultString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(Convert.ToStringAsync)
            .ElseAsync(errors => Task.FromResult(new List<Error> { Error.Unexpected() }));

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(resultString.Value, result.Value);
    }
}
