using Microsoft.Extensions.DependencyInjection;

namespace CollageSystem.Utilities.Helpers
{
    public static class ServiceLocator
    {
        private static IServiceProvider? _serviceProvider;

        public static IServiceProvider ServiceProvider
        {
            get => _serviceProvider ?? throw new InvalidOperationException("ServiceProvider is not set.");
            set => _serviceProvider = value;
        }

        public static T GetService<T>() where T : class
        {
            try
            {
                // Ensure the service provider is set and try to get the service
                using var service = _serviceProvider?.CreateScope();
                var tService = service?.ServiceProvider.GetService<T>();
                return tService ?? throw new ArgumentNullException();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static T GetRequiredService<T>() where T : class
        {
            try
            {
                // Try to resolve the service; if a scoped service is required, it should be handled correctly within the DI framework
                using var scope = _serviceProvider?.CreateScope();
                var tService = scope?.ServiceProvider.GetRequiredService<T>();
                return tService ?? throw new ArgumentNullException();
            }
            catch (Exception e)
            {
                // Use proper logging instead of console (ILogger)
                Console.WriteLine(e);

                return null;
            }
        }

        // Optionally create a scope explicitly (used only when you're outside a request context)
        public static IServiceScope CreateScope()
        {
            return _serviceProvider?.CreateScope()
                   ?? throw new InvalidOperationException("ServiceProvider is not set.");
        }
    }
}