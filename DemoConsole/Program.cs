using System;
using System.Threading.Tasks;

using DemoConsole.Screens;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DemoConsole
{
    internal sealed class Program
    {
        public static Task Main(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureServices((services) =>
                {
                    _ = services.AddSpiderConsole<DemoConsoleHostedService, DemoConsoleApp, HomeScreen>(new Type[] { typeof(HomeScreen) });
                })
                .RunConsoleAsync();
    }
}
