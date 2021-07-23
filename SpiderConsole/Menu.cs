using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpiderConsole
{
    public sealed record Menu
    {
        #region Internal State
        private readonly List<MenuOption> options;
        #endregion

        public Menu(IList<MenuOption>? options = null)
        {
            this.options = new List<MenuOption>();

            if (options is not null)
            {
                AddOptions(options);
            }
        }

        #region Class Methods
        public void AddOption(string name, MenuOptionAsyncCallback asyncCallback) => AddOption(new MenuOption(name, asyncCallback));

        public void AddOption(MenuOption option) => options.Add(option);

        public void AddOptions(IList<MenuOption> optionsToAdd) => options.AddRange(optionsToAdd);

        public bool ContainsOption(string optionName) => options.FirstOrDefault(x => string.CompareOrdinal(x.Name, optionName) == 0) is not null;

        public Task Display()
        {
            if (options.Count == 0)
            {
                return Task.CompletedTask;
            }

            for (int i = 0; i < options.Count; i++)
            {
                Output.WriteLine($"{$"{i + 1}.",5} {options[i]}");
            }

            int choice = Input.ReadInt("Please choose a menu option:", min: 1, max: options.Count);

            return options[choice - 1].AsyncCallback();
        }
        #endregion
    }
}
