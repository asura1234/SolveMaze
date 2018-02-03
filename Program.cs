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
            string sourceName = args[0];
            string destinationName = args[1];

            var astar = new AStarSearch(sourceName, destinationName);
            astar.Search();
            return;
        }
    }
}
