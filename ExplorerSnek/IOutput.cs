namespace ExplorerSnek;

public interface IOutput
{
    int Width { set; }
    int Height { set; }
    void DrawGame(SnekStuff snek, Point apple);
    void DrawMessage(string message);
    void Init();
}
