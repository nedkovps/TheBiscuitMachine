using System;
using System.Collections.Generic;
using System.Text;

namespace TheBiscuitMachine.Logic.Events
{
    public class ConveyorMovedEvent : IDomainEvent
    {
        public float ConveyorPositionRatio { get; private set; }

        public ConveyorMovedEvent(int motorPulsesSinceLastPosition, int motorPulsesToReachPosition)
        {
            ConveyorPositionRatio = (float) motorPulsesSinceLastPosition / motorPulsesToReachPosition;
        }
    }
}
