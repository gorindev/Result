# Result

A lightweight, zero-dependency .NET library for modeling success and failure without exceptions. Use `Result<T>` to represent operation outcomes, compose pipelines with `Then`, branch with `Match`/`Switch`, and carry structured, typed errors through your application layers.

## Installation

```bash
dotnet add package Gorin.Result
```

**Target framework:** .NET 10.0
**Dependencies:** None

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

// Compose a pipeline — errors short-circuit automatically
Result<string> greeting = CreateUser("Alice", "alice@example.com")
    .Then(user => $"Hello, {user.Name}!")
    .FailIf(msg => msg.Length > 100, Error.Validation("Greeting.TooLong", "Greeting is too long."));

// Branch on outcome
greeting.Match(
    onValue: msg => Console.WriteLine(msg),
    onError: errors => Console.WriteLine(errors[0].Description));
```

## Why use this library?

| Benefit                     | Description                                                             |
| --------------------------- | ----------------------------------------------------------------------- |
| **Explicit error handling** | Failures are values, not hidden control flow via exceptions             |
| **Composable pipelines**    | Chain operations with `Then`; errors propagate without boilerplate      |
| **Structured errors**       | Each error has a code, description, type, and optional metadata         |
| **Full async support**      | Extension methods on `Task<Result<T>>` keep pipelines fluent            |
| **Zero dependencies**       | No external packages; small footprint for any .NET project              |
| **Logging-friendly**        | `IRecordable` lets you serialize results without knowing the value type |

## Core types

### `Result<TValue>`

A readonly discriminated union: either a success value or one or more errors.

```csharp
Result<int> success = 42;
Result<int> failure = Error.NotFound("Order.NotFound", "Order was not found.");

success.IsSuccess;  // true
failure.IsError;    // true
failure.FirstError; // the first Error in the list
```

Key members:

| Member                  | Description                                                  |
| ----------------------- | ------------------------------------------------------------ |
| `IsSuccess` / `IsError` | State checks with nullable flow-analysis attributes          |
| `Value`                 | The success value (default if accessed on an error result)   |
| `Errors`                | Error list; on success, returns a sentinel "no errors" entry |
| `ErrorsOrEmptyList`     | Error list; on success, returns an empty list                |
| `FirstError`            | First error; on success, returns a sentinel error            |

### `Error`

A readonly record struct representing a single failure.

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

Each error exposes:

| Property      | Description                                               |
| ------------- | --------------------------------------------------------- |
| `Code`        | Unique, machine-readable identifier (e.g. `"User.Email"`) |
| `Description` | Human-readable message                                    |
| `Type`        | `ErrorType` enum value                                    |
| `NumericType` | Integer representation of `Type`                          |
| `Metadata`    | Optional `Dictionary<string, object>` for extra context   |

### `ErrorType`

| Value                | Typical use                            |
| -------------------- | -------------------------------------- |
| `Failure`            | General business-rule failure          |
| `Unexpected`         | Unhandled or system-level failure      |
| `Validation`         | Input or state validation failure      |
| `Conflict`           | Resource conflict (e.g. duplicate key) |
| `NotFound`           | Resource does not exist                |
| `Unauthorized`       | Caller is not authenticated            |
| `Forbidden`          | Caller lacks permission                |
| `NotImplemented`     | Feature or operation not implemented   |
| `ServiceUnavailable` | Required service is unavailable        |

### Semantic success types

For operations that only need to signal outcome (no payload), use the built-in marker types:

```csharp
Result<Success> ok = ResultType.Success;
Result<Created> created = ResultType.Created;
Result<Updated> updated = ResultType.Updated;
Result<Deleted> deleted = ResultType.Deleted;
```

## Creating results

### Implicit conversion (recommended for brevity)

```csharp
Result<string> fromValue = "hello";
Result<string> fromError = Error.Validation("Code", "Description");
Result<string> fromErrors = new List<Error> { Error.Validation(), Error.Conflict() };
```

### Factory methods

```csharp
Result<User> r1 = ResultFactory.From(user);
Result<User> r2 = ResultFactory.From(Error.NotFound());
Result<User> r3 = ResultFactory.From(new List<Error> { ... });

// Async wrappers
Task<Result<User>> r4 = ResultFactory.FromAsync(user);
```

### Extension methods

```csharp
Result<int> r = 42.ToResult();
Result<int> r2 = Error.Validation().ToResult<int>();
```

### Collection expressions

Build error results from C# collection expressions:

```csharp
Result<User> validationErrors =
[
    Error.Validation("User.Name", "Name is required."),
    Error.Validation("User.Email", "Email is required.")
];
```

## Pipeline operations

### `Then` — transform on success

Executes the next step only when the current result is successful. On error, the original errors are returned and subsequent steps are skipped.

```csharp
Result<int> result = ResultFactory.From("42")
    .Then(int.Parse)           // Result<int>
    .Then(n => n * 2)          // Result<int>
    .Then(n => n.ToString());  // Result<string>
```

| Method                           | Purpose                                        |
| -------------------------------- | ---------------------------------------------- |
| `Then(Func<T, Result<TNext>>)`   | Map to another `Result`                        |
| `Then(Func<T, TNext>)`           | Map to a plain value (wrapped in success)      |
| `ThenDo(Action<T>)`              | Side effect; returns the same result           |
| `ThenAsync(...)`                 | Async variants of the above                    |
| `ThenEnsure(Func<T, Result<T>>)` | Validate; return errors or keep original value |
| `ThenEnsureAsync(...)`           | Async validation variant                       |

### `FailIf` — conditional failure

Convert a success into an error when a predicate is true.

```csharp
result.FailIf(value => value < 0, Error.Validation("Value.Negative", "Value must be positive."));
result.FailIf(value => value > 100, v => Error.Validation("Value.TooLarge", $"Value {v} exceeds limit."));
```

### `Else` — recover from errors

Handle or replace errors while preserving success values.

```csharp
result.Else(Error.Unexpected("Fallback", "Using default."))
result.Else(errors => defaultValue)
result.Else(errors => RetryOperation())
result.ElseDo(errors => _logger.LogErrors(errors))
```

### `Match` / `Switch` — branch on outcome

**`Match`** returns a value from one of two functions:

```csharp
var response = result.Match(
    onValue: user => new OkResponse(user),
    onError: errors => new BadRequestResponse(errors));

// Operate on only the first error
var message = result.MatchFirst(
    onValue: _ => "OK",
    onFirstError: e => e.Description);
```

**`Switch`** executes an action (no return value):

```csharp
result.Switch(
    onValue: user => SendWelcomeEmail(user),
    onError: errors => LogErrors(errors));
```

Async variants (`MatchAsync`, `SwitchAsync`, `MatchFirstAsync`, `SwitchFirstAsync`) are available on both `Result<T>` and `Task<Result<T>>`.

## Async pipelines

All pipeline methods have `Task<Result<T>>` extension overloads, so you can chain without manual `await` at every step:

```csharp
Result<Order> result = await GetCustomerAsync(id)
    .ThenAsync(customer => ValidateCustomerAsync(customer))
    .ThenAsync(customer => CreateOrderAsync(customer))
    .FailIfAsync(order => order.Total <= 0, Error.Validation("Order.Total", "Total must be positive."))
    .ThenEnsureAsync(order => CheckInventoryAsync(order));
```

## Recording and logging

Use `IRecordable` when you need to log or audit a result without knowing its value type at compile time (e.g. in middleware or cross-cutting infrastructure).

```csharp
public interface IRecordable
{
    TOutput GetRecording<TOutput>(IRecordingSerializer<TOutput> serializer);
}

public interface IRecordingSerializer<TOutput>
{
    TOutput SerializeValue<TValue>(TValue value);
    TOutput SerializeErrors(List<Error> errors);
}
```

Example with System.Text.Json:

```csharp
public class JsonRecordingSerializer : IRecordingSerializer<string>
{
    public string SerializeValue<TValue>(TValue value) =>
        JsonSerializer.Serialize(value);

    public string SerializeErrors(List<Error> errors) =>
        JsonSerializer.Serialize(errors);
}

// Works with IResult when TValue is unknown
IResult result = await SomeService.ExecuteAsync();
var logEntry = result.GetRecording(new JsonRecordingSerializer());
```

## Interfaces

| Interface         | Purpose                                                  |
| ----------------- | -------------------------------------------------------- |
| `IResult`         | Type-less access to `IsSuccess`, `IsError`, and `Errors` |
| `IResult<TValue>` | Typed access adding `Value`                              |
| `IRecordable`     | Serialize success or error state via a custom serializer |

## Namespaces

| Namespace               | Contains                                                                                              |
| ----------------------- | ----------------------------------------------------------------------------------------------------- |
| `Gorin.Result`         | `Error`, `ErrorType`, `ResultFactory`, `ResultType`, `IResult`, `IRecordable`, `IRecordingSerializer` |
| `Gorin.Result.Results` | `Result<TValue>`, `ResultExtensions`                                                                  |

## Design notes

**Error propagation.** Once a `Result<T>` is in an error state, all subsequent `Then`, `FailIf`, and `ThenEnsure` calls return the original errors without executing their delegates.

**Accessing errors on success.** Calling `Errors` or `FirstError` on a successful result does not throw. Instead, it returns sentinel errors (`Result.NoErrors`, `Result.NoFirstError`) so callers always get a consistent API. Use `ErrorsOrEmptyList` when you need an empty list on success.

**Empty error lists.** Creating a result from a null or empty error collection produces an `EmptyInitialErrors` sentinel — an error result is always created with at least one error.

**Equality.** `Result<T>` and `Error` implement value equality. Error list comparison is order-sensitive.

**No exceptions for flow control.** This library does not throw for business failures. Use `Match`/`Switch` at boundaries (API controllers, UI layers) to translate errors into HTTP status codes, messages, or other representations.

## Mapping errors to HTTP (example)

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
        ErrorType.NotImplemented => Results.Problem(errors[0].Description, statusCode: 501),
        ErrorType.ServiceUnavailable => Results.Problem(errors[0].Description, statusCode: 503),
        _                      => Results.Problem(errors[0].Description)
    });
```

## Prior Art & Credit

This library was built from the ground up, heavily inspired by the API design and functional patterns of **[ErrorOr](https://github.com/error-or/error-or)**.

## License

This project is licensed under the terms of the [MIT](./LICENSE.md) license.
