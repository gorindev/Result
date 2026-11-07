using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public sealed class ResultEqualityTests
{
    // ReSharper disable once NotAccessedPositionalProperty.Local -- we require this property for these tests
    private record Person(string Name);

    public static readonly TheoryData<Error[], Error[]> DifferentErrors =
        new()
        {
            {
                // Different number of entries
                new[]
                {
                    Error.Validation("User.Name", "Name is too short"),
                },
                new[]
                {
                    Error.Validation("User.Name", "Name is too short"),
                    Error.Validation("User.Age", "User is too young"),
                }
            },
            {
                // Different errors
                new[]
                {
                    Error.Validation("User.Name", "Name is too short"),
                },
                new[]
                {
                    Error.Validation("User.Age", "User is too young"),
                }
            },
        };

    public static readonly TheoryData<string> Names = new() { "Marie", "feO2x" };

    public static readonly TheoryData<string, string> DifferentNames =
        new() { { "Marie", "feO2x" }, { "Tyrion", "Cersei" } };

    [Fact]
    public void Equals_WhenTwoInstancesHaveTheSameErrorsCollection_ShouldReturnTrue()
    {
        var errors = new List<Error>
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };
        Result<Person> resultPerson1 = errors;
        Result<Person> resultPerson2 = errors;

        var result = resultPerson1.Equals(resultPerson2);

        Assert.True(result);
    }

    [Fact]
    public void Equals_WhenTwoInstancesHaveDifferentErrorCollectionsWithSameErrors_ShouldReturnTrue()
    {
        var errors1 = new[]
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };
        var errors2 = new[]
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };
        Result<Person> resultPerson1 = errors1;
        Result<Person> resultPerson2 = errors2;

        var result = resultPerson1.Equals(resultPerson2);

        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(DifferentErrors))]
    public void Equals_WhenTwoInstancesHaveDifferentErrors_ShouldReturnFalse(Error[] errors1, Error[] errors2)
    {
        Result<Person> resultPerson1 = errors1;
        Result<Person> resultPerson2 = errors2;

        var result = resultPerson1.Equals(resultPerson2);

        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(Names))]
    public void Equals_WhenTwoInstancesHaveEqualValues_ShouldReturnTrue(string name)
    {
        Result<Person> resultPerson1 = new Person(name);
        Result<Person> resultPerson2 = new Person(name);

        var result = resultPerson1.Equals(resultPerson2);

        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(DifferentNames))]
    public void Equals_WhenTwoInstancesHaveDifferentValues_ShouldReturnFalse(string name1, string name2)
    {
        Result<Person> resultPerson1 = new Person(name1);
        Result<Person> resultPerson2 = new Person(name2);

        var result = resultPerson1.Equals(resultPerson2);

        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(Names))]
    public void GetHashCode_WhenTwoInstancesHaveEqualValues_ShouldReturnSameHashCode(string name)
    {
        Result<Person> resultPerson1 = new Person(name);
        Result<Person> resultPerson2 = new Person(name);

        var hashCode1 = resultPerson1.GetHashCode();
        var hashCode2 = resultPerson2.GetHashCode();

        Assert.Equal(hashCode2, hashCode1);
    }

    [Theory]
    [MemberData(nameof(DifferentNames))]
    public void GetHashCode_WhenTwoInstanceHaveDifferentValues_ShouldReturnDifferentHashCodes(
        string name1,
        string name2)
    {
        Result<Person> resultPerson1 = new Person(name1);
        Result<Person> resultPerson2 = new Person(name2);

        var hashCode1 = resultPerson1.GetHashCode();
        var hashCode2 = resultPerson2.GetHashCode();

        Assert.NotEqual(hashCode2, hashCode1);
    }

    [Fact]
    public void GetHashCode_WhenTwoInstancesHaveEqualErrors_ShouldReturnSameHashCode()
    {
        var errors1 = new[]
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };
        var errors2 = new[]
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };
        Result<Person> resultPerson1 = errors1;
        Result<Person> resultPerson2 = errors2;

        var hashCode1 = resultPerson1.GetHashCode();
        var hashCode2 = resultPerson2.GetHashCode();

        Assert.Equal(hashCode2, hashCode1);
    }

    [Theory]
    [MemberData(nameof(DifferentErrors))]
    public void GetHashCode_WhenTwoInstancesHaveDifferentErrors_ShouldReturnDifferentHashCodes(
        Error[] errors1,
        Error[] errors2)
    {
        Result<Person> resultPerson1 = errors1;
        Result<Person> resultPerson2 = errors2;

        var hashCode1 = resultPerson1.GetHashCode();
        var hashCode2 = resultPerson2.GetHashCode();

        Assert.NotEqual(hashCode2, hashCode1);
    }
}
