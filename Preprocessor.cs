using System;
using System.Drawing;
using System.Collections.Generic;


namespace SolveMaze
{
    public class Preprocessor
    {
        static short EMPTY = 0, WALL = 1, START = 2, GOAL = 3;

        Bitmap myMap;
        uint count = 0;
        double percentage = 0;
        double prev_percentage = 0;

        public int Width, Height;
        public PointDictioinary<short> maze;
        public PointDictioinary<double> distanceMatrix;
        public Point Start;
        public Point Goal;

        public Preprocessor(Bitmap sourceImage)
        {
            this.myMap = sourceImage;
            this.Width = myMap.Width;
            this.Height = myMap.Height;
            distanceMatrix = new PointDictioinary<double>(Width, Height, double.PositiveInfinity);
            maze = new PointDictioinary<short>(Width, Height, 0);

            FingStartNGoal(out Start, out Goal);
            TraverseMap(Goal);

            Console.WriteLine("Preprocessing complete");
        }

        private void FingStartNGoal(out Point start, out Point goal)
        {
            bool found_start = false, found_goal = false;

            for (int x = 0; x < myMap.Width; x++)
            {
                for (int y = 0; y < myMap.Height; y++)
                {
                    Point p = new Point(x, y);
                    Color c = myMap.GetPixel(x, y);
                    if (c.IsTheColorSameAs(Color.Black))
                        maze[p] = WALL;
                    else
                    {
                        if (c.IsTheColorSameAs(Color.Red))
                        {
                            if (!found_goal)
                            {
                                goal = p;
                                found_goal = true;
                            }
                            distanceMatrix[p] = 0;
                            maze[p] = GOAL;
                        }
                        else if (c.IsTheColorSameAs(Color.Blue))
                        {
                            if (!found_start)
                            {
                                start = p;
                                found_start = true;
                            }
                            maze[p] = START;
                        }
                        else
                            maze[p] = EMPTY;
                    }
                }
            }
        }


        private void TraverseMap(Point start)
        {
            Queue<Point> queue = new Queue<Point>();
            BinaryMatrix openSet = new BinaryMatrix(Width, Height);
            BinaryMatrix closedSet = new BinaryMatrix(Width, Height);

            queue.Enqueue(start);
            openSet[start] = 1;

            while (queue.Count != 0)
            {
                var current = queue.Dequeue();
                openSet[current] = 0;
                closedSet[current] = 1;

                count++;
                percentage = count * 100 / (maze.Width * maze.Height);
                if (percentage - prev_percentage > 1)
                    Console.WriteLine(percentage.ToString("F") + "% of the pixels have been pre-processed.");

                var neighbors = current.Neighbors();

                foreach (Point neighbor in neighbors)
                {
                    // if it is outside the image, then stop
                    if (neighbor.IsOutside(Width, Height))
                        continue;
                    
                    // if it has been visited then stop
                    if (closedSet[neighbor] == 1)
                        continue;

                    if (maze[neighbor] == WALL)
                    {
                        distanceMatrix[neighbor] = double.PositiveInfinity;
                        continue;
                    }

                    if (maze[neighbor] == GOAL)
                    {
                        distanceMatrix[neighbor] = 1;
                        continue;
                    }

                    if (openSet[neighbor] == 0)
                        queue.Enqueue(neighbor);
                    
                    if (maze[neighbor] != GOAL)
                    {
                        // calculate the distance from the goal to here
                        var pre_distance = distanceMatrix[neighbor];
                        var new_distance = distanceMatrix[current] + 1;
                        if (new_distance < pre_distance)
                            distanceMatrix[neighbor] = new_distance;
                    }
                }
            }
        }


    }
}
