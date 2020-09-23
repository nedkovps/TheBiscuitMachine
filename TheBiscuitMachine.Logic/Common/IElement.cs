using System;
using System.Collections.Generic;
using System.Text;
using TheBiscuitMachine.Logic.Events;

namespace TheBiscuitMachine.Logic.Common
{
    public interface IElement
    {
        IDomainEvent ConsumeEvent();

        void RaiseEvent(IDomainEvent domainEvent);
    }
}
