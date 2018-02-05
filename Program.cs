using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

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

            try{
                string sourceName = args[0];
                string destinationName = args[1];
                var astar = new AStarSearch(sourceName, destinationName);
                astar.SearchSolution(3);
            }
            catch (ImproperMazeImageException e)
            {
                Console.WriteLine(e);
            }
            catch(FileFormatNotSupportedException e)
            {
                Console.WriteLine(e);
            }
            catch (StartOrGoalNotFoundException e)
            {
                Console.WriteLine(e);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
            }

            return;
        }
    }
}
