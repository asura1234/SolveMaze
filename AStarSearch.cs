using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace SolveMaze
{
    public class AStarSearch
    {
        private Bitmap sourceImage;
        string solutionImageName;
        int Width, Height;
        Vector2D start, goal;

        int count = 0;
        int percentage = 0;
        int prev_percentage = 0;

        public AStarSearch(string sourceImageName, string solutionImageName)
        {
            if (CheckFileFormat(sourceImageName) && CheckFileFormat(solutionImageName))
                throw new FileFormatNotSupportedException("File format not supported. This program only supports .png, .jpg and .bmp");
            
            try
            {
                this.sourceImage = new Bitmap(sourceImageName);
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            this.solutionImageName = solutionImageName;
            this.Width = sourceImage.Width;
            this.Height = sourceImage.Height;
        }

        private bool CheckFileFormat(string fileName)
        {
            return fileName.Contains(".png") || fileName.Contains(".jpg") || fileName.Contains(".bmp");
        }

        // The following code is based on the pseudocode on Wikipedia for A* search algorithm
        // https://en.wikipedia.org/wiki/A*_search_algorithm#Pseudocode
        // It's been modified by Dylan Liu for the purpose of this exercise and for better performance.
        // For example, in the peseudocode, gScore and fScore calculation is done after the neighbor is added to the openSet.
        // This requires passing a reference of a node to the openSet instead of an instance, so that its fScore can later be modified.
        // Dylan Liu moved the fScore and gScore calculation ahead of adding the neighbor to the open set to avoid passing reference, 
        // because it is uncommon and negatively affect performance to pass reference in C#.

        public bool SearchSolution(int solutionLineThickness = 1)
        {
            if (sourceImage == null)
            {
                Console.WriteLine("Source image is not loaded.");
                return false;
            }

            // find the start and goal pixel from the source image
            bool success = FindStartNGoal();
            if (!success)
            {
                Console.WriteLine("Start and goal are not found.");
                return false;
            }

            // The set of nodes already evaluated
            var closedSet = new BinaryMatrix(Width, Height);

            // The set of currently discovered nodes that are not evaluated yet.
            var openSet = new BinaryMatrix(Width, Height);

            // A priority queue to remove the node with th lowest score
            var queue = new PriorityQueue<Node>();

            // For each node, which node it can most efficiently be reached from.
            // If a node can be reached from many nodes, cameFrom will eventually contain the
            // most efficient previous step.
            var cameFrom = new Map<Vector2D>(Width, Height);

            // For each node, the cost of getting from the start node to that node.
            var gScore = new Map<double>(Width, Height, double.PositiveInfinity);
            // The cost of going from start to start is zero.
            gScore[start] = 0;

            // For each node, the total cost of getting from the start node to the goal
            // by passing by that node. That value is partly known, partly heuristic.
            var fScore = new Map<double>(Width, Height, double.PositiveInfinity);

            // Add the start into the queue and openSet
            var startNode = new Node(start, gScore[start] + HeuristicCostEstimate(start, goal));
            queue.Enqueue(startNode);
            openSet[start] = 1;

            while (!queue.IsEmpty())
            {
                #region Code for showing search progress and debug purposes
                count++;
                percentage = count * 100 / (Width * Height);
                //sourceImage.DrawPoint(current, Color.Yellow); // draw all the pixels that haven been visited
                if (percentage - prev_percentage > 1)
                {
                    Console.WriteLine(percentage.ToString() + "% of the total pixels were processed.");
                    prev_percentage = percentage;
                }
                if (count > Width * Height) // It's impossible to visit all the pixels and yet still no solution.
                {
                    Console.WriteLine("Pathfinding is terminated due to either repeat visit to the same pixels.");
                    return false;
                }
                #endregion

                Vector2D current = (queue.Dequeue()).Vector2D;
                openSet[current] = 0;
                closedSet[current] = 1;

                if (current == goal)
                {
                    Console.WriteLine("Pathfinding complete.");
                    ReconstructPath(cameFrom, current, solutionLineThickness);
                    return true;
                }

                var neighbors = current.Neighbors();
                foreach (Vector2D neighbor in neighbors)
                {
                    if (neighbor.IsOutSideBound(0, 0, Width, Height))
                        continue; // Ignore the neighbor which is out side the bounds of the image

                    if (closedSet[neighbor] == 1)
                        continue; // Ignore the neighbor which is already evaluated.

                    if (sourceImage.IsWall(neighbor))
                        continue; // Ignore the neighbor which is a wall
                    
                    double tentative_gScore = gScore[current] + Vector2D.DistanceBetween(current, neighbor); // in this case, distance between current and neight is always 1
                    if (tentative_gScore < gScore[neighbor])
                    {
                        // This path is the best until now. Record it!
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentative_gScore;
                        fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, goal);
                    }

                    // Discover a new node
                    var neighborNode = new Node(neighbor, fScore[neighbor]);
                    if (openSet[neighbor] != 1)
                    {
                        queue.Enqueue(neighborNode);
                        openSet[neighbor] = 1;
                    }
                }
            }
            Console.WriteLine("Pathfinding failed. There is no solution to this maze.");
            return false;
        }

        private double HeuristicCostEstimate(Vector2D a, Vector2D b)
        {
            int dx = Math.Abs(a.x - b.x);
            int dy = Math.Abs(a.y - b.y);

            return dx + dy;
        }

        // based on the pseudocode provided on Wikipedia for A* search algorithm
        // https://en.wikipedia.org/wiki/A*_search_algorithm#Pseudocode
        private void ReconstructPath(Map<Vector2D> cameFrom, Vector2D current, int thickness=1)
        {
            Console.WriteLine("Drawing solution ...");
            sourceImage.DrawPoint(current, Color.Green, thickness);

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                sourceImage.DrawPoint(current, Color.Green, thickness);
            }

            sourceImage.Save(solutionImageName);
            Console.WriteLine("Solution is saved.");
        }

        private bool FindStartNGoal()
        {
            bool start_found = false, goal_found = false;
            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    Vector2D p = new Vector2D(x, y);
                    if (sourceImage.IsWall(p))
                        continue;
                    else if (sourceImage.IsGoal(p))
                    {
                        if (!goal_found)
                        {
                            goal = p;
                            goal_found = true;
                        }
                    }
                    else if (sourceImage.IsStart(p))
                    {
                        if (!start_found)
                        {
                            start = p;
                            start_found = true;
                        }
                    }
                    else if (!sourceImage.IsEmptySpace(p))
                    {
                        throw new ImproperMazeImageException("This is not a proper maze image. A proper maze image only contains red, white, blue and black color.");
                    }

                    // once a start and a goal is found, terminate the process
                    if (goal_found && start_found)
                    {
                        Console.WriteLine("Found start and goal point.");
                        return true;
                    }
                }
            }

            if (!goal_found || !start_found)
                throw new StartOrGoalNotFoundException("Start and goal are not found.");
            return false; // no start or goal is found
        }



    }
}
