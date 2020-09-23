using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TheBiscuitMachine.Logic.Events
{
    public interface IEventDispatcher
    {
        Task Dispatch(IDomainEvent domainEvent);

        void RegisterHandler<EventType>(Func<object, Task> handler) where EventType : IDomainEvent;
    }
}
