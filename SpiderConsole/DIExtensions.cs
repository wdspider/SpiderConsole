using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using SpiderConsole;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DIExtensions
    {
        public static IServiceCollection AddSpiderConsole<THostedService, TConsoleApp, THomeScreen>
            (this IServiceCollection services, IEnumerable<Type> screenAssemblyMarkerTypes, Action<ConsoleAppOptions>? configureOptions = null)
            where THostedService : ConsoleHostedService<TConsoleApp>
            where TConsoleApp : ConsoleApp, new()
            where THomeScreen : ConsoleScreen
            => AddSpiderConsoleServices<THostedService, TConsoleApp, THomeScreen>(services, screenAssemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly), configureOptions);

        public static IServiceCollection AddSpiderConsole<THostedService, TConsoleApp, THomeScreen>
            (this IServiceCollection services, IEnumerable<Assembly> assembliesToScan, Action<ConsoleAppOptions>? configureOptions = null)
            where THostedService : ConsoleHostedService<TConsoleApp>
            where TConsoleApp : ConsoleApp, new()
            where THomeScreen : ConsoleScreen
            => AddSpiderConsoleServices<THostedService, TConsoleApp, THomeScreen>(services, assembliesToScan, configureOptions);

        private static IServiceCollection AddSpiderConsoleServices<THostedService, TConsoleApp, THomeScreen>
            (IServiceCollection services, IEnumerable<Assembly> assembliesToScan, Action<ConsoleAppOptions>? configureOptions = null)
            where THostedService : ConsoleHostedService<TConsoleApp>
            where TConsoleApp : ConsoleApp, new()
            where THomeScreen : ConsoleScreen
        {
            OptionsBuilder<ConsoleAppOptions> optionsBuilder = services.AddOptions<ConsoleAppOptions>()
                .Configure<IConfiguration>((options, config) => config.GetSection("SpiderConsole").GetSection(nameof(ConsoleAppOptions)).Bind(options));

            if (configureOptions is not null)
            {
                optionsBuilder = optionsBuilder.Configure(configureOptions);
            }

            services.TryAddSingleton(provider => provider.GetRequiredService<IOptions<ConsoleAppOptions>>().Value);

            assembliesToScan ??= Array.Empty<Assembly>();
            IEnumerable<Type> screenTypes = assembliesToScan.Where(a => !a.IsDynamic)
                .SelectMany(a => a.DefinedTypes)
                .Where(t => !t.IsAbstract && typeof(ConsoleScreen).IsAssignableFrom(t))
                .Distinct()
                .Select(t => t.AsType());

            foreach (Type screenType in screenTypes)
            {
                services.TryAddSingleton(screenType);
            }

            return services.AddHostedService<THostedService>()
                .AddSingleton(provider =>
                {
                    IEnumerable<ConsoleScreen> screens = screenTypes.Select(t => (ConsoleScreen)provider.GetRequiredService(t));
                    ConsoleScreen homeScreen = provider.GetRequiredService<THomeScreen>();
                    ConsoleAppOptions appOptions = provider.GetRequiredService<ConsoleAppOptions>();

                    TConsoleApp? consoleApp = Activator.CreateInstance(typeof(TConsoleApp), screens, homeScreen, appOptions) as TConsoleApp;

                    return consoleApp ?? throw new ArgumentException($"Could not create an instance of {nameof(TConsoleApp)}");
                });
        }
    }
}
