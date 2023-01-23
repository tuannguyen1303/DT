using Microsoft.Extensions.DependencyInjection;

namespace DigitalTwin.Business.Background
{
    public abstract class ScopedProcessor : BackgroundService
    {
        protected readonly IServiceScopeFactory ServiceScopeFactory;

        protected ScopedProcessor(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Process()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            await ProcessInScope(scope.ServiceProvider);
        }

        protected abstract Task ProcessInScope(IServiceProvider serviceProvider);
    }
}

