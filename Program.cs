using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SolveMaze
{
    class MainClass
    {
        static Bitmap maze;

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

            while (!openSet.IsEmpty())
            {
                Point current = (openSet.Dequeue()).Point;
                if (current.Equals(goal))
                    return ReconstructPath(cameFrom, current);
                closedSet.Add(current);

                Point[] deltas = new Point[] { new Point(0, 1), new Point(0, -1), new Point(1, 0), new Point(-1, 0) };

                foreach (Point d in deltas)
                {
                    int x = current.X + d.X;
                    int y = current.Y + d.Y;
                    Point neighbor = new Point(x, y);

                    if (maze.GetPixel(x, y).ToArgb() == Color.Black.ToArgb())
                        continue;

                    if (closedSet.Contains(neighbor))
                        continue;

                    Node neighborNode = new Node(neighbor, fScore[neighbor]);
                    if (!openSet.Contains(neighborNode))
                        openSet.Enqueue(neighborNode);

                    double tentative_gScore = gScore[current] + 1;
                    if (tentative_gScore >= gScore[neighbor])
                        continue;

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentative_gScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, goal);
                }
            }
            return new Point[0];
        }

        static double DistanceBetween(Point a, Point b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        static double HeuristicCostEstimate(Point current, Point goal)
        {
            return DistanceBetween(current, goal);
        }

        // based on the pseudocode provided on Wikipedia for A* search algorithm
        // https://en.wikipedia.org/wiki/A*_search_algorithm#Pseudocode
        static Point[] ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
        {
            List<Point> totalPath = new List<Point>();
            totalPath.Add(current);

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                totalPath.Add(current);
            }
            Point[] results = new Point[totalPath.Count - 2];
            totalPath.CopyTo(1, results, 0, totalPath.Count -2);
            return results;
        }

        static void FindStartNGoal(out Point start, out Point goal)
        {
            bool start_found = false, goal_found = false;
            for (int x = 0; x < maze.Width; x++)
            {
                for (int y = 0; y < maze.Height; y++)
                {
                    Color c = maze.GetPixel(x, y);
                    if (c.ToArgb() == Color.Blue.ToArgb())
                    {
                        goal = new Point(x, y);
                        goal_found = true;
                    }
                    else if (c.ToArgb() == Color.Red.ToArgb())
                    {
                        start = new Point(x, y);
                        start_found = true;
                    }

                    if (start_found && goal_found)
                        break;
                }
            }
        }

        public static void Main(string[] args)
        {
            string sourceName = args[0];
            string destinationName = args[1];

            // load maze image from file
            maze = new Bitmap(sourceName);

            // find the start and goal point
            Point start, goal;
            FindStartNGoal(out start, out goal);

            // draw solution on the maze
            Point[] path = AStar(start, goal);
            Graphics graph = Graphics.FromImage(maze);
            Pen myPen = new Pen(Color.Green, 1);
            for (int i = 0; i < path.Length; i++)
                graph.DrawLines(myPen, path);
            graph.DrawImage(maze, new Point(0, 0) );

            // save solution image to destination
            maze.Save(destinationName);

            // dispose images
            maze.Dispose();

            return;
        }
    }
}
