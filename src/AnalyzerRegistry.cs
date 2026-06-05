namespace Simplify;

public static class AnalyzerRegistry
{
    public static FileReport Analyze(SourceFile file)
    {
        var loc = CountLoc(file.Source);
        var complexity = file.Language switch
        {
            "csharp" => CSharpAnalyzer.Complexity(file.Source),
            "python" => PythonAnalyzer.Complexity(file.Source),
            _ => JsTsAnalyzer.Complexity(file.Source),
        };
        return new FileReport(file.RelativePath, file.Language, loc, complexity);
    }

    private static int CountLoc(string source) =>
        source.Split('\n').Count(line => !string.IsNullOrWhiteSpace(line));
}
