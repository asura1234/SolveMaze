using System;
using System.Collections.Generic;
using System.Drawing;

namespace SolveMaze
{
    public class PNode : IComparable<PNode>
    {
        public Point Point;
        public double fScore;

        public PNode(Point pos, double score)
        {
            Point = pos;
            fScore = score;
        }

        public int CompareTo(PNode other)
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
    public class AStarSearch
    {
        private Bitmap maze;

        public AStarSearch(Bitmap source)
        {
            maze = source;
        }

        private Point[] AStar(Point start, Point goal)
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
            PriorityQueue<PNode> openSet = new PriorityQueue<PNode>();
            openSet.Enqueue(new PNode(start, fScore[start]));

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

                    if (maze.GetPixel(x, y).ToArgb() != Color.Black.ToArgb())
                        continue;

                    if (closedSet.Contains(neighbor))
                        continue;

                    PNode neighborPNode = new PNode(neighbor, fScore[neighbor]);
                    if (!openSet.Contains(neighborPNode))
                        openSet.Enqueue(neighborPNode);

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

        private double DistanceBetween(Point a, Point b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        private double HeuristicCostEstimate(Point current, Point goal)
        {
            return DistanceBetween(current, goal);
        }

        private Point[] ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
        {
            List<Point> totalPath = new List<Point>();
            totalPath.Add(current);

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                totalPath.Add(current);
            }
            Point[] results = new Point[totalPath.Count - 2];
            totalPath.CopyTo(1, results, 0, totalPath.Count - 2);
            return results;
        }

    }
}
