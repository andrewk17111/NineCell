using NineCell;

Console.WriteLine(new Board());
Console.SetCursorPosition(1, 1);

while (true)
{
    ConsoleKeyInfo key_info = Console.ReadKey(true);

    ProcessInput(key_info.Key);
}

static void ProcessInput(ConsoleKey key)
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
}
