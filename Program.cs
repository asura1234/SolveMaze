using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SolveMaze
{
    class MainClass
    {
        static Bitmap maze;
        static Point start, goal;
        static Graph<Point> graph;

        static void FindStartNGoal(out Point s, out Point g)
        {
            bool start_found = false, goal_found = false;
            for (int x = 0; x < maze.Width; x++)
            {
                for (int y = 0; y < maze.Height; y++)
                {
                    Color c = maze.GetPixel(x, y);
                    if (c.ToArgb() == Color.Blue.ToArgb())
                    {
                        g = new Point(x, y);
                        goal_found = true;
                    }
                    else if (c.ToArgb() == Color.Red.ToArgb())
                    {
                        s = new Point(x, y);
                        start_found = true;
                    }

                    if (start_found && goal_found)
                        break;
                }
            }
        }

        public static void BuildGraph(Node<Point> start)
        {
            Point[] deltas = new Point[] { new Point(0, 1), new Point(0, -1), new Point(1, 0), new Point(-1, 0) };
            List<Point> directions = new List<Point>();
            foreach (Point d in deltas)
            {
                int x = start.Value.X + d.X;
                int y = start.Value.Y + d.Y;

                if (maze.GetPixel(x, y).ToArgb() != Color.Black.ToArgb())
                    directions.Add(d);
            }
            foreach (Point d in directions)
            {
                bool hasntBeen = true;
                foreach (Edge<Point> e in start.Edges)
                {
                    if (d == e.Value)
                    {
                        hasntBeen = false;
                        break;
                    }
                }
                if (hasntBeen)
                {
                    Point next = new Point(start.Value.X + d.X, start.Value.Y + d.Y);
                    BuildGraphHelper(start, next, d, 1);
                }
            }
        }

        public static void BuildGraphHelper(Node<Point> lastNode, Point direction, Point current, int distance)
        {
            


            if (maze.GetPixel(current.X, current.Y).ToArgb() == Color.Black.ToArgb())
                return;
            
            Point[] deltas = new Point[] { new Point(0, 1), new Point(0, -1), new Point(1, 0), new Point(-1, 0) };
            List<Point> directions = new List<Point>();
            foreach (Point d in deltas)
            {
                int x = current.X + d.X;
                int y = current.Y + d.Y;

                if (maze.GetPixel(x, y).ToArgb() != Color.Black.ToArgb())
                    directions.Add(d);
            }
            if (directions.Count > 1)
            {
                Node<Point> conectedNode = graph.Find(current);
                if (conectedNode == null)
                {
                    Node<Point> newNode = new Node<Point>(current);
                    graph.AddNode(newNode);
                    graph.AddUndirectedEdge(lastNode, newNode, new Edge<Point>(direction, distance));

                    foreach (Point d in directions)
                    {
                        Point next = new Point(current.X + d.X, current.Y + d.Y);
                        BuildGraphHelper(newNode, d, next, 1);
                    }
                }
                else if (!conectedNode.Equals(lastNode))
                {
                    graph.AddUndirectedEdge(lastNode, conectedNode, new Edge<Point>(direction, distance));
                    foreach (Point d in directions)
                    {
                        bool hasntBeen = true;
                        foreach (Edge<Point> e in conectedNode.Edges)
                        {
                            if (d == e.Value)
                            {
                                hasntBeen = false;
                                break;
                            }
                        }

                        if (hasntBeen)
                        {
                            Point next = new Point(current.X + d.X, current.Y + d.Y);
                            BuildGraphHelper(conectedNode, d, next, 1);
                        }
                    }
                }
            }
            else if (directions.Count == 1)
            {
                Point next = new Point(current.X + directions[0].X, current.Y + directions[0].Y);
                BuildGraphHelper(lastNode, direction, next, distance++);
            }
        }

        public static void Main(string[] args)
        {
            string sourceName = args[0];
            string destinationName = args[1];

            maze = new Bitmap(sourceName);
            FindStartNGoal(out start, out goal);
            graph = new Graph<Point>();

            Node<Point> startNode = new Node<Point>(start);
            Node<Point> goalNode = new Node<Point>(goal);

            graph.AddNode(startNode);
            graph.AddNode(goalNode);

            BuildGraph(startNode);

            Graphics graphics = Graphics.FromImage(maze);
            Pen myPen = new Pen(Color.Green, 1);

            foreach (Node<Point> node in graph.Nodes)
                graphics.FillRectangle(myPen.Brush, node.Value.X, node.Value.Y, 1, 1);
            graphics.DrawImage(maze, new Point(0, 0));

            // save solution image to destination
            maze.Save(destinationName);

            // dispose images
            maze.Dispose();

            return;
        }
    }
}
