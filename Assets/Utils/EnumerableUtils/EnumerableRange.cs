using System.Collections.Generic;

public static class EnumerableRange
{
    public static IEnumerable<int> Range(this int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            yield return i;
        }
    }
}
