using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TheBiscuitMachine.Logic.Events;

namespace TheBiscuitMachine.Logic.Common
{
    public abstract class Machine : IMachine
    {
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ICollection<IElement> elements;

        protected Machine(IEventDispatcher eventDispatcher)
        {
            _eventDispatcher = eventDispatcher;
            InitElements();
            elements = GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(x => typeof(IElement).IsAssignableFrom(x.PropertyType))
                .Select(x => (IElement)x.GetValue(this))
                .ToList();
        }

        public IReadOnlyCollection<IElement> Elements => elements.ToList();

        protected abstract void InitElements();

        protected void RegisterHandler<TDomainEvent>(Func<object, Task> handler) where TDomainEvent : IDomainEvent
        {
            _eventDispatcher.RegisterHandler<TDomainEvent>(handler);
        }

        protected void HandleEvents()
        {
            Parallel.ForEach(elements, async element =>
            {
                IDomainEvent domainEvent = element.ConsumeEvent();
                while (domainEvent != null)
                {
                    await _eventDispatcher.Dispatch(domainEvent);
                    domainEvent = element.ConsumeEvent();
                }
            });
        }

        public abstract Task TurnOn();

        public abstract Task TurnOff();

        public abstract Task Pause();
    }
}
