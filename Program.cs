using NineCell;

Board board = new Board();

Console.WriteLine(board);
Console.SetCursorPosition(1, 1);

while (true)
{
    ConsoleKeyInfo key_info = Console.ReadKey(true);

    ProcessInput(key_info.Key);
}

void ProcessInput(ConsoleKey key)
{
    if (key == ConsoleKey.UpArrow && Console.CursorTop > 1)
    {
        Console.CursorTop -= 1;

        if (Console.CursorTop % 4 == 0)
            Console.CursorTop -= 1;
    }
    else if (key == ConsoleKey.DownArrow && Console.CursorTop < 11)
    {
        Console.CursorTop += 1;

        if (Console.CursorTop % 4 == 0)
            Console.CursorTop += 1;
    }
    else if (key == ConsoleKey.LeftArrow && Console.CursorLeft > 1)
    {
        Console.CursorLeft -= 1;

        if (Console.CursorLeft % 4 == 0)
            Console.CursorLeft -= 1;
    }
    else if (key == ConsoleKey.RightArrow && Console.CursorLeft < 11)
    {
        Console.CursorLeft += 1;

        if (Console.CursorLeft % 4 == 0)
            Console.CursorLeft += 1;
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
        if (board.UpdateNotes())
        {
            if (board.UpdateValues())
            {
                (int left, int top) = Console.GetCursorPosition();

                Console.SetCursorPosition(0, 0);
                Console.WriteLine(board);
                Console.SetCursorPosition(left, top);
            }
        }
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
