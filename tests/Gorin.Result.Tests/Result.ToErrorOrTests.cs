using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public class ToResultTests
{
    [Fact]
    public void ValueToResult_WhenAccessingValue_ShouldReturnValue()
    {
        // Arrange
        int value = 5;

        // Act
        Result<int> result = value.ToResult();

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public async Task TaskToResultAsync_WhenAccessingValue_ShouldReturnValue()
    {
        // Arrange
        int value = 5;
        Task<int> task = Task.FromResult(value);

        // Act
        Result<int> result = await task.ToResultAsync();

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void ErrorToResult_WhenAccessingFirstError_ShouldReturnSameError()
    {
        // Arrange
        Error error = Error.Unauthorized();

        // Act
        Result<int> result = error.ToResult<int>();

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(error, result.FirstError);
    }

    [Fact]
    public async Task ErrorToResultAsync_WhenAccessingFirstError_ShouldReturnSameError()
    {
        // Arrange
        Error error = Error.Unauthorized();
        Task<Error> task = Task.FromResult(error);

        // Act
        Result<int> result = await task.ToResultAsync<int>();

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(error, result.FirstError);
    }

    [Fact]
    public void ListOfErrorsToResult_WhenAccessingErrors_ShouldReturnSameErrors()
    {
        // Arrange
        List<Error> errors = [Error.Unauthorized(), Error.Validation()];

        // Act
        Result<int> result = errors.ToResult<int>();

        // Assert
        Assert.True(result.IsError);
        Assert.Equivalent(errors, result.Errors);
    }

    [Fact]
    public async Task ListOfErrorsToResultAsync_WhenAccessingErrors_ShouldReturnSameErrors()
    {
        // Arrange
        List<Error> errors = [Error.Unauthorized(), Error.Validation()];
        Task<List<Error>> task = Task.FromResult(errors);

        // Act
        Result<int> result = await task.ToResultAsync<int>();

        // Assert
        Assert.True(result.IsError);
        Assert.Equivalent(errors, result.Errors);
    }

    [Fact]
    public void EnumerableOfErrorsToResult_WhenAccessingErrors_ShouldReturnSimilarErrors()
    {
        // Arrange
        Error[] errors = [Error.Unauthorized(), Error.Validation()];

        // Act
        Result<int> result = errors.ToResult<int>();

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(errors, result.Errors);
    }

    [Fact]
    public async Task ArrayOfErrorsToResultAsync_WhenAccessingErrors_ShouldReturnSimilarErrors()
    {
        // Arrange
        Error[] errors = [Error.Unauthorized(), Error.Validation()];
        Task<Error[]> task = Task.FromResult(errors);

        // Act
        Result<int> result = await task.ToResultAsync<int>();

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(errors, result.Errors);
    }

    [Fact]
    public async Task EnumerableOfErrorsToResultAsync_WhenAccessingErrors_ShouldReturnSimilarErrors()
    {
        // Arrange
        Error[] errors = [Error.Unauthorized(), Error.Validation()];
        Task<IEnumerable<Error>> task = Task.FromResult((IEnumerable<Error>)errors);

        // Act
        Result<int> result = await task.ToResultAsync<int>();

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(errors, result.Errors);
    }
}
