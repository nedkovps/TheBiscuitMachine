using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBiscuitMachine.Logic.Events;

namespace TheBiscuitMachine.Logic.Common
{
    public abstract class Element : IElement
    {
        private readonly ConcurrentQueue<IDomainEvent> events;

        protected Element()
        {
            events = new ConcurrentQueue<IDomainEvent>();
        }

        public IDomainEvent ConsumeEvent()
        {
            if (events.IsEmpty)
            {
                return null;
            }
            events.TryDequeue(out var domainEvent);
            return domainEvent;
        }

        public void RaiseEvent(IDomainEvent domainEvent) => events.Enqueue(domainEvent);
    }
}
