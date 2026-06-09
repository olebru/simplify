using System.Text;

namespace Simplify;

public static class QuadrantChart
{
    private const int MaxPoints = 15;
    private const double Cap = 0.95;

    public static string Render(IReadOnlyList<FileReport> files, int maxLoc, int maxCc)
    {
        var picked = files
            .OrderByDescending(f => RiskScorer.Score(f, maxLoc))
            .Take(MaxPoints)
            .ToList();
        if (picked.Count == 0) return string.Empty;

        var sb = new StringBuilder();
        sb.AppendLine("```mermaid");
        sb.AppendLine("quadrantChart");
        sb.AppendLine("  title Complexity vs Lines of Code");
        sb.AppendLine($"  x-axis Low CC --> CC {2 * maxCc}+");
        sb.AppendLine($"  y-axis Few LOC --> LOC {2 * maxLoc}+");
        sb.AppendLine("  quadrant-1 Big and complex");
        sb.AppendLine("  quadrant-2 Big but simple");
        sb.AppendLine("  quadrant-3 Small and simple");
        sb.AppendLine("  quadrant-4 Small but knotty");
        foreach (var f in picked) sb.AppendLine(Point(f, maxLoc, maxCc));
        sb.AppendLine("```");
        sb.AppendLine();
        return sb.ToString();
    }

    private static string Point(FileReport f, int maxLoc, int maxCc)
    {
        var x = Math.Min(Cap, (double)f.Complexity / (2.0 * maxCc));
        var y = Math.Min(Cap, (double)f.Loc / (2.0 * maxLoc));
        var name = Path.GetFileName(f.RelativePath);
        return $"  \"{name}\": [{x:F2}, {y:F2}]";
    }
}
