using System;
namespace SolveMaze
{
    public class Vector2D
    {
        private static Vector2D[] deltas = new Vector2D[] { new Vector2D(0, 1), new Vector2D(0, -1), new Vector2D(1, 0), new Vector2D(-1, 0) };

        public int x;
        public int y;

        public Vector2D(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.x - v2.x, v1.y - v2.y);
        }

        public static bool operator ==(Vector2D v1, Vector2D v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }

        public static double DistanceBetween(Vector2D v1, Vector2D v2)
        {
            int dx = v2.x - v1.x;
            int dy = v2.y - v1.x;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public bool Equals(Vector2D other)
        {
            return this == other;
        }

        public static bool operator !=(Vector2D v1, Vector2D v2)
        {
            return v1.x != v2.x || v1.y != v2.y;
        }

        public Vector2D[] Neighbors()
        {
            Vector2D[] neighbors = new Vector2D[deltas.Length];
            for (int i = 0; i < deltas.Length; i++)
            {
                neighbors[i] = this + deltas[i];
            }
            return neighbors;
        }

        public bool IsInsideBound(int x, int y, int Width, int Height)
        {
            return this.x > 0 && this.x < Width && this.y > 0 && this.y < Height;
        }

        public bool IsOutSideBound(int x, int y, int Width, int Height) => !this.IsInsideBound(x, y, Width, Height);
    }
}
