namespace NineCell;

internal class Board
{
    private readonly Cell[,] _board;

    public Cell this[int x, int y] {
        get => _board[x, y];
    }

    /// <summary>
    /// Creates a new sudoku Board.
    /// </summary>
    public Board()
    {
        _board = InitBoard();
    }

    /// <summary>
    /// Initializes a new sudoku board.
    /// </summary>
    /// <returns>An int[,] that represents the sudoku board.</returns>
    private Cell[,] InitBoard()
    {
        Cell[,] result = new Cell[Utils.SIZE, Utils.SIZE];

        for (int y = 0; y < Utils.SIZE; y++)
            for (int x = 0; x < Utils.SIZE; x++)
                result[x, y] = new Cell(x, y, this);

        return result;
    }

    public bool UpdateNotes()
    {
        bool updated = false;

        for (int y = 0; y < Utils.SIZE; y++)
            for (int x = 0; x < Utils.SIZE; x++)
                updated = _board[x, y].UpdateNotes() ? true : updated;

        return updated;
    }

    public bool UpdateValues()
    {
        bool updated = false;

        foreach (Cell cell in _board)
        {
            if (cell.Value == 0 && cell.Notes.Length == 1)
            {
                cell.Value = cell.Notes[0];
                updated = true;
            }
        }

        return updated;
    }

    private Cell[]? GetRow(int index)
    {
        if (index >= 0 && index < Utils.SIZE) {
            List<Cell> result = new List<Cell>();

            for (int x = 0; x < Utils.SIZE; x++)
                result.Add(_board[x, index]);

            return result.ToArray();
        }
        else
        {
            return null;
        }
    }

    private Cell[]? GetColumn(int index)
    {
        if (index >= 0 && index < Utils.SIZE)
        {
            List<Cell> result = new List<Cell>();

            for (int y = 0; y < Utils.SIZE; y++)
                result.Add(_board[index, y]);

            return result.ToArray();
        }
        else
        {
            return null;
        }
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
