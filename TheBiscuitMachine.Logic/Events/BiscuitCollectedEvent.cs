using System;
using System.Collections.Generic;
using System.Text;

namespace TheBiscuitMachine.Logic.Events
{
    public class BiscuitCollectedEvent : IDomainEvent
    {
        public int TotalBiscuitsCollected { get; private set; }

        public BiscuitCollectedEvent(int total)
        {
            TotalBiscuitsCollected = total;
        }
    }
}
