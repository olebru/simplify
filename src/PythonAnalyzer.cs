using System.Text.RegularExpressions;

namespace Simplify;

public static partial class PythonAnalyzer
{
    private static readonly string[] Keywords =
        { "if", "elif", "for", "while", "except", "and", "or" };

    [GeneratedRegex(
        @"#[^\r\n]*|""""""[\s\S]*?""""""|'''[\s\S]*?'''|""(?:\\.|[^""\\\n])*""|'(?:\\.|[^'\\\n])*'")]
    private static partial Regex StripperRegex();

    public static int Complexity(string source)
    {
        var stripped = StripperRegex().Replace(source, "");
        var count = 0;
        foreach (var keyword in Keywords)
            count += Regex.Matches(stripped, $@"\b{keyword}\b").Count;
        return count + 1;
    }
}
