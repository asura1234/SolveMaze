using System;
using System.Drawing;

namespace SolveMaze
{
    public class BinaryMatrix
    {
        private bool[,] data;
        public int Width;
        public int Height;
        public int Count;

        public BinaryMatrix(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            data = new bool[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    data[i, j] = false;
                }
            }
            Count = 0;
        }

        public int this[Point p]
        {
            get
            {
                if (data[p.X, p.Y])
                    return 1;
                else
                    return 0;
            }
            set
            {
                if (value == 0)
                    data[p.X, p.Y] = false;
                else
                {
                    data[p.X, p.Y] = true;
                    Count++;
                }
            }
        }

        public int this[int x, int y]
        {
            get
            {
                if (data[x, y])
                    return 1;
                else
                    return 0;
            }
            set{
                if (value == 0)
                    data[x, y] = false;
                else
                {
                    data[x, y] = true;
                    Count++;
                }
            }
        }
    }

    public class PointDictioinary<T>
    {
        private T[,] data;
        private BinaryMatrix keys;
        public int Width;
        public int Height;
        public int Count;

        public PointDictioinary(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            data = new T[width, height];
            keys = new BinaryMatrix(width, height);
            Count = 0;
        }

        public bool ContainsKey(Point p)
        {
            return keys[p] == 1;
        }

        public PointDictioinary(int width, int height, T value)
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

        public T this[Point p]
        {
            get { return data[p.X, p.Y]; }
            set { data[p.X, p.Y] = value; keys[p] = 1; Count++; }
        }

        public T this[int x, int y]
        {
            get { return data[x, y]; }
            set { data[x, y] = value; keys[x, y] = 1; Count++; }
        }
    }
}
