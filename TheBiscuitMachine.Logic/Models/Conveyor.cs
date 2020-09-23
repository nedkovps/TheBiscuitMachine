using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheBiscuitMachine.Logic.Common;
using TheBiscuitMachine.Logic.Events;

namespace TheBiscuitMachine.Logic.Models
{
    internal class Conveyor : Element
    {
        private int _motorPulsesToReachPosition;
        private int _motorPulsesSinceLastPosition;

        internal Conveyor(Motor motor)
        {
            Motor = motor;
            Slots = new List<Biscuit> { null, null, null, null, null, null };
        }

        internal Motor Motor { get; private set; }

        internal List<Biscuit> Slots { get; set; }

        internal int TotalBiscuitsCollected { get; private set; }

        internal void SetMotorPulsesToReachPosition(int motorPulsesToReachPosition)
        {
            _motorPulsesToReachPosition = motorPulsesToReachPosition;
            _motorPulsesSinceLastPosition = motorPulsesToReachPosition - 1;
        }

        internal void Reset()
        {
            _motorPulsesSinceLastPosition = 0;
            TotalBiscuitsCollected = 0;
            Slots = new List<Biscuit> { null, null, null, null, null, null };

        }

        internal void CollectBiscuit()
        {
            Slots[5] = Slots[5].Collect();
            TotalBiscuitsCollected++;
            RaiseEvent(new BiscuitCollectedEvent(TotalBiscuitsCollected));
        }

        internal async Task ReachNextPosition()
        {
            await Motor.TurnOn();
        }

        internal async Task Stop()
        {
            await Motor.TurnOff();
        }

        internal async Task MotorActivatedEventHandler(object domainEvent)
        {
            _motorPulsesSinceLastPosition++;
            if (_motorPulsesSinceLastPosition == _motorPulsesToReachPosition)
            {
                await Motor.TurnOff();
                RaiseEvent(new ConveyorMovedEvent(_motorPulsesSinceLastPosition, _motorPulsesToReachPosition));
                _motorPulsesSinceLastPosition = 0;
            }
            else
            {
                RaiseEvent(new ConveyorMovedEvent(_motorPulsesSinceLastPosition, _motorPulsesToReachPosition));
            }
        }
    }
}
