using System;
using System.Collections.Generic;
using System.Drawing;

namespace SolveMaze
{
    public static class ConvertBitmapToUndirectedGraph
    {
        static Bitmap maze;
        static Graph graph;
        static Size[] deltas = new Size[] { new Size(0, 1), new Size(0, -1), new Size(1, 0), new Size(-1, 0) };
        static int windowSize = 43;

        static void FindStartNGoal(out Point s, out Point g)
        {
            bool start_found = false, goal_found = false;
            for (int x = 0; x < maze.Width; x++)
            {
                for (int y = 0; y < maze.Height; y++)
                {
                    Point p = new Point(x, y);
                    Color c = maze.GetPixel(p);
                    if (c.IsTheSameAs(Color.Blue))
                    {
                        g = p;
                        goal_found = true;
                    }
                    else if (c.IsTheSameAs(Color.Red))
                    {
                        s = p;
                        start_found = true;
                    }

                    if (start_found && goal_found)
                        break;
                }
            }
        }

        static bool IsInside(Point p)
        {
            if (p.X < 0 || p.X >= maze.Width)
                return false;
            if (p.Y < 0 || p.Y >= maze.Height)
                return false;
            return true;
        }

        static void BuildGraph()
        {
            foreach (Node node in graph.Nodes.ToArray())
            {
                List<Point> nexts = new List<Point>();
                foreach (Size delta in deltas)
                {
                    Point next = node.Position + delta;

                    if (!maze.GetPixel(next).IsTheSameAs(Color.Black))
                        nexts.Add(next);
                }
                foreach (Point next in nexts)
                    BuildGraphHelper(node, next, node.Position, new Path());
            }
        }

        static void BuildGraphHelper(Node lastNode, Point current, Point previous, Path path)
        {
            
            List<Point> nexts = new List<Point>();
            foreach (Size delta in deltas)
            {
                Point next = current + delta;

                // if it goes out side the boundry, then ignore it
                if (!IsInside(next))
                    continue;

                // if it does not hit a wall
                if (!maze.GetPixel(next).IsTheSameAs(Color.Black))
                {
                    // if it is not the way it came from
                    if (next != previous)
                        nexts.Add(next);
                }
            }
            Node currentNode = graph.FindNode(current);
            // if it is not a previous node, then keep going
            if (currentNode == null)
            {
                // if it is an intersection, then add a new node and start building from the new node
                if (nexts.Count > 1)
                {
                    currentNode = new Node(current);
                    graph.AddNode(currentNode);
                    graph.AddUndirectedEdge(ref lastNode, ref currentNode, path);

                    foreach (Point next in nexts)
                        BuildGraphHelper(currentNode, next, current, new Path());
                }
                // if it is not an intersection, then keep going
                else if (nexts.Count == 1)
                {
                    Point next = nexts[0];
                    path.Append(current);
                    BuildGraphHelper(lastNode, next, current, path);
                }
                // if it is dead end, then stop
            }
            // if it is previous node
            else
            {
                // if it is a loop, then stop
                if (currentNode.Equals(lastNode))
                    return;

                Path existing = graph.FindPath(lastNode, currentNode);
                // if a path already exists
                if (existing != null)
                {
                    // if the exisitng path is longer than the new path
                    if (existing.Cost > path.Cost)
                    {
                        graph.RemoveUndirectedEdge(ref lastNode, ref currentNode);
                        graph.AddUndirectedEdge(ref lastNode, ref currentNode, path);
                    }
                }
                // if no path exists, then add a path 
                else
                    graph.AddUndirectedEdge(ref lastNode, ref currentNode, path);
            }
        }

        public static Graph ConvertToGraph(this Bitmap source, out Node startNode, out Node goalNode)
        {
            maze = source;
            graph = new Graph();

            Point start, goal;
            FindStartNGoal(out start, out goal);

            startNode = new Node(start);
            goalNode = new Node(goal);

            graph.AddNode(startNode);
            graph.AddNode(goalNode);

            BuildGraph();

            // debug code that draws every node on the graph in pink, and draws every edge in yellow
            // white space on this image indicates it is a dead end, therefore no edge travels through there
            // ******IMPORTANT******* comment out this part otherwise it will be drawn onto the solution image too

            /*Graphics graphics = Graphics.FromImage(maze);
            foreach (Node node in graph.Nodes)
            {
                graphics.DrawPoint(node.Position, Color.Pink);
                foreach (Path path in node.Paths)
                {
                    Point[] points = path.ToArray();
                    for (uint i = 0; i < points.Length; i++)
                        graphics.DrawPoint(points[i], Color.Yellow);
                }
            }
            graphics.DrawImage(maze, new Point(0, 0));
            maze.Save("maze0_graph.png");*/

            // end debug code
            return graph;
        }
    }
}
