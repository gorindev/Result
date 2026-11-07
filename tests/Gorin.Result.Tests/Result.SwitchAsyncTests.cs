using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public class SwitchAsyncTests
{
    private record Person(string Name);

    [Fact]
    public async Task CallingSwitchAsync_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        Result<Person> resultPerson = new Person("Marie");
        Task ThenAction(Person person)
        {
            Assert.Equivalent(resultPerson.Value, person);
            return Task.CompletedTask;
        }

        Task ElsesAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        // Act
        Func<Task> action = async () => await resultPerson.SwitchAsync(
            ThenAction,
            ElsesAction);

        // Assert
        await action();
    }

    [Fact]
    public async Task CallingSwitchAsync_WhenIsError_ShouldExecuteElseAction()
    {
        // Arrange
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        Task ThenAction(Person _) => throw new Exception("Should not be called");
        Task ElsesAction(IReadOnlyList<Error> errors)
        {
            Assert.Equivalent(resultPerson.Errors, errors);
            return Task.CompletedTask;
        }

        // Act
        Func<Task> action = async () => await resultPerson.SwitchAsync(
            ThenAction,
            ElsesAction);

        // Assert
        await action();
    }

    [Fact]
    public async Task CallingSwitchFirstAsync_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        Result<Person> resultPerson = new Person("Marie");
        Task ThenAction(Person person)
        {
            Assert.Equivalent(resultPerson.Value, person);
            return Task.CompletedTask;
        }

        Task OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        Func<Task> action = async () => await resultPerson.SwitchFirstAsync(
            ThenAction,
            OnFirstErrorAction);

        // Assert
        await action();
    }

    [Fact]
    public async Task CallingSwitchFirstAsync_WhenIsError_ShouldExecuteOnFirstErrorAction()
    {
        // Arrange
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        Task ThenAction(Person _) => throw new Exception("Should not be called");
        Task OnFirstErrorAction(Error errors)
        {
            Assert.Equivalent(resultPerson.Errors[0], errors);
            Assert.Equivalent(resultPerson.FirstError, errors);
            return Task.CompletedTask;
        }

        // Act
        Func<Task> action = async () => await resultPerson.SwitchFirstAsync(
            ThenAction,
            OnFirstErrorAction);

        // Assert
        await action();
    }

    [Fact]
    public async Task CallingSwitchFirstAsyncAfterThenAsync_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        Result<Person> resultPerson = new Person("Marie");
        Task ThenAction(Person person)
        {
            Assert.Equivalent(resultPerson.Value, person);
            return Task.CompletedTask;
        }

        Task OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        Func<Task> action = async () => await resultPerson
            .ThenAsync(person => Task.FromResult(person))
            .SwitchFirstAsync(
                ThenAction,
                OnFirstErrorAction);

        // Assert
        await action();
    }

    [Fact]
    public async Task CallingSwitchAsyncAfterThenAsync_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        Result<Person> resultPerson = new Person("Marie");
        Task ThenAction(Person person)
        {
            Assert.Equivalent(resultPerson.Value, person);
            return Task.CompletedTask;
        }

        Task ElsesAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        // Act
        Func<Task> action = async () => await resultPerson
            .ThenAsync(person => Task.FromResult(person))
            .SwitchAsync(ThenAction, ElsesAction);

        // Assert
        await action();
    }
}
