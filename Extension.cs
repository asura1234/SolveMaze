// copyright Dylan Liu

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

        public static void DrawPoint(this Bitmap map, Vector2D p, Color c, int size = 1)
        {
            if (size == 1)
                map.SetPixel(p.x, p.y, c);
            else if (size > 1)
            {
                Graphics graphics= Graphics.FromImage(map);
                Pen myPen = new Pen(Color.Green, 1);
                graphics.DrawCircle(myPen, (float )p.x, (float) p.y, size);
            }
        }

        public static void DrawCircle(this Graphics g, Pen pen,
                                  float centerX, float centerY, float radius)
        {
            g.DrawEllipse(pen, centerX - radius, centerY - radius,
                          radius + radius, radius + radius);
            
            g.FillEllipse(pen.Brush, centerX - radius, centerY - radius,
                          radius + radius, radius + radius);
        }

        public static bool IsWall(this Color c)
        {
            return c.R < 64 && c.G < 64 && c.B < 64;
        }

        public static bool IsStart(this Color c)
        {
            return c.R < 32 && c.G < 32 && c.B > 220;
        }

        public static bool IsGoal(this Color c)
        {
            return c.R > 220 && c.G < 32 && c.B < 32;
        }
    }
}
