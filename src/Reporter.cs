using System.Text;

namespace Simplify;

public static class Reporter
{
    public static string BuildSummary(
        IReadOnlyList<FileReport> all,
        IReadOnlyList<FileReport> violations,
        int maxLoc,
        int maxCc)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Simplify Report");
        sb.AppendLine();
        sb.AppendLine($"- Files analyzed: **{all.Count}**");
        sb.AppendLine($"- Violations: **{violations.Count}**");
        sb.AppendLine($"- Limits: max LOC = {maxLoc}, max complexity = {maxCc}");
        sb.AppendLine();

        if (violations.Count == 0)
        {
            sb.AppendLine("No violations.");
            return sb.ToString();
        }

        sb.AppendLine("| File | Language | LOC | Complexity |");
        sb.AppendLine("|------|----------|----:|-----------:|");
        var sorted = violations
            .OrderByDescending(v => v.Complexity)
            .ThenByDescending(v => v.Loc);
        foreach (var v in sorted)
            sb.AppendLine(FormatRow(v, maxLoc, maxCc));
        return sb.ToString();
    }

    private static string FormatRow(FileReport v, int maxLoc, int maxCc)
    {
        var loc = v.Loc > maxLoc ? $"**{v.Loc}**" : v.Loc.ToString();
        var cc = v.Complexity > maxCc ? $"**{v.Complexity}**" : v.Complexity.ToString();
        return $"| `{v.RelativePath}` | {v.Language} | {loc} | {cc} |";
    }
}
