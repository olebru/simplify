namespace Simplify;

public static class BarFormatter
{
    private const int Width = 10;
    private const char Filled = '█';
    private const char Empty = '░';

    public static string Cell(int value, int threshold)
    {
        var ratio = threshold <= 0 ? 0.0 : (double)value / threshold;
        var fill = Math.Clamp((int)Math.Round(ratio * Width), 0, Width);
        var bar = new string(Filled, fill) + new string(Empty, Width - fill);
        var num = value > threshold ? $"**{value}**" : value.ToString();
        return $"{num} `{bar}`";
    }
}
