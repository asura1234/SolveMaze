using System;
using System.Drawing;

namespace SolveMaze
{
    public static class Extension
    {
        public static bool IsTheColorSameAs(this Color a, Color b)
        {
            return a.ToArgb() == b.ToArgb();
        }

        public static void DrawPoint(this Bitmap map, Point p, Color c)
        {
            map.SetPixel(p.X, p.Y, c);
        }

        public static bool IsInside(this Point p, int Width, int Height)
        {
            return p.X > 0 && p.X < Width && p.Y > 0 && p.Y < Height;
        }

        public static bool IsWall(this Bitmap map, Point p)
        {
           return  map.GetPixel(p.X, p.Y).IsTheColorSameAs(Color.Black);
        }
    }
}
