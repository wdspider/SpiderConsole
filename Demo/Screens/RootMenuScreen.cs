using SpiderConsole;

namespace Demo.Screens
{
    internal sealed class RootMenuScreen : MenuScreen
    {
        public RootMenuScreen(DemoConsoleProgram consoleProgram) : base("Root Menu", consoleProgram)
        {
            Menu.AddOptions(new[]
            {
                new MenuOption("Sub Menu", () => consoleProgram.NavigateTo<SubMenuScreen>()),
            });
        }
    }
}
