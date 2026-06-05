namespace Simplify;

public sealed record FileReport(string RelativePath, string Language, int Loc, int Complexity);

public sealed record SourceFile(string RelativePath, string Language, string Source);
