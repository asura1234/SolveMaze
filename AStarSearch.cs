using System;
using System.Drawing;
using System.Collections.Generic;

namespace SolveMaze
{
    public class AStarSearch
    {
        private Bitmap sourceImage;
        string solutionImageName;
        int Width, Height;
        Point start, goal;

        public AStarSearch(string sourceImageName, string solutionImageName)
        {
            this.sourceImage = new Bitmap(sourceImageName);
            this.solutionImageName = solutionImageName;
            this.Width = sourceImage.Width;
            this.Height = sourceImage.Height;
            FindStartNGoal();
        }

        private class Node : IComparable<Node>
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

        public bool IsWall(Point p)
        {
            return sourceImage.GetPixel(p.X, p.Y).IsTheColorSameAs(Color.Black);
        }

        public bool IsStart(Point p)
        {
            return sourceImage.GetPixel(p.X, p.Y).IsTheColorSameAs(Color.Blue);
        }

        public bool IsGoal(Point p)
        {
            return sourceImage.GetPixel(p.X, p.Y).IsTheColorSameAs(Color.Red);
        }

        // based on the pseudocode provided on Wikipedia for A* search algorithm
        // https://en.wikipedia.org/wiki/A*_search_algorithm#Pseudocode
        public void Search()
        {
            // find the start and goal point

            var cameFrom = new PointDictioinary<Point>(Width, Height);
            var gScore = new PointDictioinary<double>(Width, Height, double.PositiveInfinity);
            var fScore = new PointDictioinary<double>(Width, Height, double.PositiveInfinity);
            gScore[start] = 0;

            var closedSet = new BinaryMatrix(Width, Height);
            var openSet = new BinaryMatrix(Width, Height);
            var queue = new PriorityQueue<Node>();

            var startNode = new Node(start, gScore[start] + HeuristicCostEstimate(start, goal));
            queue.Enqueue(startNode);
            openSet[start] = 1;

            int count = 0;
            int percentage = 0;
            int prev_percentage = 0;
            Point last = start;
            while (!queue.IsEmpty())
            {
                Point current = (queue.Dequeue()).Point;
                openSet[current] = 0;
                closedSet[current] = 1;

                count++;
                percentage = count * 100 / (Width * Height);
                if (percentage - prev_percentage > 1)
                {
                    Console.WriteLine(percentage.ToString() + "% of the pixels have been processed.");
                    prev_percentage = percentage;
                }

                if (current.Equals(goal))
                {
                    Console.WriteLine("Pathfinding complete.");
                    ReconstructPath(cameFrom, current);
                    return;
                }

                var neighbors = current.Neighbors();
                foreach (Point neighbor in neighbors)
                {
                    if (neighbor.IsOutSide(0, 0, Width, Height))
                        continue;

                    if (closedSet[neighbor] == 1)
                        continue;

                    if (IsWall(neighbor))
                        continue;

                    var neighborNode = new Node(neighbor, fScore[neighbor]);
                    if (openSet[neighbor] != 1)
                    {
                        queue.Enqueue(ref neighborNode);
                        openSet[neighbor] = 1;
                    }

                    double tentative_gScore = gScore[current] + DistanceBetween(current, neighbor);
                    if (tentative_gScore >= gScore[neighbor])
                        continue;
                    
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentative_gScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, goal);
                    neighborNode.fScore = fScore[neighbor];
                }
            }
            Console.WriteLine("Pathfinding failed.");
            return;
        }

        private double DistanceBetween(Point a, Point b)
        {
            int dx = Math.Abs(a.X - b.X);
            int dy = Math.Abs(a.Y - b.Y);

            return dx + dy;
        }

        private double HeuristicCostEstimate(Point current, Point goal)
        {
            return DistanceBetween(current, goal);
        }

        // based on the pseudocode provided on Wikipedia for A* search algorithm
        // https://en.wikipedia.org/wiki/A*_search_algorithm#Pseudocode
        private void ReconstructPath(PointDictioinary<Point> cameFrom, Point current)
        {
            Console.WriteLine("Drawing solution ...");
            sourceImage.DrawPoint(current, Color.Green);

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                sourceImage.DrawPoint(current, Color.Green);
            }

            sourceImage.Save(solutionImageName);
            Console.WriteLine("Solution is saved.");
        }

        private void FindStartNGoal()
        {
            bool start_found = false, goal_found = false;
            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    Point p = new Point(x, y);
                    if (IsWall(p))
                        continue;
                    else if (IsGoal(p))
                    {
                        if (!goal_found)
                        {
                            goal = p;
                            goal_found = true;
                        }
                    }
                    else if (IsStart(p))
                    {
                        if (!start_found)
                        {
                            start = p;
                            start_found = true;
                        }
                    }

                    if (goal_found && start_found)
                    {
                        Console.WriteLine("Found start and goal point.");
                        return;
                    }
                }
            }
        }



    }
}
