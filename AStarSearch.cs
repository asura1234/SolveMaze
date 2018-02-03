﻿using System;
using System.Drawing;

namespace SolveMaze
{
    public class AStarSearch
    {
        private Bitmap sourceImage;
        string solutionImageName;

        static Size[] deltas = new Size[] { new Size(0, 1), new Size(0, -1), new Size(1, 0), new Size(-1, 0) };

        public AStarSearch(string sourceImageName, string solutionImageName)
        {
            this.sourceImage = new Bitmap(sourceImageName);
            this.solutionImageName = solutionImageName;
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

        // based on the pseudocode provided on Wikipedia for A* search algorithm
        // https://en.wikipedia.org/wiki/A*_search_algorithm#Pseudocode
        public void Search()
        {
            // find the start and goal point
            Point start, goal;
            var maze = Preprocess(out start, out goal);

            var cameFrom = new PointDictioinary<Point>(maze.Width, maze.Height);
            var gScore = new PointDictioinary<double>(maze.Width, maze.Height);
            var fScore = new PointDictioinary<double>(maze.Width, maze.Height);

            for (int i = 0; i < maze.Width; i++)
            {
                for (int j = 0; j < maze.Height; j++)
                {
                    gScore[i, j] = double.PositiveInfinity;
                    fScore[i, j] = double.PositiveInfinity;
                }
            }
            gScore[start] = 0;

            var closedSet = new BinaryMatrix(maze.Width, maze.Height);
            var openSet = new BinaryMatrix(maze.Width, maze.Height);
            var queue = new PriorityQueue<Node>();

            queue.Enqueue(new Node(start, fScore[start]));
            openSet[start] = 1;

            int count = 0;
            double percentage = 0;
            double prev_percentage = 0;
            while (!queue.IsEmpty())
            {
                Point current = (queue.Dequeue()).Point;
                openSet[current] = 0;
                closedSet[current] = 1;


                count++;
                if (count % 100 == 0)
                {
                    percentage = count * 100.0 / (maze.Width * maze.Height);
                    if (percentage - prev_percentage > 1)
                        Console.WriteLine(percentage.ToString("F") + "% of the pixels have been processed.");
                }

                if (current.Equals(goal))
                {
                    Console.WriteLine("Pathfinding complete.");
                    ReconstructPath(cameFrom, current);
                    return;
                }

                foreach (Size delta in deltas)
                {
                    Point neighbor = current + delta;

                    int x = neighbor.X;
                    int y = neighbor.Y;

                    if (x < 0 || x >= maze.Width || y < 0 || y >= maze.Height)
                        continue;

                    if (closedSet[neighbor] == 1)
                        continue;

                    if (maze[x, y] == 1)
                        continue;

                    var neighborNode = new Node(neighbor, fScore[neighbor]);
                    if (openSet[neighbor] != 1)
                    {
                        queue.Enqueue(neighborNode);
                        openSet[neighbor] = 1;
                    }

                    double tentative_gScore = gScore[current] + 1;
                    if (tentative_gScore >= gScore[neighbor])
                        continue;

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentative_gScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, goal);
                }
            }
            Console.WriteLine("Pathfinding failed.");
            return;
        }

        private double DistanceBetween(Point a, Point b)
        {
            int dx = a.X - b.X;
            int dy = a.Y - b.Y;

            return Math.Sqrt(dx * dx + dy * dy);
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

        private BinaryMatrix Preprocess(out Point start, out Point goal)
        {
            var result = new BinaryMatrix(sourceImage.Width, sourceImage.Height);
            bool start_found = false, goal_found = false;
            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    Color c = sourceImage.GetPixel(x, y);
                    if (c.IsTheColorSameAs(Color.Black))
                    {
                        result[x, y] = 1;
                    }
                    else
                    {
                        result[x, y] = 0;
                        if (c.IsTheColorSameAs(Color.Blue))
                        {
                            goal = new Point(x, y);
                            goal_found = true;
                        }
                        else if (c.IsTheColorSameAs(Color.Red))
                        {
                            start = new Point(x, y);
                            start_found = true;
                        }
                    }
                    if (start_found && goal_found)
                        break;
                }
            }
            return result;
        }
    }
}
