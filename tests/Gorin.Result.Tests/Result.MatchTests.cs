using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public class MatchTests
{
    private record Person(string Name);

    [Fact]
    public void CallingMatch_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        Result<Person> resultPerson = new Person("Marie");
        string ThenAction(Person person)
        {
            Assert.Equivalent(resultPerson.Value, person);
            return "Nice";
        }

        string ElsesAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        // Act
        Func<string> action = () => resultPerson.Match(
            ThenAction,
            ElsesAction);

        // Assert
        var result = action();
        Assert.Equal("Nice", result);
    }

    [Fact]
    public void CallingMatch_WhenIsError_ShouldExecuteElseAction()
    {
        // Arrange
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        string ThenAction(Person _) => throw new Exception("Should not be called");

        string ElsesAction(IReadOnlyList<Error> errors)
        {
            Assert.Equivalent(resultPerson.Errors, errors);
            return "Nice";
        }

        // Act
        Func<string> action = () => resultPerson.Match(
            ThenAction,
            ElsesAction);

        // Assert
        var result = action();
        Assert.Equal("Nice", result);
    }

    [Fact]
    public void CallingMatchFirst_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        Result<Person> resultPerson = new Person("Marie");
        string ThenAction(Person person)
        {
            Assert.Equivalent(resultPerson.Value, person);
            return "Nice";
        }

        string OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        Func<string> action = () => resultPerson.MatchFirst(
            ThenAction,
            OnFirstErrorAction);

        // Assert
        var result = action();
        Assert.Equal("Nice", result);
    }

    [Fact]
    public void CallingMatchFirst_WhenIsError_ShouldExecuteOnFirstErrorAction()
    {
        // Arrange
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        string ThenAction(Person _) => throw new Exception("Should not be called");
        string OnFirstErrorAction(Error errors)
        {
            Assert.Equivalent(resultPerson.Errors[0], errors);
            Assert.Equivalent(resultPerson.FirstError, errors);

            return "Nice";
        }

        // Act
        Func<string> action = () => resultPerson.MatchFirst(
            ThenAction,
            OnFirstErrorAction);

        // Assert
        var result = action();
        Assert.Equal("Nice", result);
    }

    [Fact]
    public async Task CallingMatchFirstAfterThenAsync_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        Result<Person> resultPerson = new Person("Marie");
        string ThenAction(Person person)
        {
            Assert.Equivalent(resultPerson.Value, person);
            return "Nice";
        }

        string OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        Func<Task<string>> action = () => resultPerson
            .ThenAsync(person => Task.FromResult(person))
            .MatchFirst(ThenAction, OnFirstErrorAction);

        // Assert
        var result = await action();
        Assert.Equal("Nice", result);
    }

    [Fact]
    public async Task CallingMatchAfterThenAsync_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        Result<Person> resultPerson = new Person("Marie");
        string ThenAction(Person person)
        {
            Assert.Equivalent(resultPerson.Value, person);
            return "Nice";
        }

        string ElsesAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        // Act
        Func<Task<string>> action = () => resultPerson
            .ThenAsync(person => Task.FromResult(person))
            .Match(ThenAction, ElsesAction);

        // Assert
        var result = await action();
        Assert.Equal("Nice", result);
    }
}
