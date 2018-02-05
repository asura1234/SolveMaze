// copyright Dylan Liu

using System;
namespace SolveMaze
{
    public class Node : IComparable<Node>
    {
        public Vector2D Vector2D;
        public double fScore;

        public Node(Vector2D pos, double score)
        {
            Vector2D = pos;
            fScore = score;
        }

        public int CompareTo(Node other)
        {
            if (this.fScore < other.fScore)
                return -1;
            else if (this.fScore > other.fScore)
                return 1;
            else
                return 0;
        }
    }
}
