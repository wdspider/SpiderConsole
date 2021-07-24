using System;
using System.Threading.Tasks;

namespace SpiderConsole
{
    public abstract class ConsoleScreen
    {
        #region State
        public string Title { get; }
        #endregion

        public ConsoleScreen(string title)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
        }

        #region Class Methods
        public virtual Task DisplayAsync()
        {
            Console.WriteLine(Title);

            return Task.CompletedTask;
        }
        #endregion
    }
}
