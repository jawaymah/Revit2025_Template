using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace MechanicalToolsAddin
{
    /// <summary>
    /// Simple service container for managing dependencies
    /// </summary>
    public static class ServiceContainer
    {
        private static IServiceProvider? _serviceProvider;

        public static void Initialize()
        {
            var services = new ServiceCollection();

            // Register Serilog logger
            services.AddSingleton<ILogger>(Log.Logger);

            // Register services as singletons (shared across the application lifetime)
            //services.AddSingleton<IExternalEventService, ExternalEventService>();
            //services.AddSingleton<IMechDefaultsService, MechDefaultsService>();
            //services.AddSingleton<IFamilyLoaderService, FamilyLoaderService>();
            //services.AddSingleton<IFamilyFilterService, FamilyFilterService>();
            //services.AddSingleton<IConveyorNumberService, ConveyorNumberService>();

            // Build the service provider
            _serviceProvider = services.BuildServiceProvider();
        }

        public static T GetService<T>() where T : notnull
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("ServiceContainer has not been initialized. Call Initialize() first.");
            }

            return _serviceProvider.GetRequiredService<T>();
        }

        public static void Dispose()
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
            _serviceProvider = null;
        }
    }
}