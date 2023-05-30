﻿namespace NineCell;

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
        => _value == 0 ? _notes.ToArray() : new byte[] { _value };

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

    public bool UpdateNotes()
    {
        bool updated = false;

        for (int y = 0; y < Utils.SIZE; y++)
            if (y != Y && Notes.Contains(_board[X, y]))
                updated = _notes.Remove(_board[X, y]) ? true : updated;

        for (int x = 0; x < Utils.SIZE; x++)
            if (x != X && Notes.Contains(_board[x, Y]))
                updated = _notes.Remove(_board[x, Y]) ? true : updated;

        for (int y = Y / 3 * 3; y < Y / 3 * 3 + 3; y++)
            for (int x = X / 3 * 3; x < X / 3 * 3 + 3; x++)
                if (!(y == Y && x == X) && Notes.Contains(_board[x, y]))
                    updated = _notes.Remove(_board[x, y]) ? true : updated;

        return updated;
    }

    public override string ToString()
        => Value.ToString();

    public static implicit operator byte(Cell cell)
        => cell.Value;
}
