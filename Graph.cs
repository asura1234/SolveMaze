using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SolveMaze
{
    // credit: Scott Mitchell
    // https://msdn.microsoft.com/en-us/library/ms379574(v=vs.80).aspx

    public class NodeList<T> : Collection<Node<T>>
    {
        public NodeList() : base() { }

        public NodeList(int initialSize)
        {
            // Add the specified number of items
            for (int i = 0; i < initialSize; i++)
                base.Items.Add(default(Node<T>));
        }

        public Node<T> FindByValue(T value)
        {
            // search the list for the value
            foreach (Node<T> node in Items)
                if (node.Value.Equals(value))
                    return node;

            // if we reached here, we didn't find a matching node
            return null;
        }
    }

    public class Node<T>
    {
        private T data;         private NodeList<T> neighbors = null;
        private List<Edge<T>> edges;

        public Node() { }         public Node(T data) : this(data, null) { }         public Node(T data, NodeList<T> neighbors)         {             this.data = data;             this.neighbors = neighbors;         }

        public T Value
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

        public NodeList<T> Neighbors         {             get             {                 if (neighbors == null)                     return new NodeList<T>();                 return neighbors;             }             set             {                 neighbors = value;             }         }

        public new List<Edge<T>> Edges
        {
            get
            {
                if (edges == null)
                    edges = new List<Edge<T>>();

                return edges;
            }
        }
    }

    public class Edge<T>
    {
        private T data;
        private int cost;

        public Edge() {}
        public Edge(T data)
        {
            this.data = data;
            cost = 0;
        }

        public Edge(int cost)
        {
            this.cost = cost;
        }

        public Edge(T data, int cost)
        {
            this.data = data;
            this.cost = cost;
        }

        public T Value
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

        public int Cost
        {
            get
            {
                return cost;
            }
            set
            {
                cost = value;
            }
        }
    }


    public class Graph<T>
    {
        private NodeList<T> nodeSet;

        public Graph() : this(null) { }
        public Graph(NodeList<T> nodeSet)
        {
            if (nodeSet == null)
                this.nodeSet = new NodeList<T>();
            else
                this.nodeSet = nodeSet;
        }

        public void AddNode(Node<T> node)
        {
            // adds a node to the graph
            nodeSet.Add(node);
        }

        public void AddNode(T value)
        {
            // adds a node to the graph
            nodeSet.Add(new Node<T>(value));
        }

        public void AddDirectedEdge(Node<T> from, Node<T> to, Edge<T> edge)
        {
            from.Neighbors.Add(to);
            from.Edges.Add(edge);
        }

        public void AddUndirectedEdge(Node<T> from, Node<T> to, Edge<T> edge)
        {
            from.Neighbors.Add(to);
            from.Edges.Add(edge);

            to.Neighbors.Add(from);
            to.Edges.Add(edge);
        }

        public Node<T> Find(T value)
        {
            return nodeSet.FindByValue(value);
        }

        public bool Contains(T value)
        {
            return nodeSet.FindByValue(value) != null;
        }

        public bool Remove(T value)
        {
            // first remove the node from the nodeset
           Node<T> nodeToRemove = (Node<T>)nodeSet.FindByValue(value);
            if (nodeToRemove == null)
                // node wasn't found
                return false;

            // otherwise, the node was found
            nodeSet.Remove(nodeToRemove);

            // enumerate through each node in the nodeSet, removing edges to this node
            foreach (Node<T> gnode in nodeSet)
            {
                int index = gnode.Neighbors.IndexOf(nodeToRemove);
                if (index != -1)
                {
                    // remove the reference to the node and associated cost
                    gnode.Neighbors.RemoveAt(index);
                    gnode.Edges.RemoveAt(index);
                }
            }

            return true;
        }

        public NodeList<T> Nodes
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

