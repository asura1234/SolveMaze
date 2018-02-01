using System;
using System.Collections.Generic;
using System.Drawing;

namespace SolveMaze
{
    public class PNode : IComparable<PNode>
    {
        public Node node;
        public double fScore;

        public PNode(Node node, double score)
        {
            this.node = node;
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
    }

    // based on the pseudocode provided on Wikipedia for A* search algorithm
    // https://en.wikipedia.org/wiki/A*_search_algorithm#Pseudocode
    public static class AStarSearch
    {
        public static Point[] AStar(this Graph graph, Node start, Node goal)
        {
            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
            Dictionary<Node, double> gScore = new Dictionary<Node, double>();
            Dictionary<Node, double> fScore = new Dictionary<Node, double>();

            foreach (Node node in graph.Nodes)
            {
                gScore.Add(node, double.PositiveInfinity);
                fScore.Add(node, double.PositiveInfinity);
            }
            gScore[start] = 0;

            List<Node> closedSet = new List<Node>();
            PriorityQueue<PNode> openSet = new PriorityQueue<PNode>();
            openSet.Enqueue(new PNode(start, fScore[start]));

            while (!openSet.IsEmpty())
            {
                Node current = (openSet.Dequeue()).node;
                if (current.Equals(goal))
                    return ReconstructPath(cameFrom, current);
                closedSet.Add(current);

                foreach (Node neighbor in current.Neighbors)
                {
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

        private static double DistanceBetween(Point a, Point b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        private static double HeuristicCostEstimate(Node current, Node goal)
        {
            return DistanceBetween(current.Position, goal.Position);
        }

        private static Point[] ReconstructPath(Dictionary<Node, Node> cameFrom, Node current)
        {
            List<Point> totalPath = new List<Point>();
            totalPath.Add(current.Position);

            while (cameFrom.ContainsKey(current))
            {
                Node previous = current;
                current = cameFrom[current];

                Point[] path = Graph.FindPath(previous, current);
                for (int i = 0; i < path.Length; i++)
                    totalPath.Add(path[i]);

                totalPath.Add(current.Position);
            }
            Point[] results = new Point[totalPath.Count - 2];
            totalPath.CopyTo(1, results, 0, totalPath.Count - 2);
            return results;
        }

    }
}
