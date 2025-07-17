namespace TicTacToe.Core.Utils;

public static class Randomizer
{
    private static readonly Random _random = new(DateTime.Now.GetHashCode());

    public static bool GetHalfChance()
    {
        return _random.Next(0, 2) == 1;
    }

    public static int GetInt(int min, int max)
    {
        return _random.Next(min, max);
    }

    public static int GetInt(int max)
    {
        return _random.Next(max);
    }
}
