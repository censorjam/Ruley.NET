using System;

namespace Ruley.NET.External
{
    public interface IConsoleOutput
    {
        void WriteLine(string text);
    }

    public class SystemConsole : IConsoleOutput
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
