using System;
using System.Collections.Generic;
using System.Text;

namespace TheBiscuitMachine.Logic.Events
{
    public class TemperatureChangedEvent : IDomainEvent
    {
        public int Temperature { get; private set; }

        public TemperatureChangedEvent(int temperature)
        {
            Temperature = temperature;
        }
    }
}
