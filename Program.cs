using NineCell;
using System.Text.RegularExpressions;

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
    else if ((key >= ConsoleKey.D0 && key <= ConsoleKey.D9) || (key >= ConsoleKey.NumPad0 && key <= ConsoleKey.NumPad9))
    {
        int value = GetValueFromKey(key);
        (int x, int y) = ConvertPosition(Console.CursorLeft, Console.CursorTop);

        if (value == 0)
        {
            board.SetCell(x, y, 0);
            Console.Write(" ");
            Console.CursorLeft -= 1;
        }
        else if (board.TrySetCell(x, y, value))
        {
            Console.Write(value);
            Console.CursorLeft -= 1;
        }
    }
}

static int GetValueFromKey(ConsoleKey key)
    => key >= ConsoleKey.D0 && key <= ConsoleKey.D9
        ? key - ConsoleKey.D0
        : key >= ConsoleKey.NumPad0 && key <= ConsoleKey.NumPad9
            ? key - ConsoleKey.NumPad0
            : 0;

static (int, int) ConvertPosition(int x, int y)
    => (x - x / 4 - 1, y - y / 4 - 1);
