using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public class SwitchTests
{
    private record Person(string Name);

    [Fact]
    public void CallingSwitch_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        Result<Person> resultPerson = new Person("Marie");
        void ThenAction(Person person) => Assert.Equivalent(resultPerson.Value, person);
        void ElsesAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        // Act
        Action action = () => resultPerson.Switch(
            ThenAction,
            ElsesAction);

        // Assert
        action();
    }

    [Fact]
    public void CallingSwitch_WhenIsError_ShouldExecuteElseAction()
    {
        // Arrange
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        void ThenAction(Person _) => throw new Exception("Should not be called");
        void ElsesAction(IReadOnlyList<Error> errors) => Assert.Equivalent(resultPerson.Errors, errors);

        // Act
        Action action = () => resultPerson.Switch(
            ThenAction,
            ElsesAction);

        // Assert
        action();
    }

    [Fact]
    public void CallingSwitchFirst_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        Result<Person> resultPerson = new Person("Marie");
        void ThenAction(Person person) => Assert.Equivalent(resultPerson.Value, person);
        void OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        Action action = () => resultPerson.SwitchFirst(
            ThenAction,
            OnFirstErrorAction);

        // Assert
        action();
    }

    [Fact]
    public void CallingSwitchFirst_WhenIsError_ShouldExecuteOnFirstErrorAction()
    {
        // Arrange
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        void ThenAction(Person _) => throw new Exception("Should not be called");
        void OnFirstErrorAction(Error errors)
        {
            Assert.Equivalent(resultPerson.Errors[0], errors);
            Assert.Equivalent(resultPerson.FirstError, errors);
        }

        // Act
        Action action = () => resultPerson.SwitchFirst(
            ThenAction,
            OnFirstErrorAction);

        // Assert
        action();
    }

    [Fact]
    public async Task CallingSwitchFirstAfterThenAsync_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        Result<Person> resultPerson = new Person("Marie");
        void ThenAction(Person person) => Assert.Equivalent(resultPerson.Value, person);
        void OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        Func<Task> action = () => resultPerson
            .ThenAsync(person => Task.FromResult(person))
            .SwitchFirst(ThenAction, OnFirstErrorAction);

        // Assert
        await action();
    }

    [Fact]
    public async Task CallingSwitchAfterThenAsync_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        Result<Person> resultPerson = new Person("Marie");
        void ThenAction(Person person) => Assert.Equivalent(resultPerson.Value, person);
        void ElsesAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        // Act
        Func<Task> action = () => resultPerson
            .ThenAsync(person => Task.FromResult(person))
            .Switch(ThenAction, ElsesAction);

        // Assert
        await action();
    }
}
