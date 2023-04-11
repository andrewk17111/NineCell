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

    /// <summary>
    /// Checks whether a value of a cell is valid.
    /// </summary>
    /// <param name="x">The row of the cell.</param>
    /// <param name="y">The column of the cell.</param>
    /// <returns><c>true</c> if the cell value is valid; <c>false</c> otherwise.</returns>
    public bool IsValid(int x, int y)
        => IsValid(x, y, this[x, y]);

    /// <summary>
    /// Checks whether a value at a cell is valid.
    /// </summary>
    /// <param name="x">The row of the cell.</param>
    /// <param name="y">The column of the cell.</param>
    /// <param name="value">The value to check.</param>
    /// <returns><c>true</c> if the cell value is valid; <c>false</c> otherwise.</returns>
    public bool IsValid(int x, int y, byte value)
        => CheckColumnCondition(x, y, value) && CheckRowCondition(x, y, value) && CheckHouseCondition(x, y, value);

    /// <summary>
    /// Checks whether a particular value at a cell is unique within a column.
    /// </summary>
    /// <param name="x">The row of the cell.</param>
    /// <param name="y">The column of the cell.</param>
    /// <param name="value">The value to check.</param>
    /// <returns><c>true</c> if the cell value is valid; <c>false</c> otherwise.</returns>
    private bool CheckColumnCondition(int x, int y, byte value)
    {
        if (value == 0)
            return true;

        for (int i = 0; i < _board.GetLength(1); i++)
            if (i != y && _board[x, i] == value)
                return false;

        return true;
    }

    /// <summary>
    /// Checks whether a particular value at a cell is unique within a row.
    /// </summary>
    /// <param name="x">The row of the cell.</param>
    /// <param name="y">The column of the cell.</param>
    /// <param name="value">The value to check.</param>
    /// <returns><c>true</c> if the cell value is valid; <c>false</c> otherwise.</returns>
    private bool CheckRowCondition(int x, int y, byte value)
    {
        if (value == 0)
            return true;

        for (int i = 0; i < _board.GetLength(0); i++)
            if (i != x && _board[i, y] == value)
                return false;

        return true;
    }

    /// <summary>
    /// Checks whether the value of a cell is unique within a house.
    /// </summary>
    /// <param name="x">The row of the cell.</param>
    /// <param name="y">The column of the cell.</param>
    /// <returns><c>true</c> if the cell value is valid; <c>false</c> otherwise.</returns>
    private bool CheckHouseCondition(int x, int y, byte value)
    {
        if (value == 0)
            return true;

        for (int j = y / 3 * 3; j < y / 3 * 3 + 1; j++)
            for (int i = x / 3 * 3; i < x / 3 * 3 + 1; i++)
                if (j != y && i != x && _board[i, j] == value)
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
