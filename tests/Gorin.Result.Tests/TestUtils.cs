using Gorin.Result.Results;

namespace Gorin.Result.Tests;

public static class Convert
{
    public static Result<string> ToString(int num) => num.ToString();

    public static Result<int> ToInt(string str) => int.Parse(str);

    public static Task<Result<int>> ToIntAsync(string str) => ResultFactory.FromAsync(int.Parse(str));

    public static Task<Result<string>> ToStringAsync(int num) => ResultFactory.FromAsync(num.ToString());
}
