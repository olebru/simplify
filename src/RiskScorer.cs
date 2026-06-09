namespace Simplify;

public static class RiskScorer
{
    public static double Score(FileReport file, int maxLoc) =>
        file.Complexity * Math.Max(1.0, (double)file.Loc / maxLoc);
}
