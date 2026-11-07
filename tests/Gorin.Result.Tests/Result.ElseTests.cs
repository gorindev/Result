using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public class ElseTests
{
    [Fact]
    public void CallingElseWithValueFunc_WhenIsSuccess_ShouldNotInvokeElseFunc()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        Result<string> result = resultString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else(errors => $"Error count: {errors.Count}");

        // Assert
        Assert.False(result.IsError);
        Assert.Equivalent(resultString.Value, result.Value);
    }

    [Fact]
    public void CallingElseWithValueFunc_WhenIsError_ShouldInvokeElseFunc()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = resultString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else(errors => $"Error count: {errors.Count}");

        // Assert
        Assert.False(result.IsError);
        Assert.Equivalent("Error count: 1", result.Value);
    }

    [Fact]
    public void CallingElseWithValue_WhenIsSuccess_ShouldNotReturnElseValue()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        Result<string> result = resultString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else("oh no");

        // Assert
        Assert.False(result.IsError);
        Assert.Equivalent(resultString.Value, result.Value);
    }

    [Fact]
    public void CallingElseWithValue_WhenIsError_ShouldInvokeElseFunc()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = resultString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else("oh no");

        // Assert
        Assert.False(result.IsError);
        Assert.Equivalent("oh no", result.Value);
    }

    [Fact]
    public void CallingElseWithError_WhenIsError_ShouldReturnElseError()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = resultString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else(Error.Unexpected());

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Unexpected, result.FirstError.Type);
    }

    [Fact]
    public void CallingElseWithError_WhenIsSuccess_ShouldNotReturnElseError()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        Result<string> result = resultString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else(Error.Unexpected());

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(resultString.Value, result.Value);
    }

    [Fact]
    public void CallingElseWithErrorsFunc_WhenIsError_ShouldReturnElseError()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = resultString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else(errors => Error.Unexpected());

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Unexpected, result.FirstError.Type);
    }

    [Fact]
    public void CallingElseWithErrorsFunc_WhenIsSuccess_ShouldNotReturnElseError()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        Result<string> result = resultString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else(errors => Error.Unexpected());

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(resultString.Value, result.Value);
    }

    [Fact]
    public void CallingElseWithErrorsFunc_WhenIsError_ShouldReturnElseErrors()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = resultString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else(errors => [Error.Unexpected()]);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Unexpected, result.FirstError.Type);
    }

    [Fact]
    public void CallingElseWithErrorsFunc_WhenIsSuccess_ShouldNotReturnElseErrors()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        Result<string> result = resultString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else(errors => [Error.Unexpected()]);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(resultString.Value, result.Value);
    }

    [Fact]
    public async Task CallingElseWithValueAfterThenAsync_WhenIsError_ShouldInvokeElseFunc()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = await resultString
            .Then(Convert.ToInt)
            .ThenAsync(Convert.ToStringAsync)
            .Else("oh no");

        // Assert
        Assert.False(result.IsError);
        Assert.Equivalent("oh no", result.Value);
    }

    [Fact]
    public async Task CallingElseWithValueFuncAfterThenAsync_WhenIsError_ShouldInvokeElseFunc()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = await resultString
            .Then(Convert.ToInt)
            .ThenAsync(Convert.ToStringAsync)
            .Else(errors => $"Error count: {errors.Count}");

        // Assert
        Assert.False(result.IsError);
        Assert.Equivalent("Error count: 1", result.Value);
    }

    [Fact]
    public async Task CallingElseWithValueFuncAfterThenAsync_WhenIsError_ShouldInvokeElseFunc_ButWithResultReturn()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> GetNewValue() => "OK";

        Result<string> result = await resultString
            .Then(Convert.ToInt)
            .ThenAsync(Convert.ToStringAsync)
            .Else(errors => GetNewValue());

        // Assert
        Assert.False(result.IsError);
        Assert.Equivalent("OK", result.Value);
    }

    [Fact]
    public async Task CallingElseWithValueFuncAfterThenAsync_WhenIsError_ShouldInvokeElseFunc_ButWithResultReturn_ContinueChaining()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> GetNewValue() => "OK";

        Result<string> result = await resultString
            .Then(Convert.ToInt)
            .ThenAsync(Convert.ToStringAsync)
            .Else(errors => GetNewValue())
            .Then(value => value + "!");

        // Assert
        Assert.False(result.IsError);
        Assert.Equivalent("OK!", result.Value);
    }

    [Fact]
    public async Task CallingElseWithErrorAfterThenAsync_WhenIsError_ShouldReturnElseError()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = await resultString
            .Then(Convert.ToInt)
            .ThenAsync(Convert.ToStringAsync)
            .Else(Error.Unexpected());

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Unexpected, result.FirstError.Type);
    }

    [Fact]
    public async Task CallingElseWithErrorFuncAfterThenAsync_WhenIsError_ShouldReturnElseError()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = await resultString
            .Then(Convert.ToInt)
            .ThenAsync(Convert.ToStringAsync)
            .Else(errors => Error.Unexpected());

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Unexpected, result.FirstError.Type);
    }

    [Fact]
    public async Task CallingElseWithErrorFuncAfterThenAsync_WhenIsError_ShouldReturnElseErrors()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> result = await resultString
            .Then(Convert.ToInt)
            .ThenAsync(Convert.ToStringAsync)
            .Else(errors => [Error.Unexpected()]);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Unexpected, result.FirstError.Type);
    }

    [Fact]
    public void CallingElseWithResultValueFunc_WhenIsSuccess_ShouldNotInvokeElseFunc()
    {
        // Arrange
        Result<string> resultString = "5";

        // Act
        Result<string> GetNewValue() => "OK";
        Result<string> result = resultString
            .Else(errors => GetNewValue());

        // Assert
        Assert.False(result.IsError);
        Assert.Equivalent(resultString.Value, result.Value);
    }

    [Fact]
    public void CallingElseWithResultValueFunc_WhenIsError_ShouldInvokeElseFunc_AndReturnError()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> GetErrorValue() => Error.Unexpected();
        Result<string> result = resultString
            .Else(errors => GetErrorValue());

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Unexpected, result.FirstError.Type);
    }

    [Fact]
    public async Task CallingElseWithResultValueFunc_WhenIsError_ShouldReturnElseErrors()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> GetErrorValue() => Error.Unexpected();
        Result<string> result = await resultString
            .Then(Convert.ToInt)
            .ThenAsync(Convert.ToStringAsync)
            .Else(errors => GetErrorValue());

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Unexpected, result.FirstError.Type);
    }

    [Fact]
    public async Task CallingElseWithResultValueFunc_WhenIsError_ShouldReturnElseFuncValue()
    {
        // Arrange
        Result<string> resultString = Error.NotFound();

        // Act
        Result<string> GetErrorValue() => "5";
        Result<string> result = await resultString
            .Then(Convert.ToInt)
            .ThenAsync(Convert.ToStringAsync)
            .Else(errors => GetErrorValue());

        // Assert
        Assert.False(result.IsError);
        Assert.Equal("5", result.Value);
    }
}
