using System;
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

        public int this[Vector2D p]
        {
            get
            {
                if (data[p.x, p.y])
                    return 1;
                else
                    return 0;
            }
            set
            {
                if (value == 0)
                    data[p.x, p.y] = false;
                else
                {
                    data[p.x, p.y] = true;
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
            set
            {
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
}
