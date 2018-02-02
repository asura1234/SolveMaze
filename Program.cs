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
            BitmapToUndirectedGraphConverter converter = new BitmapToUndirectedGraphConverter();
            Graph graph = converter.ConvertToGraph(maze);

            Point[] solution = graph.AStar(converter.startNode, converter.goalNode);

            Graphics graphics = Graphics.FromImage(maze);
            graphics.DrawPoints(solution, Color.Green);
            graphics.DrawImage(maze, new Point(0, 0));

            maze.Save(destinationName);

            // dispose images
            maze.Dispose();

            return;
        }
    }
}
