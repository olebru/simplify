namespace Simplify;

public static class FileScanner
{
    private static readonly HashSet<string> SkipDirs = new(StringComparer.OrdinalIgnoreCase)
    {
        ".git", "node_modules", "bin", "obj", "dist", "build",
        ".venv", "venv", "__pycache__", ".next", ".nuxt", "out",
    };

    private static readonly Dictionary<string, string> Languages = new(StringComparer.OrdinalIgnoreCase)
    {
        [".cs"] = "csharp",
        [".ts"] = "typescript",
        [".tsx"] = "typescript",
        [".js"] = "javascript",
        [".jsx"] = "javascript",
        [".mjs"] = "javascript",
        [".cjs"] = "javascript",
        [".py"] = "python",
    };

    public static IEnumerable<SourceFile> Scan(string root, IEnumerable<string> paths)
    {
        foreach (var rel in paths)
        {
            var full = Path.GetFullPath(Path.Combine(root, rel));
            if (!Directory.Exists(full)) continue;
            foreach (var file in Walk(full))
            {
                if (!Languages.TryGetValue(Path.GetExtension(file), out var lang)) continue;
                var relPath = Path.GetRelativePath(root, file);
                yield return new SourceFile(relPath, lang, File.ReadAllText(file));
            }
        }
    }

    private static IEnumerable<string> Walk(string dir)
    {
        var stack = new Stack<string>();
        stack.Push(dir);
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            foreach (var sub in Directory.EnumerateDirectories(current))
                if (!SkipDirs.Contains(Path.GetFileName(sub))) stack.Push(sub);
            foreach (var f in Directory.EnumerateFiles(current))
                yield return f;
        }
    }
}
