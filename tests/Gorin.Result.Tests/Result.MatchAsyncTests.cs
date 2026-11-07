using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public class MatchAsyncTests
{
    private record Person(string Name);

    [Fact]
    public async Task CallingMatchAsync_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        Result<Person> resultPerson = new Person("Marie");
        Task<string> ThenAction(Person person)
        {
            Assert.Equivalent(resultPerson.Value, person);
            return Task.FromResult("Nice");
        }

        Task<string> ElsesAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        // Act
        Func<Task<string>> action = async () => await resultPerson.MatchAsync(
            ThenAction,
            ElsesAction);

        // Assert
        var result = await action();
        Assert.Equal("Nice", result);
    }

    [Fact]
    public async Task CallingMatchAsync_WhenIsError_ShouldExecuteElseAction()
    {
        // Arrange
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        Task<string> ThenAction(Person _) => throw new Exception("Should not be called");

        Task<string> ElsesAction(IReadOnlyList<Error> errors)
        {
            Assert.Equivalent(resultPerson.Errors, errors);
            return Task.FromResult("Nice");
        }

        // Act
        Func<Task<string>> action = async () => await resultPerson.MatchAsync(
            ThenAction,
            ElsesAction);

        // Assert
        var result = await action();
        Assert.Equal("Nice", result);
    }

    [Fact]
    public async Task CallingMatchFirstAsync_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        Result<Person> resultPerson = new Person("Marie");
        Task<string> ThenAction(Person person)
        {
            Assert.Equivalent(resultPerson.Value, person);
            return Task.FromResult("Nice");
        }

        Task<string> OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        Func<Task<string>> action = async () => await resultPerson.MatchFirstAsync(
            ThenAction,
            OnFirstErrorAction);

        // Assert
        var result = await action();
        Assert.Equal("Nice", result);
    }

    [Fact]
    public async Task CallingMatchFirstAsync_WhenIsError_ShouldExecuteOnFirstErrorAction()
    {
        // Arrange
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        Task<string> ThenAction(Person _) => throw new Exception("Should not be called");
        Task<string> OnFirstErrorAction(Error errors)
        {
            Assert.Equivalent(resultPerson.Errors[0], errors);
            Assert.Equivalent(resultPerson.FirstError, errors);

            return Task.FromResult("Nice");
        }

        // Act
        Func<Task<string>> action = async () => await resultPerson.MatchFirstAsync(
            ThenAction,
            OnFirstErrorAction);

        // Assert
        var result = await action();
        Assert.Equal("Nice", result);
    }

    [Fact]
    public async Task CallingMatchFirstAsyncAfterThenAsync_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        Task<string> ThenAction(Person _) => throw new Exception("Should not be called");
        Task<string> OnFirstErrorAction(Error errors)
        {
            Assert.Equivalent(resultPerson.Errors[0], errors);
            Assert.Equivalent(resultPerson.FirstError, errors);

            return Task.FromResult("Nice");
        }

        // Act
        Func<Task<string>> action = () => resultPerson
            .ThenAsync(person => Task.FromResult(person))
            .MatchFirstAsync(ThenAction, OnFirstErrorAction);

        // Assert
        var result = await action();
        Assert.Equal("Nice", result);
    }

    [Fact]
    public async Task CallingMatchAsyncAfterThenAsync_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        Task<string> ThenAction(Person _) => throw new Exception("Should not be called");

        Task<string> ElsesAction(IReadOnlyList<Error> errors)
        {
            Assert.Equivalent(resultPerson.Errors, errors);
            return Task.FromResult("Nice");
        }

        // Act
        Func<Task<string>> action = () => resultPerson
            .ThenAsync(person => Task.FromResult(person))
            .MatchAsync(ThenAction, ElsesAction);

        // Assert
        var result = await action();
        Assert.Equal("Nice", result);
    }
}
