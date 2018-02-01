using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SolveMaze
{
    class MainClass
    {
        static Bitmap maze;
        static Point start, goal;



        public static void Main(string[] args)
        {
            string sourceName = args[0];
            string destinationName = args[1];

            maze = new Bitmap(sourceName);
            maze.ConvertToGraph();

            // dispose images
            maze.Dispose();

            return;
        }
    }
}
