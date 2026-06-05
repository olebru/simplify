namespace Simplify;

public sealed record Config(
    string Root,
    string[] Paths,
    int MaxLoc,
    int MaxComplexity,
    bool FailOnViolation)
{
    public static Config FromEnvironment()
    {
        var root = Env("GITHUB_WORKSPACE") ?? Directory.GetCurrentDirectory();
        var paths = (Env("INPUT_PATHS") ?? ".")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var maxLoc = int.Parse(Env("INPUT_MAX_LOC") ?? "300");
        var maxCc = int.Parse(Env("INPUT_MAX_COMPLEXITY") ?? "15");
        var fail = (Env("INPUT_FAIL_ON_VIOLATION") ?? "true")
            .Equals("true", StringComparison.OrdinalIgnoreCase);
        return new Config(root, paths, maxLoc, maxCc, fail);
    }

    private static string? Env(string name) => Environment.GetEnvironmentVariable(name);
}
