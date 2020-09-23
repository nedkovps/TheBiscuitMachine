using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheBiscuitMachine.Logic.Common;
using TheBiscuitMachine.Logic.Events;

namespace TheBiscuitMachine.Logic.Models
{
    internal class Extruder : Element
    {
        internal async Task Activate()
        {
            RaiseEvent(new ExtruderActivatedEvent());
            await Task.Delay(1200);
            RaiseEvent(new BiscuitExtractedEvent());
        }
    }
}
