using System;

namespace SpiderConsole
{
    public static class Input
    {
        public static int ReadInt(string prompt, int min, int max)
        {
            Output.WritePrompt(prompt);
            return ReadInt(min, max);
        }

        public static int ReadInt(int min, int max)
        {
            int value = ReadInt();

            while (value < min || value > max)
            {
                value = ReadInt($"Please enter an integer between {min} and {max} (inclusive):");
            }

            return value;
        }

        public static int ReadInt(string prompt)
        {
            Output.WritePrompt(prompt);
            return ReadInt();
        }

        public static int ReadInt()
        {
            int value;
            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Output.WritePrompt("Please enter an integer:");
            }

            return value;
        }
    }
}
