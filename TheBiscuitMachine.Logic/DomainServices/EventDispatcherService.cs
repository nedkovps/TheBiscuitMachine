using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TheBiscuitMachine.Logic.Events;

namespace TheBiscuitMachine.Logic.DomainServices
{
    public class EventDispatcherService : IEventDispatcher
    {
        private readonly ConcurrentDictionary<Type, IList<Func<object, Task>>> Handlers
            = new ConcurrentDictionary<Type, IList<Func<object, Task>>>();

        public async Task Dispatch(IDomainEvent domainEvent)
        {
            if (Handlers.TryGetValue(domainEvent.GetType(), out var eventHandlers))
            {
                await Task.WhenAll(eventHandlers.Select(x => x(domainEvent)));
            }
        }

        public void RegisterHandler<EventType>(Func<object, Task> handler) where EventType : IDomainEvent
        {
            if (Handlers.TryGetValue(typeof(EventType), out var eventHandlers))
            {
                eventHandlers.Add(handler);
            }
            else
            {
                Handlers.TryAdd(typeof(EventType), new List<Func<object, Task>> { handler });
            }
        }
    }
}
