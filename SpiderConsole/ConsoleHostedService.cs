using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SpiderConsole
{
    public abstract class ConsoleHostedService<TConsoleApp> : IHostedService
        where TConsoleApp : ConsoleApp
    {
        #region Internal State
        private readonly IHostApplicationLifetime appLifetime;
        private readonly TConsoleApp consoleApp;
        private readonly ILogger<ConsoleHostedService<TConsoleApp>> logger;
        #endregion

        public ConsoleHostedService(IHostApplicationLifetime appLifetime, TConsoleApp consoleApp, ILogger<ConsoleHostedService<TConsoleApp>> logger)
        {
            this.appLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
            this.consoleApp = consoleApp ?? throw new ArgumentNullException(nameof(consoleApp));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region IHostedService Implementation
        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogDebug($"Starting {nameof(ConsoleHostedService<TConsoleApp>)}");

            _ = appLifetime.ApplicationStarted.Register(() =>
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await consoleApp.RunAsync();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Unhandled Exception!");
                    }
                    finally
                    {
                        appLifetime.StopApplication();
                    }
                }, cancellationToken);
            });

            return Task.CompletedTask;
        }

        public virtual Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        #endregion
    }
}
