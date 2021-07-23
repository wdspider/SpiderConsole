using System;
using System.Threading.Tasks;

namespace SpiderConsole
{
    public delegate Task MenuOptionAsyncCallback();

    public sealed record MenuOption
    {
        #region State
        public MenuOptionAsyncCallback AsyncCallback { get; init; }
        public string Name { get; init; }
        #endregion

        public MenuOption(string name, MenuOptionAsyncCallback asyncCallback)
        {
            AsyncCallback = asyncCallback ?? throw new ArgumentNullException(nameof(asyncCallback));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        #region Class Methods
        public override string ToString() => Name;
        #endregion
    }
}
