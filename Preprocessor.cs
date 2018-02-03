using System;
using System.Drawing;


namespace SolveMaze
{
    public class Preprocessor
    {
        BinaryMatrix visited;
        static Size[] deltas = new Size[] { new Size(0, 1), new Size(0, -1), new Size(1, 0), new Size(-1, 0) };
        Bitmap myMap;
        int count = 0;
        double percentage = 0;
        double prev_percentage = 0;

        public int Width, Height;
        public PointDictioinary<short> maze;
        public PointDictioinary<uint> distanceMatrix;
        public Point Start;
        public Point Goal;

        public Preprocessor(Bitmap sourceImage)
        {
            this.myMap = sourceImage;
            this.Width = myMap.Width;
            this.Height = myMap.Height;
            visited = new BinaryMatrix(Width, Height);
            distanceMatrix = new PointDictioinary<uint>(Width, Height, uint.MaxValue);
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
                        maze[x, y] = 1;
                    else
                    {
                        if (c.IsTheColorSameAs(Color.Red))
                        {
                            if (!found_goal)
                            {
                                goal = p;
                                found_goal = true;
                            }
                            distanceMatrix[x, y] = 0;
                            maze[x, y] = 2;
                        }
                        else if (c.IsTheColorSameAs(Color.Blue))
                        {
                            if (!found_start)
                            {
                                start = p;
                                found_start = true;
                            }
                            maze[x, y] = 3;
                        }
                        else
                            maze[x, y] = 0;
                    }
                }
            }
        }

        private void TraverseMap(Point current)
        {
            // mark current as visited
            visited[current] = 1;

            foreach (Size delta in deltas)
            {
                Point neighbor = current + delta;
                // if ias been visited then stop
                if (visited[neighbor] == 1)
                    continue;

                // is outside the image
                if (!neighbor.IsInside(Width, Height))
                    continue;

                count++;
                percentage = count * 100.0 / (Width * Height);
                if (percentage - prev_percentage > 1)
                {
                    Console.WriteLine(percentage.ToString("F") + "% of the pixels have been preprocessed.");
                    prev_percentage = percentage;
                }

                // hit a wall
                if (maze[neighbor] == 1)
                {
                    visited[neighbor] = 1;
                    continue;
                }

                // it is not inside the goal, then it is always 0
                if (maze[neighbor] != 2)
                {
                    // calculate the distance from the goal to here
                    var pre_distance = distanceMatrix[neighbor];
                    var new_distance = distanceMatrix[current] + 1;
                    if (new_distance < pre_distance)
                        distanceMatrix[neighbor] = new_distance;
                }
                TraverseMap(neighbor);
            }
        }
    }
}
