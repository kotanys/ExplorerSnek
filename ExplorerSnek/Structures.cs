namespace ExplorerSnek;

public record struct Point(int X, int Y)
{
    public static implicit operator Point((int x, int y) tuple) => new(tuple.x, tuple.y);
    public static Point operator +(Point p1, Point p2) => new(p1.X + p2.X, p1.Y + p2.Y);
    public static Point Random(int maxX, int maxY, IReadOnlyCollection<Point>? exclude = null) 
    {
        Point point = (System.Random.Shared.Next(maxX), System.Random.Shared.Next(maxY));
        if (exclude is not null)
        {
            DoExclusion(maxX, maxY, exclude, ref point);
        }
        return point;
    }

    private static void DoExclusion(int maxX, int maxY, IReadOnlyCollection<Point> exclude, ref Point point)
    {
        if (exclude.Count >= maxX * maxY)
        {
            throw new InvalidOperationException();
        }
        while (exclude.Contains(point))
        {
            if (point.X < maxX - 1)
            {
                point.X++;
            }
            else if (point.Y < maxY - 1)
            {
                point.X = 0;
                point.Y++;
            }
            else
            {
                point.X = 0;
                point.Y = 0;
            }
        }
    }
}

public static class Direction
{
    public static readonly Point Left = (-1, 0);
    public static readonly Point Right = (1, 0);
    public static readonly Point Up = (0, -1);
    public static readonly Point Down = (0, 1);

    public static Point? FromConsoleKey(ConsoleKey key)
    {
        return key switch
        {
            ConsoleKey.LeftArrow => Left,
            ConsoleKey.UpArrow => Up,
            ConsoleKey.RightArrow => Right,
            ConsoleKey.DownArrow => Down,
            _ => null
        };
    }

    public static bool AreOpposites(Point p1, Point p2)
    {
        return p1.X + p2.X == 0 || p1.Y + p2.Y == 0;
    }
}

public class GameEndedException : Exception
{
    public GameEndedException(string reason) : base(reason) { }
}

public class Messages
{
    public string Upper { get; init; } = "";
    public string Death { get; init; } = "";
    public string Win { get; init; } = "";
}

public class SnekStuff
{
    public Point Head { get; set; }
    public Queue<Point> Tail { get; init; } = new();
    public Point Direction { get; set; }

    public void Move() => Head += Direction;
}