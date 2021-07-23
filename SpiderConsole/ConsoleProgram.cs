using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpiderConsole
{
    public abstract class ConsoleProgram
    {
        #region State
        public IEnumerable<string> Breadcrumbs => history.Select(x => x.Title).Reverse();
        public bool DoesPreviousScreenExist => history.Count > 1;
        public bool UseBreadcrumbHeader { get; }

        protected ConsoleScreen? CurrentScreen => history.Any() ? history.Peek() : null;
        protected ConsoleScreen? HomeScreen { get; private set; }
        protected string? Title { get; }
        #endregion

        #region Internal State
        private readonly Stack<ConsoleScreen> history;
        private readonly Dictionary<Type, ConsoleScreen> screens;
        #endregion

        public ConsoleProgram(string? title = null, IEnumerable<ConsoleScreen>? startingScreens = null, bool useBreadcrumbHeader = true)
        {
            history = new Stack<ConsoleScreen>();
            screens = new Dictionary<Type, ConsoleScreen>();
            Title = title;
            UseBreadcrumbHeader = useBreadcrumbHeader;

            if (startingScreens is not null)
            {
                AddScreens(startingScreens);
            }
        }

        #region Class Methods
        public void AddScreen(ConsoleScreen screen)
        {
            Type type = screen.GetType();

            screens[type] = screen;
        }

        public void AddScreens(IEnumerable<ConsoleScreen> screensToAdd)
        {
            foreach (ConsoleScreen screen in screensToAdd)
            {
                AddScreen(screen);
            }
        }

        public Task NavigateBack()
        {
            history.Pop();

            return DisplayCurrentScreen();
        }

        public Task NavigateHome()
        {
            if (HomeScreen is null)
            {
                throw new ArgumentNullException(nameof(HomeScreen));
            }

            history.Clear();
            history.Push(HomeScreen);

            return DisplayCurrentScreen();
        }

        public Task NavigateTo<TScreen>()
            where TScreen : ConsoleScreen
        {
            UodateCurrentScreen<TScreen>();

            return DisplayCurrentScreen();
        }

        public virtual async Task RunAsync()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Title))
                    Console.Title = Title;

                if (CurrentScreen is not null)
                {
                    await CurrentScreen.Display();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public void SetHomeScreen(ConsoleScreen screen, bool clearHistory = false)
        {
            if (clearHistory || HomeScreen is null)
            {
                history.Clear();
                history.Push(screen);
            }

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

            Output.ClearScreen();
            return CurrentScreen!.Display();
        }

        private void UodateCurrentScreen<TScreen>()
            where TScreen : ConsoleScreen
        {
            if (CurrentScreen is TScreen)
            {
                return;
            }

            if (!screens.TryGetValue(typeof(TScreen), out ConsoleScreen? nextScreen) || nextScreen is null)
            {
                throw new KeyNotFoundException($"The '{typeof(TScreen)}' screen is not registered within the console program!");
            }

            history.Push(nextScreen);
        }
        #endregion
    }
}
