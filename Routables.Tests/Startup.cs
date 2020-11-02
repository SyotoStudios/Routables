using Microsoft.Extensions.DependencyInjection;

namespace Routables.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRoutingService, RoutingService>();
        }
    }
}