using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheBiscuitMachine.Logic.Common;
using TheBiscuitMachine.Logic.Events;

namespace TheBiscuitMachine.Logic.Models
{
    internal class Stamper : Element
    {
        internal async Task Activate()
        {
            RaiseEvent(new StamperActivatedEvent());
            await Task.Delay(800);
            RaiseEvent(new BiscuitStampedEvent());
        }
    }
}
