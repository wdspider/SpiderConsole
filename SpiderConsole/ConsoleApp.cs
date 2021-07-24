using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpiderConsole
{
    public abstract class ConsoleApp
    {
        #region State
        protected ConsoleScreen? CurrentScreen => history.Any() ? history.Peek() : null;
        protected ConsoleScreen? HomeScreen { get; private set; }
        #endregion

        #region Internal State
        private readonly ConsoleAppOptions appOptions;
        private readonly Stack<ConsoleScreen> history;
        private readonly Dictionary<Type, ConsoleScreen> screens;
        #endregion

        public ConsoleApp(ConsoleAppOptions? appOptions = null, IEnumerable<ConsoleScreen>? startingScreens = null, ConsoleScreen? homeScreen = null)
        {
            this.appOptions = appOptions ?? new ConsoleAppOptions();
            history = new Stack<ConsoleScreen>();
            screens = new Dictionary<Type, ConsoleScreen>();

            if (startingScreens is not null)
            {
                AddScreens(startingScreens);
            }

            if (homeScreen is not null)
            {
                SetHomeScreen(homeScreen);
            }
        }

        #region Class Methods
        public void AddScreen(ConsoleScreen screenToAdd)
        {
            Type type = screenToAdd.GetType();
            screens[type] = screenToAdd;
        }

        public void AddScreens(IEnumerable<ConsoleScreen> screensToAdd)
        {
            foreach (ConsoleScreen screen in screensToAdd)
            {
                AddScreen(screen);
            }
        }

        public virtual async Task RunAsync()
        {
            if (CurrentScreen is not null)
            {
                await DisplayCurrentScreen();
            }

            Console.WriteLine();
        }

        public void SetHomeScreen(ConsoleScreen screen, bool clearHistory = true)
        {
            if (clearHistory)
            {
                history.Clear();
            }

            history.Push(screen);
            HomeScreen = screen;
        }
        #endregion

        #region Helper Methods
        private Task DisplayCurrentScreen()
        {
            if (CurrentScreen is null)
            {
                throw new ArgumentNullException(nameof(CurrentScreen));
            }

            Console.Clear();
            return CurrentScreen.DisplayAsync();
        }
        #endregion
    }
}
