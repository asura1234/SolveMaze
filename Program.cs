using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SolveMaze
{
    class MainClass
    {
        static Image maze;
        static Image solved;

        // based on the pseudocode provided on Wikipedia for A* search algorithm
        // https://en.wikipedia.org/wiki/A*_search_algorithm#Pseudocode
        static Point[] AStar(Point start, Point goal)
        {
            
        }

        // based on the pseudocode provided on Wikipedia for A* search algorithm
        // https://en.wikipedia.org/wiki/A*_search_algorithm#Pseudocode
        static Point[] ReconstructPath()
        {
            List<Point> totalPath = new List<Point>();
            return totalPath.ToArray();
        }

        static void FindStartNGoal(out Point start, out Point end)
        {
            
        }

        public static void Main(string[] args)
        {
            string sourceName = args[0];
            string destinationName = args[1];

            // load maze image from file
            maze = Image.FromFile(sourceName);

            // find the start and goal point
            Point start, goal;
            FindStartNGoal(out start, out goal);

            // draw solution on the maze
            solved = (Image)maze.Clone();
            Point[] path = AStar(start, goal);
            Graphics graph = Graphics.FromImage(solved);
            Pen myPen = new Pen(Color.Green, 1);
            for (int i = 0; i < path.Length; i++)
                graph.DrawLines(myPen, path);
            graph.DrawImage(solved, new Point(0, 0) );

            // save solution image to destination
            solved.Save(destinationName);

            // dispose images
            maze.Dispose();
            solved.Dispose();

            return;
        }
    }
}
