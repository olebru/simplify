using Simplify;

var config = Config.FromEnvironment();
var reports = FileScanner
    .Scan(config.Root, config.Paths)
    .Select(AnalyzerRegistry.Analyze)
    .ToList();
var violations = reports
    .Where(r => r.Loc > config.MaxLoc || r.Complexity > config.MaxComplexity)
    .ToList();

var summary = Reporter.BuildSummary(reports, violations, config.MaxLoc, config.MaxComplexity);
WriteStepSummary(summary);
Console.WriteLine(summary);

return (config.FailOnViolation && violations.Count > 0) ? 1 : 0;

static void WriteStepSummary(string content)
{
    var path = Environment.GetEnvironmentVariable("GITHUB_STEP_SUMMARY");
    if (!string.IsNullOrEmpty(path)) File.AppendAllText(path, content);
}
