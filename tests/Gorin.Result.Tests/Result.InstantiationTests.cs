using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public class ResultInstantiationTests
{
    private record Person(string Name);

    [Fact]
    public void CreateFromValue_WhenAccessingValue_ShouldReturnValue()
    {
        // Arrange
        IEnumerable<string> value = ["value"];

        // Act
        Result<IEnumerable<string>> resultPerson = ResultFactory.From(value);

        // Assert
        Assert.False(resultPerson.IsError);
        Assert.Same(value, resultPerson.Value);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingValue_ViaStronglyTypedInterface_ShouldReturnValue()
    {
        // Arrange
        IEnumerable<string> value = ["value"];

        // Act
#pragma warning disable CA1859 // Use concrete types when possible for improved performance
        IResult<IEnumerable<string>> resultValue = ResultFactory.From(value);
#pragma warning restore CA1859 // Use concrete types when possible for improved performance

        // Assert
        Assert.False(resultValue.IsError);
        Assert.Same(value, resultValue.Value);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingErrors_ShouldReturnUnexpectedError()
    {
        // Arrange
        IEnumerable<string> value = ["value"];
        Result<IEnumerable<string>> resultPerson = ResultFactory.From(value);

        // Act
        List<Error> errors = resultPerson.Errors;

        // Assert
        var error = Assert.Single(errors);
        Assert.Equal(ErrorType.Unexpected, error.Type);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingErrorsOrEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        IEnumerable<string> value = ["value"];
        Result<IEnumerable<string>> resultPerson = ResultFactory.From(value);

        // Act
        List<Error> errors = resultPerson.ErrorsOrEmptyList;

        // Assert
        Assert.Empty(errors);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingFirstError_ShouldReturnUnexpectedError()
    {
        // Arrange
        IEnumerable<string> value = ["value"];
        Result<IEnumerable<string>> resultPerson = ResultFactory.From(value);

        // Act
        Error firstError = resultPerson.FirstError;

        // Assert
        Assert.Equal(ErrorType.Unexpected, firstError.Type);
    }

    [Fact]
    public async Task CreateFromAsyncValue_WhenAccessingValue_ShouldReturnValue()
    {
        // Arrange
        IEnumerable<string> value = ["value"];

        // Act
        Result<IEnumerable<string>> resultPerson = await ResultFactory.FromAsync(value);

        // Assert
        Assert.False(resultPerson.IsError);
        Assert.Same(value, resultPerson.Value);
    }

    [Fact]
    public void CreateFromErrorList_UsingFactory_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        var error = Error.Validation("User.Name", "Name is too short");

        // Act
        Result<Person> resultPerson = ResultFactory.From<Person>([error]);

        // Assert
        Assert.True(resultPerson.IsError);
        var singleError = Assert.Single(resultPerson.Errors);
        Assert.Equal(error, singleError);
    }

    [Fact]
    public async Task CreateFromAsyncErrorList_UsingFactory_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        var error = Error.Validation("User.Name", "Name is too short");

        // Act
        Result<Person> resultPerson = await ResultFactory.FromAsync<Person>([error]);

        // Assert
        Assert.True(resultPerson.IsError);
        var singleError = Assert.Single(resultPerson.Errors);
        Assert.Equal(error, singleError);
    }

    [Fact]
    public void CreateFromErrorList_UsingFactory_WhenAccessingErrorsOrEmptyList_ShouldReturnErrorList()
    {
        // Arrange
        var error = Error.Validation("User.Name", "Name is too short");

        // Act
        Result<Person> resultPerson = ResultFactory.From<Person>([error]);

        // Assert
        Assert.True(resultPerson.IsError);
        var singleError = Assert.Single(resultPerson.ErrorsOrEmptyList);
        Assert.Equal(error, singleError);
    }

    [Fact]
    public void CreateFromErrorList_UsingFactory_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        Result<Person> resultPerson = ResultFactory.From<Person>([Error.Validation("User.Name", "Name is too short")]);

        // Act
        Person value = resultPerson.Value;

        // Assert
        Assert.Equal(default, value);
    }

    [Fact]
    public void CreateFromSingleError_UsingFactory_ShouldBeError()
    {
        // Act
        Result<Person> resultPerson = ResultFactory.From<Person>(Error.Validation("User.Name", "Name is too short"));

        // Assert
        Assert.True(resultPerson.IsError);
    }

    [Fact]
    public async Task CreateFromAsyncSingleError_UsingFactory_ShouldBeError()
    {
        // Act
        Result<Person> resultPerson = await ResultFactory.FromAsync<Person>(Error.Validation("User.Name", "Name is too short"));

        // Assert
        Assert.True(resultPerson.IsError);
    }

    [Fact]
    public void CreateFromArrayOfErrors_UsingFactory_ShouldBeError()
    {
        // Arrange
        Error[] errors = [
            Error.Validation("User.Name", "Name is too short"),
            Error.Forbidden("User.Forbidden", "You are not allowed to create user")
        ];

        // Act
        Result<Person> resultPerson = ResultFactory.From<Person>(errors);

        // Assert
        Assert.True(resultPerson.IsError);
    }

    [Fact]
    public async Task CreateFromAsyncArrayOfErrors_UsingFactory_ShouldBeError()
    {
        // Arrange
        Error[] errors = [
            Error.Validation("User.Name", "Name is too short"),
            Error.Forbidden("User.Forbidden", "You are not allowed to create user")
        ];

        // Act
        Result<Person> resultPerson = await ResultFactory.FromAsync<Person>(errors);

        // Assert
        Assert.True(resultPerson.IsError);
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingResult_ShouldReturnValue()
    {
        // Arrange
        Person person = new Person("Amici");

        // Act
        Result<Person> result = person;

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(person, result.Value);
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingErrors_ShouldReturnUnexpectedError()
    {
        Result<Person> resultPerson = new Person("Marie");

        // Act
        List<Error> errors = resultPerson.Errors;

        // Assert
        var error = Assert.Single(errors);
        Assert.Equal(ErrorType.Unexpected, error.Type);
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingFirstError_ShouldReturnUnexpectedError()
    {
        Result<Person> resultPerson = new Person("Marie");

        // Act
        Error firstError = resultPerson.FirstError;

        // Assert
        Assert.Equal(ErrorType.Unexpected, firstError.Type);
    }

    [Fact]
    public void ImplicitCastPrimitiveResult_WhenAccessingResult_ShouldReturnValue()
    {
        // Arrange
        const int result = 4;

        // Act
        Result<int> resultInt = result;

        // Assert
        Assert.False(resultInt.IsError);
        Assert.Equal(result, resultInt.Value);
    }

    [Fact]
    public async Task ImplicitCastConditionalToObject_WhenBranchesAreDifferentResultTypes_ShouldNestResultValue()
    {
        // Repro guard for issue #144: conditional branch assignment can wrap Result<T> into Result<object>.
        var useFirstBranch = true;

        Result<object> request = useFirstBranch
            ? await CreateRequestModel1Async()
            : await CreateRequestModel2Async();

        Assert.False(request.IsError);
        Assert.IsType<Result<ModelClass1>>(request.Value);

        return;

        static Task<Result<ModelClass1>> CreateRequestModel1Async()
        {
            return Task.FromResult<Result<ModelClass1>>(new ModelClass1("first"));
        }

        static Task<Result<ModelClass2>> CreateRequestModel2Async()
        {
            return Task.FromResult<Result<ModelClass2>>(new ModelClass2(7));
        }
    }

    [Fact]
    public void ImplicitCastResultType_WhenAccessingResult_ShouldReturnValue()
    {
        // Act
        Result<Success> resultSuccess = ResultType.Success;
        Result<Created> resultCreated = ResultType.Created;
        Result<Deleted> resultDeleted = ResultType.Deleted;
        Result<Updated> resultUpdated = ResultType.Updated;

        // Assert
        Assert.False(resultSuccess.IsError);
        Assert.Equal(ResultType.Success, resultSuccess.Value);

        Assert.False(resultCreated.IsError);
        Assert.Equal(ResultType.Created, resultCreated.Value);

        Assert.False(resultDeleted.IsError);
        Assert.Equal(ResultType.Deleted, resultDeleted.Value);

        Assert.False(resultUpdated.IsError);
        Assert.Equal(ResultType.Updated, resultUpdated.Value);
    }

    private record ModelClass1(string Name);

    private record ModelClass2(int Count);

    [Fact]
    public void ImplicitCastSingleError_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        Error error = Error.Validation("User.Name", "Name is too short");

        // Act
        Result<Person> resultPerson = error;

        // Assert
        Assert.True(resultPerson.IsError);
        var singleError = Assert.Single(resultPerson.Errors);
        Assert.Equal(error, singleError);
    }

    [Fact]
    public void ImplicitCastSingleError_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        Result<Person> resultPerson = Error.Validation("User.Name", "Name is too short");

        // Act
        Person value = resultPerson.Value;

        // Assert
        Assert.Equal(default, value);
    }

    [Fact]
    public void ImplicitCastSingleError_WhenAccessingFirstError_ShouldReturnError()
    {
        // Arrange
        Error error = Error.Validation("User.Name", "Name is too short");

        // Act
        Result<Person> resultPerson = error;

        // Assert
        Assert.True(resultPerson.IsError);
        Assert.Equal(error, resultPerson.FirstError);
    }

    [Fact]
    public void ImplicitCastErrorList_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        List<Error> errors =
        [
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        Result<Person> resultPerson = errors;

        // Assert
        Assert.True(resultPerson.IsError);
        Assert.Equal(errors.Count, resultPerson.Errors.Count);
        Assert.Equivalent(errors, resultPerson.Errors);
    }

    [Fact]
    public void ImplicitCastErrorArray_WhenAccessingErrors_ShouldReturnErrorArray()
    {
        // Arrange
        Error[] errors =
        [
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        Result<Person> resultPerson = errors;

        // Assert
        Assert.True(resultPerson.IsError);
        Assert.Equal(errors.Length, resultPerson.Errors.Count);
        Assert.Equivalent(errors, resultPerson.Errors);
    }

    [Fact]
    public void ImplicitCastErrorList_WhenAccessingFirstError_ShouldReturnFirstError()
    {
        // Arrange
        List<Error> errors =
        [
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        Result<Person> resultPerson = errors;

        // Assert
        Assert.True(resultPerson.IsError);
        Assert.Equal(errors[0], resultPerson.FirstError);
    }

    [Fact]
    public void ImplicitCastErrorArray_WhenAccessingFirstError_ShouldReturnFirstError()
    {
        // Arrange
        Error[] errors =
        [
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        Result<Person> resultPerson = errors;

        // Assert
        Assert.True(resultPerson.IsError);
        Assert.Equal(errors[0], resultPerson.FirstError);
    }

#pragma warning disable SA1129 // Do not use default value type constructor
    [Fact]
    public void CreateWithEmptyConstructor_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        Result<int> resultInt = new Result<int>();

        // Act &Assert
        Assert.False(resultInt.IsError);
        Assert.Equal(default, resultInt.Value);
    }

    [Fact]
    public void CreateWithEmptyConstructor_WhenAccessingErrors_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int> resultInt = new Result<int>();

        // Act
        List<Error> errors = resultInt.Errors;

        // Assert
        var error = Assert.Single(errors);
        Assert.Equal(ErrorType.Unexpected, error.Type);
    }

    [Fact]
    public void CreateWithEmptyConstructor_WhenAccessingErrorsOrEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        Result<int> resultInt = new Result<int>();

        // Act
        List<Error> errors = resultInt.ErrorsOrEmptyList;

        // Assert
        Assert.Empty(errors);
    }

    [Fact]
    public void CreateWithEmptyConstructor_WhenAccessingFirstError_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int> resultInt = new Result<int>();

        // Act
        Error firstError = resultInt.FirstError;

        // Assert
        Assert.Equal(ErrorType.Unexpected, firstError.Type);
    }
#pragma warning restore SA1129 // Do not use default value type constructor

    [Fact]
    public void CreateWithDefault_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        Result<int> resultInt = default;

        // Act & Assert
        Assert.False(resultInt.IsError);
        Assert.Equal(default, resultInt.Value);
    }

    [Fact]
    public void CreateWithDefault_WhenAccessingErrors_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int> resultInt = default;

        // Act
        List<Error> errors = resultInt.Errors;

        // Assert
        var error = Assert.Single(errors);
        Assert.Equal(ErrorType.Unexpected, error.Type);
    }

    [Fact]
    public void CreateWithDefault_WhenAccessingErrorsOrEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        Result<int> resultInt = default;

        // Act
        List<Error> errors = resultInt.ErrorsOrEmptyList;

        // Assert
        Assert.Empty(errors);
    }

    [Fact]
    public void Result_FromErrorCollectionExpression_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        var nameTooShort = Error.Validation("User.Name", "Name is too short");
        var userTooYoung = Error.Validation("User.Age", "User is too young");

        // Act
        Result<Person> resultPerson = [nameTooShort, userTooYoung];

        // Assert
        Assert.True(resultPerson.IsError);
        Assert.Equal(2, resultPerson.Errors.Count);
        Assert.Equivalent(new[] { nameTooShort, userTooYoung }, resultPerson.Errors);
    }

    [Fact]
    public void Result_FromEmptyErrorCollectionExpression_ShouldBeError()
    {
        // Act
        Result<Person> resultPerson = [];

        // Assert
        Assert.True(resultPerson.IsError);
    }

    [Fact]
    public void GenericResultInterface_FromErrorCollectionExpression_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        var nameTooShort = Error.Validation("User.Name", "Name is too short");
        var userTooYoung = Error.Validation("User.Age", "User is too young");

        // Act
        IResult<Person> resultPerson = [nameTooShort, userTooYoung];

        // Assert
        Assert.True(resultPerson.IsError);
        Assert.Equal(2, resultPerson.Errors?.Count);
        Assert.Equivalent(new[] { nameTooShort, userTooYoung }, resultPerson.Errors);
    }

    [Fact]
    public void ResultInterface_FromErrorCollectionExpression_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        var nameTooShort = Error.Validation("User.Name", "Name is too short");
        var userTooYoung = Error.Validation("User.Age", "User is too young");

        // Act
        IResult resultPerson = [nameTooShort, userTooYoung];

        // Assert
        Assert.True(resultPerson.IsError);
        Assert.Equal(2, resultPerson.Errors?.Count);
        Assert.Equivalent(new[] { nameTooShort, userTooYoung }, resultPerson.Errors);
    }

    [Fact]
    public void CreateWithDefault_WhenAccessingFirstError_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int> resultInt = default;

        // Act
        Error firstError = resultInt.FirstError;

        // Assert
        Assert.Equal(ErrorType.Unexpected, firstError.Type);
    }

    [Fact]
    public void ImplicitCastEmptyErrorList_ShouldBeError()
    {
        // Arrange
        Result<int> resultInt = new List<Error>();

        // Act & Assert
        Assert.True(resultInt.IsError);
    }

    [Fact]
    public void ImplicitCastEmptyErrorList_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        Result<int> resultInt = new List<Error>();

        // Act & Assert
        Assert.Equal(default, resultInt.Value);
    }

    [Fact]
    public void ImplicitCastEmptyErrorList_WhenAccessingErrors_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int> resultInt = new List<Error>();

        // Act
        List<Error> errors = resultInt.Errors;

        // Assert
        var error = Assert.Single(errors);
        Assert.Equal(ErrorType.Unexpected, error.Type);
    }

    [Fact]
    public void ImplicitCastEmptyErrorList_WhenAccessingErrorsOrEmptyList_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int> resultInt = new List<Error>();

        // Act
        List<Error> errors = resultInt.ErrorsOrEmptyList;

        // Assert
        var error = Assert.Single(errors);
        Assert.Equal(ErrorType.Unexpected, error.Type);
    }

    [Fact]
    public void ImplicitCastEmptyErrorList_WhenAccessingFirstError_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int> resultInt = new List<Error>();

        // Act
        Error firstError = resultInt.FirstError;

        // Assert
        Assert.Equal(ErrorType.Unexpected, firstError.Type);
    }

    [Fact]
    public void ImplicitCastEmptyErrorArray_ShouldBeError()
    {
        // Arrange
        Result<int> resultInt = Array.Empty<Error>();

        // Act & Assert
        Assert.True(resultInt.IsError);
    }

    [Fact]
    public void ImplicitCastEmptyErrorArray_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        Result<int> resultInt = Array.Empty<Error>();

        // Act & Assert
        Assert.Equal(default, resultInt.Value);
    }

    [Fact]
    public void ImplicitCastEmptyErrorArray_WhenAccessingErrors_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int> resultInt = Array.Empty<Error>();

        // Act
        List<Error> errors = resultInt.Errors;

        // Assert
        var error = Assert.Single(errors);
        Assert.Equal(ErrorType.Unexpected, error.Type);
    }

    [Fact]
    public void ImplicitCastEmptyErrorArray_WhenAccessingErrorsOrEmptyList_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int> resultInt = Array.Empty<Error>();

        // Act
        List<Error> errors = resultInt.ErrorsOrEmptyList;

        // Assert
        var error = Assert.Single(errors);
        Assert.Equal(ErrorType.Unexpected, error.Type);
    }

    [Fact]
    public void ImplicitCastEmptyErrorArray_WhenAccessingFirstError_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int> resultInt = Array.Empty<Error>();

        // Act
        Error firstError = resultInt.FirstError;

        // Assert
        Assert.Equal(ErrorType.Unexpected, firstError.Type);
    }

    [Fact]
    public void ImplicitCastNullValue_WhenAccessingValue_ShouldReturnNull()
    {
        // Arrange
        Result<int?> resultInt = default(int?);

        // Act & Assert
        Assert.False(resultInt.IsError);
        Assert.Null(resultInt.Value);
    }

    [Fact]
    public void ImplicitCastNullObject_WhenAccessingValue_ShouldReturnNull()
    {
        // Arrange
        Result<Person?> result = CreatePersonOrNull(
            firstName: null,
            lastName: null);

        // Act & Assert
        Assert.False(result.IsError);
        Assert.Null(result.Value);
    }

    [Fact]
    public void ImplicitCastNullValue_WhenAccessingErrors_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int?> resultInt = default(int?);

        // Act
        List<Error> errors = resultInt.Errors;

        // Assert
        var error = Assert.Single(errors);
        Assert.Equal(ErrorType.Unexpected, error.Type);
    }

    [Fact]
    public void ImplicitCastNullValue_WhenAccessingErrorsOrEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        Result<int?> resultInt = default(int?);

        // Act
        List<Error> errors = resultInt.ErrorsOrEmptyList;

        // Assert
        Assert.Empty(errors);
    }

    [Fact]
    public void ImplicitCastNullValue_WhenAccessingFirstError_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int?> resultInt = default(int?);

        // Act
        Error firstError = resultInt.FirstError;

        // Assert
        Assert.Equal(ErrorType.Unexpected, firstError.Type);
    }

    [Fact]
    public void ImplicitCastNullErrorList_ShouldBeError()
    {
        // Arrange
        Result<int> resultInt = default(List<Error>)!;

        // Act & Assert
        Assert.True(resultInt.IsError);
    }

    [Fact]
    public void ImplicitCastNullErrorList_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        Result<int> resultInt = default(List<Error>)!;

        // Act & Assert
        Assert.Equal(default, resultInt.Value);
    }

    [Fact]
    public void ImplicitCastNullErrorList_WhenAccessingErrors_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int> resultInt = default(List<Error>)!;

        // Act
        List<Error> errors = resultInt.Errors;

        // Assert
        var error = Assert.Single(errors);
        Assert.Equal(ErrorType.Unexpected, error.Type);
    }

    [Fact]
    public void ImplicitCastNullErrorList_WhenAccessingErrorsOrEmptyList_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int> resultInt = default(List<Error>)!;

        // Act
        List<Error> errors = resultInt.ErrorsOrEmptyList;

        // Assert
        var error = Assert.Single(errors);
        Assert.Equal(ErrorType.Unexpected, error.Type);
    }

    [Fact]
    public void ImplicitCastNullErrorList_WhenAccessingFirstError_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int> resultInt = default(List<Error>)!;

        // Act
        Error firstError = resultInt.FirstError;

        // Assert
        Assert.Equal(ErrorType.Unexpected, firstError.Type);
    }

    [Fact]
    public void ImplicitCastNullErrorArray_ShouldBeError()
    {
        // Arrange
        Result<int> resultInt = default(Error[])!;

        // Act & Assert
        Assert.True(resultInt.IsError);
    }

    [Fact]
    public void ImplicitCastNullErrorArray_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        Result<int> resultInt = default(Error[])!;

        // Act & Assert
        Assert.Equal(default, resultInt.Value);
    }

    [Fact]
    public void ImplicitCastNullErrorArray_WhenAccessingErrors_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int> resultInt = default(Error[])!;

        // Act
        List<Error> errors = resultInt.Errors;

        // Assert
        var error = Assert.Single(errors);
        Assert.Equal(ErrorType.Unexpected, error.Type);
    }

    [Fact]
    public void ImplicitCastNullErrorArray_WhenAccessingErrorsOrEmptyList_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int> resultInt = default(Error[])!;

        // Act
        List<Error> errors = resultInt.ErrorsOrEmptyList;

        // Assert
        var error = Assert.Single(errors);
        Assert.Equal(ErrorType.Unexpected, error.Type);
    }

    [Fact]
    public void ImplicitCastNullErrorArray_WhenAccessingFirstError_ShouldReturnUnexpected()
    {
        // Arrange
        Result<int> resultInt = default(Error[])!;

        // Act
        Error firstError = resultInt.FirstError;

        // Assert
        Assert.Equal(ErrorType.Unexpected, firstError.Type);
    }

    private static Result<Person?> CreatePersonOrNull(string? firstName, string? lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                return (Person?)null;
            }
            else
            {
                return Error.NotFound("Person.FirstNameNotFound", "First name is required.");
            }
        }
        else
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                return Error.NotFound("Person.LastNameNotFound", "Last name is required.");
            }
            else
            {
                return new Person($"{firstName} {lastName}");
            }
        }
    }
}
