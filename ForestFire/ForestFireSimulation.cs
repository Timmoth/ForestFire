namespace Timmoth;

public class ForestFireSimulation
{
    public int Width { get; }
    public int Height { get; }
    public int[,] Cells { get; private set; }
    public int F { get; set; } = 5000;
    public int P { get; set; } = 40;

    private const int _minTreeAge = 1;
    private const int _maxTreeAge = 3;
    private const int _minFireAge = 4;
    private const int _maxFireAge = 5;

    private readonly Random _rnd = new();
    public ForestFireSimulation(int width, int height)
    {
        Width = width;
        Height = height;
        Cells = new int[Width, Height];
        Reset();
    }

    public void Reset()
    {
        Cells = new int[Width, Height];
        for (var i = 0; i < Width; i++)
        {
            for (var j = 0; j < Height; j++)
            {
                Cells[i, j] = 0;
            }
        }
    }

    public void Next()
    {
        var cells = new int[Width, Height];

        for (var x = 1; x < Width - 1; x++)
        {
            for (var y = 1; y < Height - 1; y++)
            {
                var value = Cells[x, y];
                switch (value)
                {
                    case 0:
                        // Tree grows
                        cells[x, y] = (_rnd.Next(0, P) == 0) ? 1 : 0;
                        break;
                    case >= _minTreeAge and <= _maxTreeAge:
                        if (_rnd.Next(0, F) == 0)
                        {
                            // Forrest fire starts
                            cells[x, y] = _minFireAge;
                            break;
                        }

                        var treeAge = value < _maxTreeAge ? value + 1 : _maxTreeAge;
                        if (IsNextToFire(x, y))
                        {
                            // Tree catches fire
                            cells[x, y] = _rnd.Next(0, 1) == 0 ? _minFireAge : treeAge;
                        }
                        else
                        {
                            // Tree survives
                            cells[x, y] = treeAge;
                        }
                        break;
                    case >= _minFireAge:
                        if (value >= _maxFireAge)
                        {
                            // Fire burnt out
                            cells[x, y] = 0;
                        }
                        else
                        {
                            // Did fire burn out early?
                            cells[x, y] = _rnd.Next(0, 1) == 0 ? 0 : value + 1;
                        }

                        break;
                }
            }
        }

        Cells = cells;
    }
    
    private bool IsNextToFire(int x, int y)
    {
        for (var r = -1; r <= 1; r++)
        {
            for (var c = -1; c <= 1; c++)
            {
                if (r == 0 && c == 0)
                    continue;

                if (Cells[x + r, y + c] >= _minFireAge)
                    return true;
            }
        }

        return false;
    }
}