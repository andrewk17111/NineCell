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
        if (Value != 0)
            return false;

        bool updated = false;

        IEnumerable<Cell> pool = _board.GetRow(Y)!
            .Union(_board.GetColumn(X)!)
            .Union(_board.GetBox(X, Y)!)
            /*.Distinct()*/;

        foreach (Cell cell in pool)
            if (Notes.Contains(cell))
                updated = _notes.Remove(cell) || updated;

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
