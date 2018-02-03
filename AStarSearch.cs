using System;
using System.Drawing;

namespace SolveMaze
{
    public class AStarSearch
    {
        private Bitmap sourceImage;
        string solutionImageName;
        int Width, Height;
        Preprocessor preprocessor;
        PointDictioinary<short> maze;
        PointDictioinary<double> distanceMatrix;

        static Size[] deltas = new Size[] { new Size(0, 1), new Size(0, -1), new Size(1, 0), new Size(-1, 0) };

        public AStarSearch(string sourceImageName, string solutionImageName)
        {
            sourceImage = new Bitmap(sourceImageName);
            preprocessor = new Preprocessor(sourceImage);
            Width = preprocessor.Width;
            Height = preprocessor.Height;
            maze = preprocessor.maze;
            distanceMatrix = preprocessor.distanceMatrix;
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
            Point start = preprocessor.Start;
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
            int percentage = 0;
            int prev_percentage = 0;
            while (!queue.IsEmpty())
            {
                Point current = (queue.Dequeue()).Point;
                openSet[current] = 0;
                closedSet[current] = 1;

                count++;
                percentage = count * 100 / (maze.Width * maze.Height);
                if (percentage - prev_percentage > 1)
                    Console.WriteLine(percentage.ToString("F") + "% of the pixels have been searched.");

                if (maze[current] == 2)
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
                    fScore[neighbor] = gScore[neighbor] + distanceMatrix[neighbor];
                }
            }
            Console.WriteLine("Pathfinding failed.");
            return;
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
    }
}
