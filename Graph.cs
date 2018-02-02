using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SolveMaze
{ 
    // the graph data structure implementation is based on the following
    // Scott Mitchell
    // https://msdn.microsoft.com/en-us/library/ms379574(v=vs.80).aspx
    // But it has been HEAVILY and PURPOSELY modified for this problem

    public class Path
    {
        LinkedList<Point> list;

        public Path() 
        { 
            list = new LinkedList<Point>(); 
        }

        public Path(LinkedList<Point> list)
        {
            if (list != null)
                this.list = list;
            else
                list = new LinkedList<Point>();
        }

        public Point[] ToArray()
        {
            Point[] arr = new Point[list.Count];
            list.CopyTo(arr, 0);
            return arr;
        }

        public Path Reverse()
        {
            LinkedList<Point> newList = new LinkedList<Point>();
            LinkedListNode<Point> last = list.Last;

            while(last != null)
            {
                newList.AddLast(new LinkedListNode<Point>(last.Value));
                last = last.Previous;
            }
            return new Path(newList);
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
        public int Index = -1;

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
    }


    public class Graph
    {
        private struct Edge
        {
            Point From;
            Point To;

            public Edge (Point from, Point to)
            {
                From = from;
                To = to;
            }
        }

        private bool[] visited;
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
            node.Index = Nodes.Count;
            nodeSet.Add(node);
        }

        public bool IsConnected()
        {
            visited = new bool[Nodes.Count];
            for (int i = 0; i < Nodes.Count; i++)
                visited[i] = false;

            DepthFirstSearch(Nodes[0]);

            for (int i = 0; i < Nodes.Count; i++)
            {
                if (!visited[i])
                    return false;
            }
            return true;
        }

        private void DepthFirstSearch(Node node)
        {
            if (visited[node.Index])
                return;
            visited[node.Index] = true;
            foreach (Node neighbor in node.Neighbors)
            {
                if (!visited[neighbor.Index])
                    DepthFirstSearch(neighbor);
            }
        }

        public void RemoveUndirectedEdge(ref Node from, ref Node to)
        {
            from.Neighbors.Remove(to);
            to.Neighbors.Remove(from);

            edgeDictionary.Remove(new Edge(from.Position, to.Position));
            edgeDictionary.Remove(new Edge(to.Position, from.Position));
        }

        public void AddUndirectedEdge(ref Node from, ref Node to, Path path)
        {
            from.Neighbors.Add(to);
            edgeDictionary.Add(new Edge(from.Position, to.Position), path);

            Path reverse = path.Reverse();
            to.Neighbors.Add(from);
            edgeDictionary.Add(new Edge(to.Position, from.Position), reverse);
        }

        public Node FindNode(Point value)
        {
            return nodeSet.Find(n => n.Position == value);
        }

        public Path FindPath(Node from, Node to)
        {
            Edge edge = new Edge(from.Position, to.Position);
            if (edgeDictionary.ContainsKey(edge))
                return edgeDictionary[edge];
            
            return null;
        }

        public Point[] FindPathPoints(Node from, Node to)
        {
            Edge edge = new Edge(from.Position, to.Position);
            if (edgeDictionary.ContainsKey(edge))
                return edgeDictionary[edge].ToArray();
            
            return null;
        }

        public int FindPathCost(Node from, Node to)
        {
            Edge edge = new Edge(from.Position, to.Position);
            if (edgeDictionary.ContainsKey(edge))
                return edgeDictionary[edge].Cost;

            return -1;
        }

        public bool Contains(Point value)
        {
            return FindNode(value) != null;
        }

        public bool RemoveNode(Point value)
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

