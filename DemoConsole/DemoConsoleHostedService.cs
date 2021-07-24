using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using SpiderConsole;

namespace DemoConsole
{
    internal sealed class DemoConsoleHostedService : ConsoleHostedService<DemoConsoleApp>
    {
        public DemoConsoleHostedService(IHostApplicationLifetime appLifetime, DemoConsoleApp demoConsoleApp, ILogger<ConsoleHostedService<DemoConsoleApp>> logger)
            : base(appLifetime, demoConsoleApp, logger)
        {
        }
    }
}
