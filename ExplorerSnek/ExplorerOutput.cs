using System.Text;

namespace ExplorerSnek
{
    public class ExplorerOutput : IOutput
    {
        private const int StartYear = 1990;

        public required int Width { get; set; }

        public required int Height { get; set; }

        public required string GamePath { get; init; }

        public void DrawGame(SnekStuff snek, Point apple)
        {
            var rows = GetRows(snek, apple);
            Reset();
            int year = StartYear;
            foreach (var row in rows)
            {
                CreateDirectory(row, year++);
            }
        }

        private string[] GetRows(SnekStuff snek, Point apple)
        {
            string[] rows = new string[Height];
            var sb = new StringBuilder();
            for (int y = 0; y < Height; y++)
            {
                sb.Clear();
                for (int x = 0; x < Width; x++)
                {
                    sb.Append(GetSymbolToWrite((x, y), snek, apple));
                    sb.Append(' ');
                }
                sb.Append((char)(64 + y));
                rows[y] = sb.ToString();
            }

            return rows;
        }

        private static char GetSymbolToWrite(Point point, SnekStuff snek, Point apple)
        {
            if (point == snek.Head) return '█';
            if (point == apple) return '▓';
            if (snek.Tail.Contains(point)) return '▒';

            return '░';
        }

        public void DrawMessage(string message)
        {
            CreateDirectory(message, StartYear + 1 + Height);
        }

        public void Reset()
        {
            string doNotDelete = Path.Combine(GamePath, "SNEK!");
            foreach (string i in Directory.EnumerateDirectories(GamePath))
            {
                if (i != doNotDelete)
                {
                    Directory.Delete(i);
                }
            }
        }

        public void Init()
        {
            CreateDirectory("SNEK!", StartYear - 1);
            Reset();
        }

        private void CreateDirectory(string name, int year)
        {
            string path = Path.Combine(GamePath, name);
            Directory.CreateDirectory(path);
            Directory.SetCreationTime(path, new DateTime(year, 1, 1));
        }
    }
}
