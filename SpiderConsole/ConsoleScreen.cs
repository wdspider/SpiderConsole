using System;
using System.Threading.Tasks;

namespace SpiderConsole
{
    public abstract class ConsoleScreen
    {
        #region State
        public string Title { get; }

        protected ConsoleProgram ConsoleProgram { get; }
        #endregion

        public ConsoleScreen(string title, ConsoleProgram consoleProgram)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            ConsoleProgram = consoleProgram ?? throw new ArgumentNullException(nameof(consoleProgram));
        }

        #region Class Methods
        public virtual Task Display()
        {
            if (ConsoleProgram.UseBreadcrumbHeader)
            {
                Output.WriteLine(string.Join(" > ", ConsoleProgram.Breadcrumbs));
            }
            else
            {
                Output.WriteLine(Title);
            }

            return Task.CompletedTask;
        }
        #endregion
    }
}
