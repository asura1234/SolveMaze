using System;
using System.Drawing;

namespace SolveMaze
{
    public static class Extension
    {
        public static bool IsTheColorSameAs(this Color a, Color b)
        {
            var dr = a.R - b.R;
            var dg = a.G - b.G;
            var db = a.B - b.B;
            var distance = Math.Sqrt(dr * dr + dg * dg + db * db);
            return distance < 32;
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

        public static bool IsWall(this Bitmap map, Vector2D p)
        {
            return map.GetPixel(p.x, p.y).IsTheColorSameAs(Color.Black);
        }

        public static bool IsStart(this Bitmap map, Vector2D p)
        {
            return map.GetPixel(p.x, p.y).IsTheColorSameAs(Color.Blue);
        }

        public static bool IsGoal(this Bitmap map, Vector2D p)
        {
            return map.GetPixel(p.x, p.y).IsTheColorSameAs(Color.Red);
        }

        public static bool IsEmptySpace(this Bitmap map, Vector2D p)
        {
            return map.GetPixel(p.x, p.y).IsTheColorSameAs(Color.White);
        }
    }
}
