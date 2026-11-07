namespace Gorin.Result;

internal static class KnownErrors
{
    public static Error NoFirstError { get; } = Error.Unexpected(
        code: "Result.NoFirstError",
        description: "First error cannot be retrieved from a successful Result.");

    public static Error NoErrors { get; } = Error.Unexpected(
        code: "Result.NoErrors",
        description: "Error list cannot be retrieved from a successful Result.");

    public static Error EmptyInitialErrors { get; } = Error.Unexpected(
        code: "Result.EmptyInitialErrors",
        description: "Error list cannot be null or empty when initializing Result.");

    public static List<Error> CachedNoErrorsList { get; } = new (1) { NoErrors };

    public static List<Error> CachedInvalidInitialErrorsList { get; } = new(1) { EmptyInitialErrors };

    public static List<Error> CachedEmptyErrorsList { get; } = new (0);
}
