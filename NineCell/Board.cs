namespace NineCell;

public class Board
{
    private readonly Cell[,] _board;

    // Solving Cache.
    private readonly List<Cell>[] _rows_unsolved;
    private readonly List<Cell>[] _columns_unsolved;
    private readonly List<Cell>[,] _boxes_unsolved;

    public bool Complete
    {
        get
        {
            foreach (Cell cell in _board)
                if (cell.Value == 0)
                    return false;

            return true;
        }
    }

    public Cell this[int x, int y] {
        get => _board[x, y];
    }

    /// <summary>
    /// Creates a new sudoku Board.
    /// </summary>
    public Board()
    {
        _board = InitBoard();
        (_rows_unsolved, _columns_unsolved, _boxes_unsolved) = InitCache();
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

    private (List<Cell>[], List<Cell>[], List<Cell>[,]) InitCache()
    {
        List<Cell>[] rows_unsolved = new List<Cell>[Utils.SIZE];
        List<Cell>[] columns_unsolved = new List<Cell>[Utils.SIZE];
        List<Cell>[,] boxes_unsolved = new List<Cell>[Utils.SIZE / 3, Utils.SIZE / 3];

        for (int y = 0; y < Utils.SIZE; y++)
            rows_unsolved[y] = GetRow(y)!.ToList();

        for (int x = 0; x < Utils.SIZE; x++)
            columns_unsolved[x] = GetColumn(x)!.ToList();

        for (int y = 0; y < Utils.SIZE; y += 3)
            for (int x = 0; x < Utils.SIZE; x += 3)
                boxes_unsolved[x / 3, y / 3] = GetBox(x, y)!.ToList();

        return (rows_unsolved, columns_unsolved, boxes_unsolved);
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
                Cell[] row = GetRowUnsolved(y)!;
                Cell[][] sets = GetNakedN(n, row, out byte[][] notes);

                for (int i = 0; i < sets.Length; i++)
                    foreach (Cell cell in row.Except(sets[i]))
                        updated = cell.RemoveNotes(notes[i]) || updated;
            }

            for (int x = 0; x < Utils.SIZE; x++)
            {
                Cell[] column = GetColumnUnsolved(x)!;
                Cell[][] sets = GetNakedN(n, column, out byte[][] notes);

                for (int i = 0; i < sets.Length; i++)
                    foreach (Cell cell in column.Except(sets[i]))
                        updated = cell.RemoveNotes(notes[i]) || updated;
            }

            for (int y = 0; y < Utils.SIZE; y += 3)
            {
                for (int x = 0; x < Utils.SIZE; x += 3)
                {
                    Cell[] box = GetBoxUnsolved(x, y)!;
                    Cell[][] sets = GetNakedN(n, box, out byte[][] notes);

                    for (int i = 0; i < sets.Length; i++)
                        foreach (Cell cell in box.Except(sets[i]))
                            updated = cell.RemoveNotes(notes[i]) || updated;
                }
            }

            // Hidden Pairs/Triples/Quadruples.
            for (int y = 0; y < Utils.SIZE; y++)
            {
                Cell[] row = GetRowUnsolved(y)!;
                Cell[][] sets = GetHiddenN(n, row, out byte[][] notes);

                for (int i = 0; i < sets.Length; i++)
                    foreach (Cell cell in sets[i])
                        updated = cell.RemoveNotesExcept(notes[i]) || updated;
            }

            for (int x = 0; x < Utils.SIZE; x++)
            {
                Cell[][] sets = GetHiddenN(n, GetColumnUnsolved(x)!, out byte[][] notes);

                for (int i = 0; i < sets.Length; i++)
                    foreach (Cell cell in sets[i])
                        updated = cell.RemoveNotesExcept(notes[i]) || updated;
            }

            for (int y = 0; y < Utils.SIZE; y += 3)
            {
                for (int x = 0; x < Utils.SIZE; x += 3)
                {
                    Cell[] box = GetBoxUnsolved(x, y)!;
                    Cell[][] sets = GetHiddenN(n, box, out byte[][] notes);

                    for (int i = 0; i < sets.Length; i++)
                        foreach (Cell cell in sets[i])
                            updated = cell.RemoveNotesExcept(notes[i]) || updated;
                }
            }
        }

        return updated;
    }

    public bool UpdateValues()
    {
        bool updated = false;
        IEnumerable<Cell> unsolved_cells = _rows_unsolved.SelectMany(c => c);

        foreach (Cell cell in unsolved_cells)
        {
            if (cell.Value != 0)
                continue;

            // Naked Singles.
            if (cell.Notes.Length == 1)
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
                    if (x != cell.X && _board[x, cell.Y].Notes.Contains(n))
                    {
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
        if (n == 0 || cells is null || cells.Length == 0)
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

    private Cell[]? GetRowUnsolved(int y)
    {
        if (y < 0 || y >= Utils.SIZE)
            return null;

        for (int x = 0; x < _rows_unsolved[y].Count; x++)
        {
            if (_rows_unsolved[y][x].Value != 0)
            {
                _rows_unsolved[y].RemoveAt(x);
                x--;
            }
        }

        return _rows_unsolved[y].ToArray();
    }

    private Cell[]? GetColumnUnsolved(int x)
    {
        if (x < 0 || x >= Utils.SIZE)
            return null;

        for (int y = 0; y < _columns_unsolved[x].Count; y++)
        {
            if (_columns_unsolved[x][y].Value != 0)
            {
                _columns_unsolved[x].RemoveAt(y);
                y--;
            }
        }

        return _columns_unsolved[x].ToArray();
    }

    private Cell[]? GetBoxUnsolved(int x, int y)
    {
        if (x < 0 || x >= Utils.SIZE || y < 0 || y >= Utils.SIZE)
            return null;

        for (int i = 0; i < _boxes_unsolved[x / 3, y / 3].Count; i++)
        {
            if (_boxes_unsolved[x / 3, y / 3][i].Value != 0)
            {
                _boxes_unsolved[x / 3, y / 3].RemoveAt(i);
                i--;
            }
        }

        return _boxes_unsolved[x / 3, y / 3].ToArray();
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

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != typeof(Board))
            return false;

        Board board = (Board)obj;

        for (int y = 0; y < Utils.SIZE; y++)
            for (int x = 0; x < Utils.SIZE; x++)
                if (this[x, y].Value != board[x, y].Value)
                    return false;

        return true;
    }
}
