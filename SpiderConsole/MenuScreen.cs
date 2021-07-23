using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpiderConsole
{
    public abstract class MenuScreen : ConsoleScreen
    {
        #region State
        protected Menu Menu { get; }
        #endregion

        public MenuScreen(string title, ConsoleProgram consoleProgram, IList<MenuOption> menuOptions) : this(title, consoleProgram, new Menu(menuOptions)) { }
        public MenuScreen(string title, ConsoleProgram consoleProgram, Menu? menu = null)
            : base(title, consoleProgram)
        {
            Menu = menu ?? new Menu();
        }

        #region Class Methods
        public override async Task Display()
        {
            await base.Display();

            await Menu.Display();
        }
        #endregion
    }
}
