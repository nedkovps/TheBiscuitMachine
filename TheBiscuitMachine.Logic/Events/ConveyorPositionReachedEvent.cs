using System;
using System.Collections.Generic;
using System.Text;

namespace TheBiscuitMachine.Logic.Events
{
    public class ConveyorPositionReachedEvent : IDomainEvent
    {
        public ConveyorPositionReachedEvent(List<string> slots)
        {
            Slots = slots;
        }

        public List<string> Slots { get; private set; }
    }
}
