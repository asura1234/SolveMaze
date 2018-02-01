using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SolveMaze
{
    // the graph data structure implementation is based on the following
    // Scott Mitchell
    // https://msdn.microsoft.com/en-us/library/ms379574(v=vs.80).aspx

    public class Path
    {
        LinkedList<Point> list;

        public Path()
        {
            list = new LinkedList<Point>();
        }

        public Path(Point point)
        {
            list = new LinkedList<Point>();
            list.AddLast(new LinkedListNode<Point>(point));
        }

        public Point[] ToArray()
        {
            Point[] arr = new Point[list.Count];
            list.CopyTo(arr, 0);
            return arr;
        }

        public bool IsEmpty
        {
            get { return Count == 0; }
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool Contains(Point value)
        {
            return list.Contains(value);
        }

        public Point LastPoint
        {
            get { return list.Last.Value; }
        }

        public Point FirstPoint
        {
            get { return list.First.Value; }
        }

        public void Append(Point point)
        {
            list.AddLast(new LinkedListNode<Point>(point));
        }

        public int Cost
        {
            get { return list.Count; }
        }
    }

    public class Node
    {
        private Point data;         private List<Node> neighbors = null;
        private List<Path> paths;

        public Node() { }         public Node(Point data) : this(data, null) { }         public Node(Point data, List<Node> neighbors)         {             this.data = data;             this.neighbors = neighbors;         }

        public bool Equals(Node other)
        {
            return data == other.data;
        }

        public Point Position
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        public List<Node> Neighbors         {             get             {                 if (neighbors == null)                     neighbors = new List<Node>();                 return neighbors;             }             set             {                 neighbors = value;             }         }

        public List<Path> Paths
        {
            get
            {
                if (paths == null)
                    paths = new List<Path>();
                return paths;
            }
        }
    }


    public class Graph
    {
        private struct Edge
        {
            Node From;
            Node To;

            public Edge (Node from, Node to)
            {
                From = from;
                To = to;
            }
        }

        private Dictionary<Edge, Path> edgeDictionary;
        private List<Node> nodeSet;
        public Graph() : this(null) { }
        public Graph(List<Node> nodeSet)
        {
            if (nodeSet == null)
                this.nodeSet = new List<Node>();
            else
                this.nodeSet = nodeSet;
            edgeDictionary = new Dictionary<Edge, Path>();
        }

        public void AddNode(Node node)
        {
            // adds a node to the graph
            nodeSet.Add(node);
        }

        public void AddUndirectedEdge(ref Node from, ref Node to, Path path)
        {
            from.Neighbors.Add(to);
            from.Paths.Add(path);

            to.Neighbors.Add(from);
            to.Paths.Add(path);

            Edge edge = new Edge(from, to);
            edgeDictionary.Add(edge, path);
        }

        public Node FindNode(Point value)
        {
            return nodeSet.Find(n => n.Position == value);
        }

        public Point[] FindPath(Node from, Node to)
        {
            Edge edge = new Edge(from, to);
            Path path = edgeDictionary[edge];

            if (path != null)
                return path.ToArray();
            
            return null;
        }

        public int FindPathCost(Node from, Node to)
        {
            Edge edge = new Edge(from, to);
            Path path = edgeDictionary[edge];

            if (path != null)
                return path.Cost;
            
            return -1;
        }

        public bool Contains(Point value)
        {
            return FindNode(value) != null;
        }

        public bool Remove(Point value)
        {
            // first remove the node from the nodeset
           Node nodeToRemove = FindNode(value);
            if (nodeToRemove == null)
                // node wasn't found
                return false;

            // otherwise, the node was found
            nodeSet.Remove(nodeToRemove);

            // enumerate through each node in the nodeSet, removing paths to this node
            foreach (Node gnode in nodeSet)
            {
                int index = gnode.Neighbors.IndexOf(nodeToRemove);
                if (index != -1)
                {
                    // remove the reference to the node and associated cost
                    gnode.Neighbors.RemoveAt(index);
                }
            }

            return true;
        }

        public List<Node> Nodes
        {
            get
            {
                return nodeSet;
            }
        }
    }
}

