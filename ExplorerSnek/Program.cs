using ExplorerSnek;

var (width, height, framePeriod) = ParseArgs(args);
var messages = new Messages
{
    Upper = "SNEK!",
    Death = "Snek ded...",
    Win = "Snek big!"
};
var snek = InitializeSnek();
Point apple = Point.Random(width, height, exclude: snek.Tail);
IOutput output = new ExplorerOutput
{
    Height = height,
    Width = width,
    GamePath = @"d:\gametest\"
};

output.Init();
GameEndedException gameEndedReason;
while (true)
{
    Thread.Sleep(framePeriod);
    try
    {
        if (Console.KeyAvailable)
        {
            var newDirection = Direction.FromConsoleKey(Console.ReadKey(true).Key);
            if (newDirection.HasValue
                && !Direction.AreOpposites(snek.Direction, newDirection.Value))
                snek.Direction = newDirection.Value;

        }
        Update();
    }
    catch (GameEndedException e)
    {
        gameEndedReason = e;
        break;
    }
    finally
    {
        output.DrawGame(snek, apple);
    }
}
output.DrawMessage(gameEndedReason.Message);

void Update()
{
    snek.Move();
    if (snek.Head == apple)
    {
        try
        {
            apple = Point.Random(width, height, exclude: snek.Tail);
        }
        catch (InvalidOperationException)
        {
            throw new GameEndedException(messages.Win);
        }
    }
    else snek.Tail.Dequeue();

    if (snek.Tail.Count >= width * height)
        throw new GameEndedException(messages.Win);
    if (IsSnekDead())
        throw new GameEndedException(messages.Death);

    snek.Tail.Enqueue(snek.Head);
}

bool IsSnekDead()
{
    return snek.Head.X >= width || snek.Head.X < 0
        || snek.Head.Y >= height || snek.Head.Y < 0
        || snek.Tail.Contains(snek.Head);
}

static SnekStuff InitializeSnek()
{
    var tail = new Queue<Point>();
    tail.Enqueue((-1, 0));
    tail.Enqueue((0, 0));
    Point head = (0, 0);
    return new SnekStuff
    {
        Head = head,
        Tail = tail,
        Direction = Direction.Right
    };
}

static (int width, int height, int movementTimeout) ParseArgs(string[] args)
{
    (int width, int height, int movementTimeout) parsed = (10, 8, 1000);
    for (int i = 0; i < args.Length; i++)
    {
        var arg = args[i];
        if (arg.Length == 1) continue;
        switch (arg[0])
        {
            case 'w':
                parsed.width = int.Parse(arg[1..]);
                break;
            case 'h':
                parsed.height = int.Parse(arg[1..]);
                break;
            case 's':
                parsed.movementTimeout = (int)((1 / float.Parse(arg[1..]) * 1000));
                break;
        }
    }
    return parsed;
}