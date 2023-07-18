namespace NineCell;

public class Cell
{
    private Board _board;
    private byte _value;
    private List<byte> _notes;

    public readonly int X;
    public readonly int Y;

    public byte Value
    {
        get => _value;
        set => _value = Immutable ? _value : value;
    }
    public byte[] Notes
        => _value == 0 ? _notes.ToArray() : new byte[] { _value };
    public bool Immutable
    {
        get;
        set;
    }

    public Cell(int x, int y, Board board)
    {
        _board = board;
        _notes = new List<byte>(Utils.Range(Utils.MIN_VALUE, Utils.MAX_VALUE));
        Value = 0;
        X = x;
        Y = y;
    }

    public bool AddNote(byte note)
    {
        if (Notes.Contains(note))
            return false;

        _notes.Add(note);
        return true;
    }

    public bool RemoveNote(byte note)
        => Notes.Contains(note) && _notes.Remove(note);

    public bool RemoveNotes(IEnumerable<byte> notes)
        => notes.Aggregate(false, (updated, note) => RemoveNote(note) || updated);

    public bool RemoveNotesExcept(IEnumerable<byte> notes)
        => RemoveNotes(Utils.Range(Utils.MIN_VALUE, Utils.MAX_VALUE).Except(notes));

    public bool UpdateNotes()
    {
        bool updated = false;

        for (int y = 0; y < Utils.SIZE; y++)
            if (y != Y && Notes.Contains(_board[X, y]))
                updated = _notes.Remove(_board[X, y]) || updated;

        for (int x = 0; x < Utils.SIZE; x++)
            if (x != X && Notes.Contains(_board[x, Y]))
                updated = _notes.Remove(_board[x, Y]) || updated;

        for (int y = Y / 3 * 3; y < Y / 3 * 3 + 3; y++)
            for (int x = X / 3 * 3; x < X / 3 * 3 + 3; x++)
                if (!(y == Y && x == X) && Notes.Contains(_board[x, y]))
                    updated = _notes.Remove(_board[x, y]) || updated;

        return updated;
    }

    public void Reset()
    {
        Value = 0;
        _notes = new List<byte>(Utils.Range(Utils.MIN_VALUE, Utils.MAX_VALUE));
    }

    public override string ToString()
        => Value == 0 ? " " : $"{(Immutable ? AnsiUtils.bold : AnsiUtils.fgcyan)}{Value}{AnsiUtils.reset}";

    public static implicit operator byte(Cell cell)
        => cell.Value;
}
