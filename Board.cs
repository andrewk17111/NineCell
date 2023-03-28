namespace NineCell;

internal class Board
{
    private readonly int[,] _board;

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

    /// <summary>
    /// Get the value of a cell in the board.
    /// </summary>
    /// <param name="x">The row of the cell.</param>
    /// <param name="y">The column of the cell.</param>
    /// <returns>The int value of the cell.</returns>
    public int GetCell(int x, int y)
        => _board[x, y];

    /// <summary>
    /// Set the value of a cell in the board.
    /// </summary>
    /// <param name="x">The row of the cell.</param>
    /// <param name="y">The column of the cell.</param>
    /// <param name="value">The value of the cell.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <c>value</c> is out of range.</exception>
    public void SetCell(int x, int y, int value)
        => _board[x, y] = value < 0 || value > 9
            ? throw new ArgumentOutOfRangeException(nameof(value), value, $"Cell value must be between 0 and 9")
            : value;

    /// <summary>
    /// Tries to set the value of a cell in the board.
    /// </summary>
    /// <param name="x">The row of the cell.</param>
    /// <param name="y">The column of the cell.</param>
    /// <param name="value">The value of the cell.</param>
    /// <returns><c>true</c> if the cell value was set; <c>false</c> otherwise.</returns>
    public bool TrySetCell(int x, int y, int value)
    {
        if (IsValid(x, y, value))
        {
            SetCell(x, y, value);
            return true;
        }
        
        return false;
    }

    /// <summary>
    /// Checks whether a value of a cell is valid.
    /// </summary>
    /// <param name="x">The row of the cell.</param>
    /// <param name="y">The column of the cell.</param>
    /// <returns><c>true</c> if the cell value is valid; <c>false</c> otherwise.</returns>
    public bool IsValid(int x, int y)
        => IsValid(x, y, GetCell(x, y));

    /// <summary>
    /// Checks whether a value at a cell is valid.
    /// </summary>
    /// <param name="x">The row of the cell.</param>
    /// <param name="y">The column of the cell.</param>
    /// <param name="value">The value to check.</param>
    /// <returns><c>true</c> if the cell value is valid; <c>false</c> otherwise.</returns>
    public bool IsValid(int x, int y, int value)
        => CheckColumnCondition(x, y, value) && CheckRowCondition(x, y, value) && CheckHouseCondition(x, y, value);

    /// <summary>
    /// Checks whether the value of a cell is unique within a column.
    /// </summary>
    /// <param name="x">The row of the cell.</param>
    /// <param name="y">The column of the cell.</param>
    /// <returns><c>true</c> if the cell value is valid; <c>false</c> otherwise.</returns>
    private bool CheckColumnCondition(int x, int y)
        => CheckColumnCondition(x, y, GetCell(x, y));

    /// <summary>
    /// Checks whether a particular value at a cell is unique within a column.
    /// </summary>
    /// <param name="x">The row of the cell.</param>
    /// <param name="y">The column of the cell.</param>
    /// <param name="value">The value to check.</param>
    /// <returns><c>true</c> if the cell value is valid; <c>false</c> otherwise.</returns>
    private bool CheckColumnCondition(int x, int y, int value)
    {
        if (value == 0)
            return true;

        for (int i = 0; i < _board.GetLength(1); i++)
            if (i != y && _board[x, i] == value)
                return false;

        return true;
    }

    /// <summary>
    /// Checks whether the value of a cell is unique within a row.
    /// </summary>
    /// <param name="x">The row of the cell.</param>
    /// <param name="y">The column of the cell.</param>
    /// <returns><c>true</c> if the cell value is valid; <c>false</c> otherwise.</returns>
    private bool CheckRowCondition(int x, int y)
        => CheckRowCondition(x, y, GetCell(x, y));

    /// <summary>
    /// Checks whether a particular value at a cell is unique within a row.
    /// </summary>
    /// <param name="x">The row of the cell.</param>
    /// <param name="y">The column of the cell.</param>
    /// <param name="value">The value to check.</param>
    /// <returns><c>true</c> if the cell value is valid; <c>false</c> otherwise.</returns>
    private bool CheckRowCondition(int x, int y, int value)
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
    private bool CheckHouseCondition(int x, int y)
        => CheckHouseCondition(x, y, GetCell(x, y));

    /// <summary>
    /// Checks whether the value of a cell is unique within a house.
    /// </summary>
    /// <param name="x">The row of the cell.</param>
    /// <param name="y">The column of the cell.</param>
    /// <returns><c>true</c> if the cell value is valid; <c>false</c> otherwise.</returns>
    private bool CheckHouseCondition(int x, int y, int value)
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
