using System;
using System.Drawing;
using System.Collections.Generic;

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

        public static bool IsInside(this Point p, int x, int y, int Width, int Height)
        {
            return p.X > x && p.X < Width && p.Y > y && p.Y < Height;
        }

        public static bool IsOutSide(this Point p, int x, int y, int Width, int Height)
        {
            return !IsInside(p, x, y, Width, Height);
        }

        public static bool IsWall(this Bitmap map, Point p)
        {
           return  map.GetPixel(p.X, p.Y).IsTheColorSameAs(Color.Black);
        }

        public static bool IntersectWall(this Bitmap map, Point from, Point to)
        {
            List<Point> line = new List<Point>();
            int x0 = from.X, y0 = from.Y, x1 = to.X, y1 = to.Y,
            dx = x1 - x0, dy = y1 - y0,
            xi = 1, D;

            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }
            D = 2 * dx - dy;

            int x = x0;

            for (int y = y0; y <= y1; y++)
            {
                var p = new Point(x, y);
                if (map.IsWall(p))
                    return true;
                if (D > 0)
                {
                    x = x + xi;
                    D = D - 2 * dy;
                }
                D = D + 2 * dx;
            }
            return false;
        }

        public static bool IsNearWall(this Bitmap map, Point p)
        {
            foreach (Size delta in deltas)
            {
                var neighbor = p + delta;
                if (IsWall(map, neighbor))
                    return true;
            }
            return false;
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
