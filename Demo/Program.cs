using System.Threading.Tasks;

namespace Demo
{
    internal sealed class Program
    {
        public static async Task Main(string[] args)
        {
            DemoConsoleProgram demoConsole = new DemoConsoleProgram();

            await demoConsole.RunAsync();
        }
    }
}
