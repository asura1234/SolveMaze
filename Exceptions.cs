// copyright Dylan Liu

using System;
namespace SolveMaze
{
    public class ImproperMazeImageException : Exception
    {
        public ImproperMazeImageException(string message) : base(message) {}
    }

    public class StartOrGoalNotFoundException : Exception
    {
        public StartOrGoalNotFoundException(string message) : base(message) {}
    }

    public class FileFormatNotSupportedException : Exception
    {
        public FileFormatNotSupportedException(string message) : base(message) {}
    }
}
