// Source - https://stackoverflow.com/a
// Posted by kprobst
// Retrieved 2025-12-23, License - CC BY-SA 2.5

using System.Collections.Generic;

public static class Utils
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        Random rnd = new();
        while (n > 1)
        {
            int k = rnd.Next(0, n) % n;
            n--;
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}
