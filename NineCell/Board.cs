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
                updated = _board[x, y].UpdateNotes() || updated;

        for (int n = 2; n < 5; n++)
        {
            // Naked Pairs/Triples/Quadruples.
            for (int y = 0; y < Utils.SIZE; y++)
            {
                Cell[] row = GetRow(y)!;
                Cell[][] sets = GetNakedN(n, row, out byte[][] notes);

                for (int i = 0; i < sets.Length; i++)
                    foreach (Cell cell in row.Except(sets[i]))
                        cell.RemoveNotes(notes[i]);
            }

            for (int x = 0; x < Utils.SIZE; x++)
            {
                Cell[] column = GetColumn(x)!;
                Cell[][] sets = GetNakedN(n, column, out byte[][] notes);

                for (int i = 0; i < sets.Length; i++)
                    foreach (Cell cell in column.Except(sets[i]))
                        cell.RemoveNotes(notes[i]);
            }

            for (int y = 0; y < Utils.SIZE; y += 3)
            {
                for (int x = 0; x < Utils.SIZE; x += 3)
                {
                    Cell[] box = GetBox(x, y)!;
                    Cell[][] sets = GetNakedN(n, box, out byte[][] notes);

                    for (int i = 0; i < sets.Length; i++)
                        foreach (Cell cell in box.Except(sets[i]))
                            cell.RemoveNotes(notes[i]);
                }
            }

            // Hidden Pairs/Triples/Quadruples.
            for (int y = 0; y < Utils.SIZE; y++)
            {
                Cell[] row = GetRow(y)!;
                Cell[][] sets = GetHiddenN(n, row, out byte[][] notes);

                for (int i = 0; i < sets.Length; i++)
                    foreach (Cell cell in sets[i])
                        cell.SetNotes(notes[i]);
            }

            for (int x = 0; x < Utils.SIZE; x++)
            {
                Cell[][] sets = GetHiddenN(n, GetColumn(x)!, out byte[][] notes);

                for (int i = 0; i < sets.Length; i++)
                    foreach (Cell cell in sets[i])
                        cell.SetNotes(notes[i]);
            }

            for (int y = 0; y < Utils.SIZE; y += 3)
            {
                for (int x = 0; x < Utils.SIZE; x += 3)
                {
                    Cell[] box = GetBox(x, y)!;
                    Cell[][] sets = GetHiddenN(n, box, out byte[][] notes);

                    for (int i = 0; i < sets.Length; i++)
                        foreach (Cell cell in sets[i])
                            cell.SetNotes(notes[i]);
                }
            }
        }

        return updated;
    }

    public bool UpdateValues()
    {
        bool updated = false;

        foreach (Cell cell in _board)
        {
            // Naked Singles.
            if (cell.Value == 0 && cell.Notes.Length == 1)
            {
                cell.Value = cell.Notes[0];
                updated = true;
            }

            // Hidden Singles.
            foreach (byte n in cell.Notes)
            {
                // Column.
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

                // Row.
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

                // Box.
                foreach (Cell box_cell in GetBox(cell.X, cell.Y)!)
                {
                    int x = box_cell.X;
                    int y = box_cell.Y;

                    if (box_cell != cell && _board[x, y].Notes.Contains(n))
                    {
                        break;
                    }
                    else if (y == cell.Y / 3 * 3 + 2 && x == cell.X / 3 * 3 + 2)
                    {
                        cell.Value = n;
                        updated = true;
                    }
                }

                if (cell.Value != 0)
                    break;
            }
        }

        return updated;
    }
    
    private static Cell[][] GetNSubSet(int n, Cell[]? cells)
    {
        if (n == 0 || cells is null)
            return Array.Empty<Cell[]>();
        else if (n == 1)
            return cells.Select(c => new Cell[] { c }).ToArray();
        
        List<Cell[]> sets = new List<Cell[]>();

        for (int i = 0; i < cells.Length - n + 1; i++)
        {
            IEnumerable<Cell[]> sub_sets = GetNSubSet(n - 1, cells[(i + 1)..]);

            foreach (Cell[] sub_set in  sub_sets)
                sets.Add(sub_set.Prepend(cells[i]).ToArray());
        }

        return sets.ToArray();
    }

    private static Cell[][] GetNakedN(int n, Cell[]? cells, out byte[][] notes)
    {
        notes = Array.Empty<byte[]>();

        if (n == 0 || cells is null)
            return Array.Empty<Cell[]>();

        Cell[][] sets = (from ngram in GetNSubSet(n, cells)
                         where ngram.All(cell => cell.Value == 0)
                         select ngram)
            .ToArray();
        byte[][] all_notes = (from set in sets
                              select set.Select(c => c.Notes).Aggregate((a, b) => a.Union(b).ToArray()))
            .ToArray();

        sets = sets.Where((set, i) => all_notes[i].Length == n).ToArray();
        notes = all_notes.Where(x => x.Length == n).ToArray();

        return sets;
    }

    private static Cell[][] GetHiddenN(int n, Cell[]? cells, out byte[][] notes)
    {
        notes = Array.Empty<byte[]>();

        if (n == 0 || cells is null)
            return Array.Empty<Cell[]>();

        Cell[][] sets = (from ngram in GetNSubSet(n, cells)
                         where ngram.All(cell => cell.Value == 0)
                         select ngram)
            .ToArray();
        byte[][] all_notes = (from set in sets
                              select set.Select(c => c.Notes).Aggregate((a, b) => a.Intersect(b).ToArray()))
            .ToArray();

        for (int i = 0; i < sets.Length; i++)
        {
            IEnumerable<Cell> domain = cells.Except(sets[i]).Where(cell => cell.Value == 0).ToArray();

            all_notes[i] = (from note in all_notes[i]
                            where !domain.Any(cell => cell.Notes.Contains(note))
                            select note)
                .ToArray();
        }

        sets = sets.Where((set, i) => all_notes[i].Length == n)
            .ToArray();
        notes = all_notes.Where(x => x.Length == n).ToArray();

        return sets;
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

    private Cell[]? GetBox(int x, int y)
    {
        if (x >= 0 && x < Utils.SIZE && y >= 0 && y < Utils.SIZE)
        {
            List<Cell> result = new List<Cell>();

            for (int j = y / 3 * 3; j < y / 3 * 3 + 3; j++)
                for (int i = x / 3 * 3; i < x / 3 * 3 + 3; i++)
                    result.Add(_board[i, j]);

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
