using Microsoft.Extensions.DependencyInjection;
using System;

namespace EasyDesk.Testing.DependencyInjection
{
    public abstract class DependencyInjectionTestBase : IDisposable
    {
        private readonly IServiceScope _serviceScope;

        public DependencyInjectionTestBase()
        {
            var container = new ServiceCollection();
            ConfigureServices(container);
            var provider = container.BuildServiceProvider();
            _serviceScope = provider.CreateScope();
        }

        protected IServiceProvider ServiceProvider => _serviceScope.ServiceProvider;

        protected abstract void ConfigureServices(IServiceCollection services);

        protected T Service<T>() => ServiceProvider.GetRequiredService<T>();

        public void Dispose() => _serviceScope.Dispose();
    }
}
