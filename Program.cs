using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SolveMaze
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string sourceName = args[0];
            string destinationName = args[1];

            Bitmap maze = new Bitmap(sourceName);
            Node start, goal;
            Graph graph = maze.ConvertToGraph(out start, out goal);
            Point[] solution = graph.AStar(start, goal);

            Graphics graphics = Graphics.FromImage(maze);
            Pen myPen = new Pen(Color.Green, 1);
            for (int i = 0; i < solution.Length; i++)
                graphics.DrawLines(myPen, solution);
            graphics.DrawImage(maze, new Point(0, 0));

            maze.Save(destinationName);

            // dispose images
            maze.Dispose();

            return;
        }
    }
}
