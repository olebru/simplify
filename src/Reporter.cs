using System.Text;

namespace Simplify;

public static class Reporter
{
    private const int TopHotFiles = 5;

    public static string BuildSummary(
        IReadOnlyList<FileReport> all,
        IReadOnlyList<FileReport> violations,
        int maxLoc,
        int maxCc)
    {
        var sb = new StringBuilder();
        AppendHeader(sb, all, violations, maxLoc, maxCc);
        AppendHotFiles(sb, all, maxLoc);
        sb.Append(QuadrantChart.Render(all, maxLoc, maxCc));
        AppendTable(sb, all, maxLoc, maxCc);
        return sb.ToString();
    }

    private static void AppendHeader(
        StringBuilder sb,
        IReadOnlyList<FileReport> all,
        IReadOnlyList<FileReport> violations,
        int maxLoc,
        int maxCc)
    {
        sb.AppendLine("# Simplify Report");
        sb.AppendLine();
        sb.AppendLine($"- Files analyzed: **{all.Count}**");
        sb.AppendLine($"- Violations: **{violations.Count}**");
        sb.AppendLine($"- Limits: max LOC = {maxLoc}, max complexity = {maxCc}");
        sb.AppendLine();
    }

    private static void AppendHotFiles(StringBuilder sb, IReadOnlyList<FileReport> all, int maxLoc)
    {
        var hot = all
            .OrderByDescending(f => RiskScorer.Score(f, maxLoc))
            .Take(TopHotFiles)
            .ToList();
        if (hot.Count == 0) return;
        sb.AppendLine("## Hot files");
        sb.AppendLine();
        sb.AppendLine($"Top {hot.Count} by risk (CC × max(1, LOC / {maxLoc})):");
        sb.AppendLine();
        var rank = 1;
        foreach (var f in hot)
        {
            var score = RiskScorer.Score(f, maxLoc);
            sb.AppendLine($"{rank}. `{f.RelativePath}` — CC {f.Complexity}, LOC {f.Loc} — risk **{score:F1}**");
            rank++;
        }
        sb.AppendLine();
    }

    private static void AppendTable(
        StringBuilder sb,
        IReadOnlyList<FileReport> all,
        int maxLoc,
        int maxCc)
    {
        sb.AppendLine("## All files");
        sb.AppendLine();
        sb.AppendLine($"| File | Language | LOC (max {maxLoc}) | Complexity (max {maxCc}) |");
        sb.AppendLine("|------|----------|--------------------|--------------------------|");
        var sorted = all.OrderByDescending(f => RiskScorer.Score(f, maxLoc));
        foreach (var f in sorted)
        {
            var loc = BarFormatter.Cell(f.Loc, maxLoc);
            var cc = BarFormatter.Cell(f.Complexity, maxCc);
            sb.AppendLine($"| `{f.RelativePath}` | {f.Language} | {loc} | {cc} |");
        }
    }
}
