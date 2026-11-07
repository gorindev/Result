# Gorin.Result

A lightweight, zero-dependency .NET library for modeling success and failure without exceptions. Use `Result<T>` to represent outcomes, compose pipelines with `Then`, branch with `Match`/`Switch`, and carry structured errors through your application layers.

```bash
dotnet add package Gorin.Result
```

**Target framework:** .NET 10.0 · **Dependencies:** none

## Quick start

```csharp
using Gorin.Result;
using Gorin.Result.Results;

public Result<User> CreateUser(string name, string email)
{
    if (string.IsNullOrWhiteSpace(name))
        return Error.Validation("User.Name", "Name is required.");

    if (!email.Contains('@'))
        return Error.Validation("User.Email", "Email is invalid.");

    return new User(name, email);
}

// Errors short-circuit the pipeline automatically
Result<string> greeting = CreateUser("Alice", "alice@example.com")
    .Then(user => $"Hello, {user.Name}!")
    .FailIf(msg => msg.Length > 100, Error.Validation("Greeting.TooLong", "Greeting is too long."));

greeting.Match(
    onValue: msg => Console.WriteLine(msg),
    onError: errors => Console.WriteLine(errors[0].Description));
```

## Core types

### `Result<TValue>`

Either a success value or a list of errors.

```csharp
Result<int> success = 42;
Result<int> failure = Error.NotFound("Order.NotFound", "Order was not found.");

success.IsSuccess;  // true
failure.IsError;    // true
failure.FirstError; // first Error in the list
```

| Member                  | Description                                                |
| ----------------------- | ---------------------------------------------------------- |
| `IsSuccess` / `IsError` | State checks with nullable flow-analysis attributes        |
| `Value`                 | The success value (default on error)                       |
| `Errors`                | Error list; sentinel "no errors" entry on success          |
| `ErrorsOrEmptyList`     | Error list; empty list on success                          |
| `FirstError`            | First error; sentinel error on success                     |

### `Error`

A readonly record struct with `Code`, `Description`, `Type` (`ErrorType`), `NumericType` and optional `Metadata`.

```csharp
Error.Validation("User.Email", "Invalid email format.");
Error.NotFound("Order.123", "Order not found.");
Error.Forbidden("User.Banned", "Account is suspended.");
Error.NotImplemented("Export.Pdf", "PDF export is not available yet.");
Error.ServiceUnavailable("Payments.Gateway", "Payment provider is unreachable.");
Error.Unauthorized();
Error.Conflict();
Error.Failure();
Error.Unexpected();
Error.Custom(42, "Custom.Code", "A custom error type.");
```

`ErrorType` values: `Failure`, `Unexpected`, `Validation`, `Conflict`, `NotFound`, `Unauthorized`, `Forbidden`, `NotImplemented`, `ServiceUnavailable`.

### Semantic success types

For operations with no payload:

```csharp
Result<Success> ok = ResultType.Success;
Result<Created> created = ResultType.Created;
Result<Updated> updated = ResultType.Updated;
Result<Deleted> deleted = ResultType.Deleted;
```

## Creating results

```csharp
// Implicit conversion
Result<string> fromValue = "hello";
Result<string> fromError = Error.Validation("Code", "Description");
Result<string> fromErrors = new List<Error> { Error.Validation(), Error.Conflict() };

// Collection expressions
Result<User> errors = [Error.Validation("User.Name", "Name is required."), Error.Validation("User.Email", "Email is required.")];

// Factory and extension methods
Result<User> r1 = ResultFactory.From(user);
Task<Result<User>> r2 = ResultFactory.FromAsync(user);
Result<int> r3 = 42.ToResult();
Result<int> r4 = Error.Validation().ToResult<int>();
```

## Pipeline operations

### `Then` — transform on success

On error, the original errors are returned and later steps are skipped.

```csharp
Result<string> result = ResultFactory.From("42")
    .Then(int.Parse)
    .Then(n => n * 2)
    .Then(n => n.ToString());
```

| Method                           | Purpose                                   |
| -------------------------------- | ----------------------------------------- |
| `Then(Func<T, Result<TNext>>)`   | Map to another `Result`                   |
| `Then(Func<T, TNext>)`           | Map to a plain value                      |
| `ThenDo(Action<T>)`              | Side effect; returns the same result      |
| `ThenEnsure(Func<T, Result<T>>)` | Validate; keep the original value         |
| `ThenAsync` / `ThenEnsureAsync`  | Async variants                            |

### `FailIf` — conditional failure

```csharp
result.FailIf(v => v < 0, Error.Validation("Value.Negative", "Value must be positive."));
result.FailIf(v => v > 100, v => Error.Validation("Value.TooLarge", $"Value {v} exceeds limit."));
```

### `Else` — recover from errors

```csharp
result.Else(Error.Unexpected("Fallback", "Using default."));
result.Else(errors => defaultValue);
result.Else(errors => RetryOperation());
result.ElseDo(errors => _logger.LogErrors(errors));
```

### `Match` / `Switch` — branch on outcome

```csharp
var response = result.Match(
    onValue: user => new OkResponse(user),
    onError: errors => new BadRequestResponse(errors));

var message = result.MatchFirst(
    onValue: _ => "OK",
    onFirstError: e => e.Description);

result.Switch(
    onValue: user => SendWelcomeEmail(user),
    onError: errors => LogErrors(errors));
```

Async variants (`MatchAsync`, `SwitchAsync`, `MatchFirstAsync`, `SwitchFirstAsync`) exist on both `Result<T>` and `Task<Result<T>>`.

## Async pipelines

All pipeline methods have `Task<Result<T>>` overloads, so you can chain without awaiting each step:

```csharp
Result<Order> result = await GetCustomerAsync(id)
    .ThenAsync(customer => ValidateCustomerAsync(customer))
    .ThenAsync(customer => CreateOrderAsync(customer))
    .FailIfAsync(order => order.Total <= 0, Error.Validation("Order.Total", "Total must be positive."))
    .ThenEnsureAsync(order => CheckInventoryAsync(order));
```

## Recording and logging

`IRecordable` lets you log or audit a result without knowing its value type (e.g. in middleware).

```csharp
public class JsonRecordingSerializer : IRecordingSerializer<string>
{
    public string SerializeValue<TValue>(TValue value) => JsonSerializer.Serialize(value);
    public string SerializeErrors(List<Error> errors) => JsonSerializer.Serialize(errors);
}

IResult result = await SomeService.ExecuteAsync();
var logEntry = result.GetRecording(new JsonRecordingSerializer());
```

## Mapping errors to HTTP

```csharp
return result.Match(
    onValue: data => Results.Ok(data),
    onError: errors => errors[0].Type switch
    {
        ErrorType.Validation   => Results.BadRequest(errors),
        ErrorType.NotFound     => Results.NotFound(errors[0].Description),
        ErrorType.Unauthorized => Results.Unauthorized(),
        ErrorType.Forbidden    => Results.Forbid(),
        ErrorType.Conflict     => Results.Conflict(errors[0].Description),
        ErrorType.NotImplemented     => Results.Problem(errors[0].Description, statusCode: 501),
        ErrorType.ServiceUnavailable => Results.Problem(errors[0].Description, statusCode: 503),
        _                      => Results.Problem(errors[0].Description)
    });
```

## Design notes

- **Error propagation:** once in an error state, `Then`, `FailIf` and `ThenEnsure` return the original errors without running their delegates.
- **Errors on success:** `Errors` and `FirstError` never throw; they return sentinel errors. Use `ErrorsOrEmptyList` for an empty list instead.
- **Empty error lists:** creating a result from a null or empty error collection yields an `EmptyInitialErrors` sentinel, so an error result always has at least one error.
- **Equality:** `Result<T>` and `Error` use value equality; error list comparison is order-sensitive.

## Credits

Heavily inspired by the API design of [ErrorOr](https://github.com/error-or/error-or).

## License

[MIT](https://github.com/gorindev/Result/blob/main/LICENSE.md)
