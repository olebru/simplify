using System.Text.RegularExpressions;

namespace Simplify;

public static partial class JsTsAnalyzer
{
    private static readonly string[] Keywords = { "if", "for", "while", "case", "catch" };

    [GeneratedRegex(
        @"//[^\r\n]*|/\*[\s\S]*?\*/|""(?:\\.|[^""\\\n])*""|'(?:\\.|[^'\\\n])*'|`(?:\\.|[^`\\])*`")]
    private static partial Regex StripperRegex();

    [GeneratedRegex(@"&&|\|\||(?<!\?)\?(?![?.:])")]
    private static partial Regex OperatorsRegex();

    public static int Complexity(string source)
    {
        var stripped = StripperRegex().Replace(source, "");
        var count = OperatorsRegex().Matches(stripped).Count;
        foreach (var keyword in Keywords)
            count += Regex.Matches(stripped, $@"\b{keyword}\b").Count;
        return count + 1;
    }
}
