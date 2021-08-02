using Microsoft.Extensions.DependencyInjection;
using System;

namespace EasyDesk.Testing.DependencyInjection
{
    public abstract class DependencyInjectionTestBase
    {
        private readonly IServiceProvider _serviceProvider;

        public DependencyInjectionTestBase()
        {
            var container = new ServiceCollection();
            ConfigureServices(container);
            _serviceProvider = container.BuildServiceProvider();
        }

        protected abstract void ConfigureServices(IServiceCollection services);

        protected T Service<T>() => _serviceProvider.GetRequiredService<T>();
    }
}
