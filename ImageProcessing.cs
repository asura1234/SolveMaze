using System;
using System.Drawing;

namespace SolveMaze
{
    public class ImageProcessing
    {
        // if there is a wall (or black pixel) inside the block, then it cannot be traveled through
        bool HitAWall(Bitmap maze, Point center, int windowSize)
        {

            if (windowSize == 1)
                return maze.GetPixel(center).IsTheSameAs(Color.Black);

            for (int x = center.X - windowSize / 2; x <= center.X + windowSize / 2; x++)
            {
                for (int y = center.Y - windowSize / 2; y <= center.Y + windowSize / 2; y++)
                {
                    if (maze.GetPixel(x, y).IsTheSameAs(Color.Black))
                        return true;
                }
            }
            return false;
        }

        // start block is all blue and white
        bool IsStart(Bitmap maze, Point center, int windowSize)
        {
            if (windowSize == 1)
                return maze.GetPixel(center).IsTheSameAs(Color.Blue);
            
            bool result = true;
            for (int x = center.X - windowSize / 2; x <= center.X + windowSize / 2; x++)
            {
                for (int y = center.Y - windowSize / 2; y <= center.Y + windowSize / 2; y++)
                {
                    result &= (maze.GetPixel(x, y).IsTheSameAs(Color.Blue) || maze.GetPixel(x,y).IsTheSameAs(Color.White));
                    if (!result)
                        break;
                }
            }
            return result;
        }

        // goal block is all red and white
        bool IsGoal(Bitmap maze, Point center, int windowSize)
        {
            if (windowSize == 1)
                return maze.GetPixel(center).IsTheSameAs(Color.Red);

            bool result = true;
            for (int x = center.X - windowSize / 2; x <= center.X + windowSize / 2; x++)
            {
                for (int y = center.Y - windowSize / 2; y <= center.Y + windowSize / 2; y++)
                {
                    result &= (maze.GetPixel(x, y).IsTheSameAs(Color.Red) || maze.GetPixel(x, y).IsTheSameAs(Color.White));
                    if (!result)
                        break;
                }
            }
            return result;
        }
    }
}
