using System;

namespace SolveMaze
{
    public class Map<T>
    {
        private T[,] data;
        private BinaryMatrix keys;
        public int Width;
        public int Height;
        public int Count;

        public Map(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            data = new T[width, height];
            keys = new BinaryMatrix(width, height);
            Count = 0;
        }

        public Map(int width, int height, T value)
        {
            this.Width = width;
            this.Height = height;
            data = new T[width, height];
            keys = new BinaryMatrix(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    data[i, j] = value;
                }
            }
        }

        public bool ContainsKey(Vector2D p)
        {
            return keys[p] == 1;
        }

        public T this[Vector2D p]
        {
            get { return data[p.x, p.y]; }
            set { data[p.x, p.y] = value; keys[p] = 1; Count++; }
        }

        public T this[int x, int y]
        {
            get { return data[x, y]; }
            set { data[x, y] = value; keys[x, y] = 1; Count++; }
        }
    }
}
