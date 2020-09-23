using System;
using System.Collections.Generic;
using System.Text;

namespace TheBiscuitMachine.Logic.Events
{
    public class BiscuitBakedEvent : IDomainEvent
    {
        public int BakedBiscuitsCount { get; private set; }

        public BiscuitBakedEvent(int count)
        {
            BakedBiscuitsCount = count;
        }
    }
}
