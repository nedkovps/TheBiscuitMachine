using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheBiscuitMachine.Logic.Events;

namespace TheBiscuitMachine.Logic.Common
{
    public interface IMachine
    {
        IReadOnlyCollection<IElement> Elements { get; }

        Task TurnOn();

        Task TurnOff();

        Task Pause();
    }
}
