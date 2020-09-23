using System;
using System.Collections.Generic;
using System.Text;

namespace TheBiscuitMachine.Logic.Configuration
{
    public class BiscuitMachineOptions
    {
        public int MinOvenTemperature { get; set; }

        public int MaxOvenTemperature { get; set; }

        public int MotorPulsesToReachPosition { get; set; }

        public int BiscuitBakeTimeInSeconds { get; set; }
    }
}
