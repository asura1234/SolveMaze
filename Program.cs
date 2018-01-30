using System;
using System.Drawing;

namespace SolveMaze
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string sourceName = args[0];
            Image sourceImage = Image.FromFile(sourceName);

            string destinationName = args[1];

            Console.WriteLine("Hello World!");
        }
    }
}
