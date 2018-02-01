using System;
using System.Drawing;

namespace SolveMaze
{
    public static class Extension
    {
        public static Color GetPixel(this Bitmap map, Point pos)
        {
            return map.GetPixel(pos.X, pos.Y);
        }

        public static bool IsTheSameAs(this Color a, Color b)
        {
            return a.ToArgb() == b.ToArgb();
        }

    }
}
