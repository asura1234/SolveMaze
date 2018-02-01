using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SolveMaze
{
    // credit: Scott Mitchell
    // https://msdn.microsoft.com/en-us/library/ms379574(v=vs.80).aspx

    public class NodeList : Collection<Node>
    {
        public NodeList() : base() { }

        public NodeList(int initialSize)
        {
            // Add the specified number of items
            for (int i = 0; i < initialSize; i++)
                base.Items.Add(default(Node));
        }

        public bool Contains(Point value)
        {
            if (FindByValue(value) != null)
                return true;
            return false;
        }

        public Node FindByValue(Point value)
        {
            // search the list for the value
            foreach (Node node in Items)
                if (node.Value.Equals(value))
                    return node;

            // if we reached here, we didn't find a matching node
            return null;
        }
    }

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
    }

    public class Node
    {
        private Point data;         private NodeList neighbors = null;
        private List<Path> paths;

        public Node() { }         public Node(Point data) : this(data, null) { }         public Node(Point data, NodeList neighbors)         {             this.data = data;             this.neighbors = neighbors;         }

        public Point Value
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

        public int Cost(int i)
        {
            return Paths[i].Count;
        }

        public NodeList Neighbors         {             get             {                 if (neighbors == null)                     neighbors = new NodeList();                 return neighbors;             }             set             {                 neighbors = value;             }         }

        public new List<Path> Paths
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
        private NodeList nodeSet;

        public Graph() : this(null) { }
        public Graph(NodeList nodeSet)
        {
            if (nodeSet == null)
                this.nodeSet = new NodeList();
            else
                this.nodeSet = nodeSet;
        }

        public void AddNode(Node node)
        {
            // adds a node to the graph
            nodeSet.Add(node);
        }

        public void AddNode(Point value)
        {
            // adds a node to the graph
            nodeSet.Add(new Node(value));
        }

        public void AddUndirectedEdge(ref Node from, ref Node to, Path path)
        {
            from.Neighbors.Add(to);
            from.Paths.Add(path);

            to.Neighbors.Add(from);
            to.Paths.Add(path);
        }

        public Node Find(Point value)
        {
            return nodeSet.FindByValue(value);
        }

        public bool Contains(Point value)
        {
            return nodeSet.FindByValue(value) != null;
        }

        public bool Remove(Point value)
        {
            // first remove the node from the nodeset
           Node nodeToRemove = (Node)nodeSet.FindByValue(value);
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

        public NodeList Nodes
        {
            get
            {
                return nodeSet;
            }
        }

        public int Count
        {
            get { return nodeSet.Count; }
        }
    }
}

