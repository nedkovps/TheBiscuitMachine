using System;
using System.Collections.Generic;
using System.Text;
using TheBiscuitMachine.Logic.Models;

namespace TheBiscuitMachine.Logic.Events
{
    public class MachineStateChangedEvent : IDomainEvent
    {
        public BiscuitMachineState State { get; private set; }

        public MachineStateChangedEvent(BiscuitMachineState state)
        {
            State = state;
        }
    }
}
