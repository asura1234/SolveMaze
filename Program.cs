using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SolveMaze
{
    class MainClass
    {

        public static void Main(string[] args)
        {
            if (args == null || args.Length != 2)
            {
                Console.WriteLine("Improper input arguments.");
                return;
            }

            string sourceName = args[0];
            string destinationName = args[1];

            var astar = new AStarSearch(sourceName, destinationName);
            astar.SearchSolution(3);
            return;
        }
    }
}
