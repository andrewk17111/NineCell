using NineCell;

Board board = new Board();

if (args.Length > 0 && File.Exists(args[0]))
{
    string[] lines = File.ReadAllLines(args[0]);

    for (int j = 0; j < lines.Length && j < Utils.SIZE; j++)
    {
        for (int i = 0; i < lines[j].Length && i < Utils.SIZE; i++)
        {
            if (lines[j][i] >= '1' && lines[j][i] <= '9')
            {
                board[i, j].Value = (byte)Math.Max(lines[j][i] - '0', 0);
                board[i, j].Immutable = true;
            }
        }
    }
}

Console.Clear();
Console.WriteLine(board.Render(0, 0));
Thread.Sleep(-1);
Console.WriteLine(board);
Console.SetCursorPosition(1, 1);
PrintNotes();

while (true)
{
    ConsoleKeyInfo key_info = Console.ReadKey(true);

    ProcessInput(key_info.Key, key_info.Modifiers);
}

void ProcessInput(ConsoleKey key, ConsoleModifiers modifiers)
{
    bool ctrl = (modifiers & ConsoleModifiers.Control) != 0;
    bool shift = (modifiers & ConsoleModifiers.Shift) != 0;

    if (key == ConsoleKey.UpArrow || key == ConsoleKey.W)
    {
        if (Console.CursorTop == 1)
        {
            Console.CursorTop = ctrl ? 9 : 11;
        }
        else if (ctrl)
        {
            Console.CursorTop = (Console.CursorTop - 2) / 4 * 4 + 1;
        }
        else
        {
            Console.CursorTop -= 1;

            if (Console.CursorTop % 4 == 0)
                Console.CursorTop -= 1;
        }
    }
    else if (key == ConsoleKey.DownArrow || key == ConsoleKey.S)
    {
        if (Console.CursorTop == 11 || (ctrl && Console.CursorTop > 8))
        {
            Console.CursorTop = 1;
        }
        else if (ctrl)
        {
            Console.CursorTop = Console.CursorTop / 4 * 4 + 5;
        }
        else
        {
            Console.CursorTop += 1;

            if (Console.CursorTop % 4 == 0)
                Console.CursorTop += 1;
        }
    }
    else if (key == ConsoleKey.LeftArrow || key == ConsoleKey.A)
    {
        if (Console.CursorLeft == 1)
        {
            Console.CursorLeft = ctrl ? 9 : 11;
        }
        else if (ctrl)
        {
            Console.CursorLeft = (Console.CursorLeft - 2) / 4 * 4 + 1;
        }
        else
        {
            Console.CursorLeft -= 1;

            if (Console.CursorLeft % 4 == 0)
                Console.CursorLeft -= 1;
        }
    }
    else if (key == ConsoleKey.RightArrow || key == ConsoleKey.D)
    {
        if (Console.CursorLeft == 11 || (ctrl && Console.CursorLeft > 8))
        {
            Console.CursorLeft = 1;
        }
        else if (ctrl)
        {
            Console.CursorLeft = Console.CursorLeft / 4 * 4 + 5;
        }
        else
        {
            Console.CursorLeft += 1;

            if (Console.CursorLeft % 4 == 0)
                Console.CursorLeft += 1;
        }
    }
    else if (shift && ((key >= ConsoleKey.D1 && key <= ConsoleKey.D9) ||
        (key >= ConsoleKey.NumPad1 && key <= ConsoleKey.NumPad9)))
    {
        byte value = GetValueFromKey(key);
        (int x, int y) = ConvertPosition(Console.CursorLeft, Console.CursorTop);

        if (board[x, y].Value == 0)
        {
            if (board[x, y].Notes.Contains(value))
                board[x, y].RemoveNote(value);
            else
                board[x, y].AddNote(value);
        }
    }
    else if ((key >= ConsoleKey.D0 && key <= ConsoleKey.D9) ||
        (key >= ConsoleKey.NumPad0 && key <= ConsoleKey.NumPad9) ||
        key == ConsoleKey.Spacebar || key == ConsoleKey.Delete || key == ConsoleKey.Backspace)
    {
        byte value = GetValueFromKey(key);
        (int x, int y) = ConvertPosition(Console.CursorLeft, Console.CursorTop);

        if (board[x, y] != value)
        {
            board[x, y].Value = value;
            Console.Write(board[x, y].ToString());
            Console.CursorLeft -= 1;
        }
    }
    else if (key == ConsoleKey.Enter || key == ConsoleKey.Spacebar)
    {
        board.UpdateNotes();

        if (board.UpdateValues())
        {
            (int left, int top) = Console.GetCursorPosition();

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(board);
            Console.SetCursorPosition(left, top);
        }
    }
    else if (key == ConsoleKey.Escape)
    {
        board = new Board();
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        Console.WriteLine(board);
        Console.SetCursorPosition(1, 1);
    }
    else if (key == ConsoleKey.R)
    {
        (int x, int y) = ConvertPosition(Console.CursorLeft, Console.CursorTop);

        board[x, y].Reset();
        Console.Write(board[x, y].ToString());
        Console.CursorLeft -= 1;
    }

    PrintNotes();
}

static byte GetValueFromKey(ConsoleKey key)
    => key >= ConsoleKey.D0 && key <= ConsoleKey.D9
        ? (byte)(key - ConsoleKey.D0)
        : key >= ConsoleKey.NumPad0 && key <= ConsoleKey.NumPad9
            ? (byte)(key - ConsoleKey.NumPad0)
            : (byte)0;

static (int, int) ConvertPosition(int x, int y)
    => (x - x / 4 - 1, y - y / 4 - 1);

void PrintNotes()
{
    (int left, int top) = Console.GetCursorPosition();
    (int x, int y) = ConvertPosition(left, top);

    Console.SetCursorPosition(13, 0);
    Console.Write("                         ");
    Console.SetCursorPosition(13, 0);
    Console.Write(String.Join(", ", board[x, y].Notes));
    Console.SetCursorPosition(left, top);
}
