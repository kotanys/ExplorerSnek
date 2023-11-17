namespace ExplorerSnek;

public class ConsoleOutput : IOutput
{
    private Point _writeGameHere;

    public required Messages Messages { get; init; }

    public required int Width { get; set; }

    public required int Height { get; set; }

    public void Reset()
    {
        Console.Clear();
        Console.WriteLine(GetPaddedMessage(Messages.Upper, Width));
        _writeGameHere = Console.GetCursorPosition();
    }

    public void DrawGame(SnekStuff snek, Point apple)
    {
        Console.SetCursorPosition(_writeGameHere.X, _writeGameHere.Y);
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Console.Write(GetSymbolToWrite((x, y), snek, apple));
                Console.Write(' ');
            }
            Console.WriteLine();
        }
    }

    private static char GetSymbolToWrite(Point point, SnekStuff snek, Point apple)
    {
        if (point == snek.Head) return '0';
        if (point == apple) return 'a';
        if (snek.Tail.Contains(point)) return '#';
        return '.';
    }

    private static string GetPaddedMessage(string initial, int width)
    {
        return new string(' ', Math.Max(width - initial.Length / 2, 0)) + initial;
    }

    public void DrawMessage(string message)
    {
        Console.WriteLine(GetPaddedMessage(message, Width));
    }

    public void Init() => Reset();
}
