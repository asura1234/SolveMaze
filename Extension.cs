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

        public static void DrawPoint(this Graphics graphics, Point p, Color c)
        {
            Pen myPen = new Pen(c, 1);
            graphics.FillRectangle(myPen.Brush, p.X, p.Y, 1, 1);
        }

        public static void DrawPoints(this Graphics graphics, Point[] ps, Color c)
        {
            for (int i = 0; i < ps.Length; i++)
            {
                graphics.DrawPoint(ps[i], c);
            }
        }
    }
}
