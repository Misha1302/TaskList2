namespace TaskList2.Controllers;

public static class HashManager
{
    public static int GetHashOfString(this string text)
    {
        unchecked
        {
            return text.Aggregate(23, (current, c) => current * 31 + c);
        }
    }
}