using System;
using System.Drawing;

namespace SolveMaze
{
    public static class Extension
    {
        static Size[] deltas = new Size[] { new Size(0, 1), new Size(0, -1), new Size(1, 0), new Size(-1, 0) };

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

        public static bool IsOutSide(this Point p, int Width, int Height)
        {
            return !IsInside(p, Width, Height);
        }

        public static bool IsWall(this Bitmap map, Point p)
        {
           return  map.GetPixel(p.X, p.Y).IsTheColorSameAs(Color.Black);
        }

        public static Point[] Neighbors(this Point p)
        {
            Point[] neighbors = new Point[deltas.Length];
            for (int i = 0; i < deltas.Length; i++)
            {
                neighbors[i] = new Point(p.X, p.Y) + deltas[i];
            }
            return neighbors;
        }
    }
}
