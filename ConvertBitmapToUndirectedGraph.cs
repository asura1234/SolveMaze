using System;
using System.Collections.Generic;
using System.Drawing;

namespace SolveMaze
{
    public static class ConvertBitmapToUndirectedGraph
    {
        static Bitmap maze;
        static Graph graph;

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

        static void BuildGraph(Node start)
        {
            Size[] deltas = new Size[] { new Size(0, 1), new Size(0, -1), new Size(1, 0), new Size(-1, 0) };
            List<Point> nexts = new List<Point>();
            foreach (Size delta in deltas)
            {
                Point next = start.Value + delta;

                if (!maze.GetPixel(next).IsTheSameAs(Color.Black))
                    nexts.Add(next);
            }
            foreach (Point next in nexts)
            {
                BuildGraphHelper(start, next, start.Value, new Path());
            }
        }

        static void BuildGraphHelper(Node lastNode, Point current, Point previous, Path path)
        {
            Size[] allDeltas = new Size[] { new Size(0, 1), new Size(0, -1), new Size(1, 0), new Size(-1, 0) };
            List<Point> nexts = new List<Point>();
            foreach (Size delta in allDeltas)
            {
                Point next = current + delta;

                if (!IsInside(next))
                    continue;

                if (!maze.GetPixel(next).IsTheSameAs(Color.Black))
                {
                    if (next != previous)
                        nexts.Add(next);
                }
            }
            if (nexts.Count > 1)
            {
                Node currentNode = graph.Find(current);
                if (currentNode == null)
                {
                    currentNode = new Node(current);
                    graph.AddNode(currentNode);
                    graph.AddUndirectedEdge(ref lastNode, ref currentNode, path);

                    foreach (Point next in nexts)
                        BuildGraphHelper(currentNode, next, current, new Path());
                }
                else if (currentNode.Value != lastNode.Value)
                    graph.AddUndirectedEdge(ref lastNode, ref currentNode, path);
            }
            else if (nexts.Count == 1)
            {
                Point next = nexts[0];
                path.Append(next);
                BuildGraphHelper(lastNode, next, current, path);
            }
        }

        public static Graph ConvertToGraph(this Bitmap source)
        {
            maze = source;
            graph = new Graph();

            Point start, goal;
            FindStartNGoal(out start, out goal);

            Node startNode = new Node(start);
            Node goalNode = new Node(goal);

            graph.AddNode(startNode);
            graph.AddNode(goalNode);

            BuildGraph(startNode);

            // debug code
            Pen myPen = new Pen(Color.Green, 1);
            Graphics graphics = Graphics.FromImage(maze);
            foreach (Node node in graph.Nodes)
                graphics.FillRectangle(myPen.Brush, node.Value.X, node.Value.Y, 1, 1);
            graphics.DrawImage(maze, new Point(0, 0));
            maze.Save("maze0_graph.png");
            // end debug code

            maze.Dispose();
            return graph;
        }
    }
}
