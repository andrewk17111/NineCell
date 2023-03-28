namespace NineCell;

internal class Board
{
    private readonly int[,] _board;

    public Board()
    {
        _board = InitBoard();
    }

    private static int[,] InitBoard()
        => new int[,]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };

    public int GetCell(int x, int y)
        => _board[x, y];

    public void SetCell(int x, int y, int value)
        => _board[x, y] = value < 0 || value > 9
            ? throw new ArgumentOutOfRangeException(nameof(value), value, $"Cell value must be between 0 and 9")
            : value;

    private bool CheckColumnCondition(int x, int y)
    {
        int cell = GetCell(x, y);

        if (cell == 0)
            return true;

        for (int i = 0;  i < _board.GetLength(1); i++)
            if (_board[x, i] == cell)
                return false;

        return true;
    }

    private bool CheckRowCondition(int x, int y)
    {
        int cell = GetCell(x, y);

        if (cell == 0)
            return true;

        for (int i = 0; i < _board.GetLength(0); i++)
            if (_board[i, y] == cell)
                return false;

        return true;
    }

    private bool CheckHouseCondition(int x, int y)
    {
        int cell = GetCell(x, y);

        if (cell == 0)
            return true;

        for (int j = y / 3 * 3; j < y / 3 * 3 + 1; j++)
            for (int i = x / 3 * 3; i < x / 3 * 3 + 1; i++)
                if (_board[i, j] == cell)
                    return false;

        return true;
    }

    public override string ToString()
    {
        string output = "┌───┬───┬───┐\n│";

        for (int y = 0; y < _board.GetLength(1); y++)
        {
            for (int x = 0; x < _board.GetLength(0); x++)
            {
                output += _board[x, y] == 0
                    ? " "
                    : _board[x, y];

                if (x % 3 == 2)
                {
                    output += "│";

                    if (x == _board.GetLength(0) - 1)
                    {
                        output += "\n";

                        if (y % 3 != 2)
                            output += "│";
                    }
                }
            }

            if (y % 3 == 2 && y != _board.GetLength(1) - 1)
                output += "├───┼───┼───┤\n│";
        }

        output += "└───┴───┴───┘";
        return output;
    }
}
