﻿namespace NineCell;

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
            // Cell has a single note.
            if (cell.Value == 0 && cell.Notes.Length == 1)
            {
                cell.Value = cell.Notes[0];
                updated = true;
            }

            foreach (byte n in cell.Notes)
            {
                // Cell is only in column with a certain note.
                for (int y = 0; y < Utils.SIZE; y++)
                {
                    if (y != cell.Y && _board[cell.X, y].Notes.Contains(n))
                    {
                        break;
                    }
                    else if (y == Utils.SIZE - 1)
                    {
                        cell.Value = n;
                        updated = true;
                    }
                }

                if (cell.Value != 0)
                    break;

                // Cell is only in row with a certain note.
                for (int x = 0; x < Utils.SIZE; x++)
                {
                    if (x != cell.X && _board[x, cell.Y].Notes.Contains(n)) {
                        break;
                    }
                    else if (x == Utils.SIZE - 1)
                    {
                        cell.Value = n;
                        updated = true;
                    }
                }

                if (cell.Value != 0)
                    break;

                // Cell is only in box with certain note.
                for (int y = cell.Y / 3 * 3; y < cell.Y / 3 * 3 + 3; y++)
                {
                    bool broken = false;

                    for (int x = cell.X / 3 * 3; x < cell.X / 3 * 3 + 3; x++)
                    {
                        if (!(y == cell.Y && x == cell.X) && _board[x, y].Notes.Contains(n))
                        {
                            broken = true;
                            break;
                        }
                        else if (y == cell.Y / 3 * 3 + 2 && x == cell.X / 3 * 3 + 2)
                        {
                            cell.Value = n;
                            updated = true;
                        }
                    }

                    if (broken)
                        break;
                }

                if (cell.Value != 0)
                    break;
            }
        }

        // Check for naked pairs.
        for (int y = 0; y < Utils.SIZE; y++)
        {
            Cell[]? row = GetRow(y);

            if (row is null)
                continue;

            for (int x = 0; x < Utils.SIZE; x += 3)
            {
                Cell[] sub_row = row[x..(x + 3)].Where(c => c.Value == 0).ToArray();

                if (sub_row.Length == 2 &&
                    sub_row.Select((c, i) => c.Notes.Length == 2 &&
                        c.Notes.SequenceEqual(sub_row[(i + 1) % sub_row.Length].Notes)).Aggregate((a, b) => a && b))
                {
                    for (int i = 0; i < Utils.SIZE; i++)
                        if (i < x || i > x + 2)
                            foreach (byte n in sub_row[0].Notes)
                                row[i].RemoveNote(n);

                    for (int y2 = sub_row[0].Y / 3 * 3; y2 < sub_row[0].Y / 3 * 3 + 3; y2++)
                    {
                        for (int x2 = sub_row[0].X / 3 * 3; x2 < sub_row[0].X / 3 * 3 + 3; x2++)
                        {
                            if (sub_row.Select(c => c.X == x2 && c.Y == y2).Aggregate((a, b) => a || b))
                                continue;

                            _board[x2, y2].RemoveNotes(sub_row[0].Notes);
                        }
                    }
                }
            }
        }

        for (int x = 0; x < Utils.SIZE; x++)
        {
            Cell[]? col = GetColumn(x);

            if (col is null)
                continue;

            for (int y = 0; y < Utils.SIZE; y += 3)
            {
                Cell[] sub_col = col[y..(y + 3)].Where(c => c.Value == 0).ToArray();

                if (sub_col.Length == 2 &&
                    sub_col.Select((c, i) => c.Notes.Length == 2 &&
                        c.Notes.SequenceEqual(sub_col[(i + 1) % sub_col.Length].Notes)).Aggregate((a, b) => a && b))
                {
                    for (int i = 0; i < Utils.SIZE; i++)
                        if (i < y || i > y + 2)
                            foreach (byte n in sub_col[0].Notes)
                                col[i].RemoveNote(n);

                    for (int y2 = sub_col[0].Y / 3 * 3; y2 < sub_col[0].Y / 3 * 3 + 3; y2++)
                    {
                        for (int x2 = sub_col[0].X / 3 * 3; x2 < sub_col[0].X / 3 * 3 + 3; x2++)
                        {
                            if (sub_col.Select(c => c.X == x2 && c.Y == y2).Aggregate((a, b) => a || b))
                                continue;

                            _board[x2, y2].RemoveNotes(sub_col[0].Notes);
                        }
                    }
                }
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
