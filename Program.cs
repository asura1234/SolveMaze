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

        public class Node : IComparable<Node>
        {
            public Point Point;
            public double fScore;

            public Node(Point pos, double score)
            {
                Point = pos;
                fScore = score;
            }

            public int CompareTo(Node other)
            {
                if (this.fScore < other.fScore)
                    return -1;
                else if (this.fScore > other.fScore)
                    return 1;
                else
                    return 0;
            }

            public bool Equals(Point other)
            {
                return Point.X == other.X && Point.Y == other.Y;
            }
        }

        // based on the pseudocode provided on Wikipedia for A* search algorithm
        // https://en.wikipedia.org/wiki/A*_search_algorithm#Pseudocode
        static Point[] AStar(Point start, Point goal)
        {
            Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();
            Dictionary<Point, double> gScore = new Dictionary<Point, double>();
            Dictionary<Point, double> fScore = new Dictionary<Point, double>();

            for (int i = 0; i < maze.Width; i++)
            {
                for (int j = 0; j < maze.Height; j++)
                {
                    gScore.Add(new Point(i, j), double.PositiveInfinity);
                    fScore.Add(new Point(i, j), double.PositiveInfinity);
                }
            }
            gScore[start] = 0;

            List<Point> closedSet = new List<Point>();
            PriorityQueue<Node> openSet = new PriorityQueue<Node>();
            openSet.Enqueue(new Node(start, fScore[start]));

            while (openSet.IsEmpty())
            {
                Node current = openSet.Dequeue();
                if (current.Equals(goal))
                    return ReconstructPath(cameFrom, current.Point);
                closedSet.Add(current.Point);

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        int x = current.Point.X + i;
                        int y = current.Point.Y + j;
                        Point p = new Point(x, y);
                        Node neighbor = new Node(p, fScore[p]);
                    }
                }
            }
        }

        // based on the pseudocode provided on Wikipedia for A* search algorithm
        // https://en.wikipedia.org/wiki/A*_search_algorithm#Pseudocode
        static Point[] ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
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
