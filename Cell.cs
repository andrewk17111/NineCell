using System.Numerics;

namespace NineCell;

internal class Cell
{
    private Board _board;
    private byte _value;
    private List<byte> _notes;

    public readonly int X;
    public readonly int Y;

    public byte Value
    {
        get => _value;
        set => _value = value;
    }
    public byte[] Notes
        => _notes.ToArray();

    public Cell(int x, int y, Board board)
    {
        _board = board;
        Value = 0;
        _notes = new List<byte>(Utils.Range(Utils.MIN_VALUE, Utils.MAX_VALUE));
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

    public static implicit operator byte(Cell cell)
        => cell.Value;
}
