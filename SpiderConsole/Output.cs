using System;

namespace SpiderConsole
{
    public static class Output
    {
        public static void ClearScreen() => Console.Clear();

        public static void WriteLine(string line) => Console.WriteLine(line);

        public static void WritePrompt(string prompt) => Console.Write($"{prompt.Trim()} ");
    }
}
