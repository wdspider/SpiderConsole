using Demo.Screens;

using SpiderConsole;

namespace Demo
{
    internal sealed class DemoConsoleProgram : ConsoleProgram
    {
        public DemoConsoleProgram() : base("Demo Console Program")
        {
            RootMenuScreen? home = new RootMenuScreen(this);

            AddScreens(new ConsoleScreen[]
            {
                home,
                new SubMenuScreen(this),
            });

            SetHomeScreen(home);
        }
    }
}
