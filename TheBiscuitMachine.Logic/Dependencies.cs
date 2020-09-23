using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TheBiscuitMachine.Logic.DomainServices;
using TheBiscuitMachine.Logic.Events;

namespace TheBiscuitMachine.Logic
{
    public static class Dependencies
    {
        public static IServiceCollection RegisterDomainDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IEventDispatcher, EventDispatcherService>();

            return services;
        }
    }
}
