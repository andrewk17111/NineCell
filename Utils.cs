namespace NineCell;

internal static class Utils
{
    public const byte SIZE = 9;
    public const byte MIN_VALUE = 1;
    public const byte MAX_VALUE = SIZE;

    public static IEnumerable<byte> Range(byte start, byte end)
    {
        for (byte i = start; i <= end; i++)
            yield return i;
    }
}
