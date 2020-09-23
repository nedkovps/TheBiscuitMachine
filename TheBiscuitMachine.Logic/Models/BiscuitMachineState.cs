using System;
using System.Collections.Generic;
using System.Text;

namespace TheBiscuitMachine.Logic.Models
{
    public class BiscuitMachineState
    {
        public static readonly BiscuitMachineState Initial = new BiscuitMachineState
        {
            IsOn = false,
            IsPaused = false,
            IsOvenHeated = false,
            IsProductionStarted = false,
            IsProductionFinished = false
        };

        private static readonly BiscuitMachineState On = new BiscuitMachineState
        {
            IsOn = true,
            IsPaused = false,
            IsOvenHeated = false,
            IsProductionStarted = false,
            IsProductionFinished = false
        };

        private static readonly BiscuitMachineState Paused = new BiscuitMachineState
        {
            IsOn = true,
            IsPaused = true,
            IsOvenHeated = false,
            IsProductionStarted = false,
            IsProductionFinished = false
        };

        private static readonly BiscuitMachineState OvenHeated = new BiscuitMachineState
        {
            IsOn = true,
            IsPaused = false,
            IsOvenHeated = true,
            IsProductionStarted = false,
            IsProductionFinished = false
        };

        private static readonly BiscuitMachineState OvenHeatedPaused = new BiscuitMachineState
        {
            IsOn = true,
            IsPaused = true,
            IsOvenHeated = true,
            IsProductionStarted = false,
            IsProductionFinished = false
        };

        private static readonly BiscuitMachineState ProductionStarted = new BiscuitMachineState
        {
            IsOn = true,
            IsPaused = false,
            IsOvenHeated = true,
            IsProductionStarted = true,
            IsProductionFinished = false
        };

        private static readonly BiscuitMachineState ProductionStartedPaused = new BiscuitMachineState
        {
            IsOn = true,
            IsPaused = true,
            IsOvenHeated = true,
            IsProductionStarted = true,
            IsProductionFinished = false
        };

        private static readonly BiscuitMachineState Off = new BiscuitMachineState
        {
            IsOn = false,
            IsPaused = false,
            IsOvenHeated = false,
            IsProductionStarted = false,
            IsProductionFinished = false
        };

        private static readonly BiscuitMachineState FinishingProduction = new BiscuitMachineState
        {
            IsOn = false,
            IsPaused = false,
            IsOvenHeated = true,
            IsProductionStarted = true,
            IsProductionFinished = false
        };

        private static readonly BiscuitMachineState ProductionFinished = new BiscuitMachineState
        {
            IsOn = false,
            IsPaused = false,
            IsOvenHeated = true,
            IsProductionStarted = true,
            IsProductionFinished = true
        };

        private BiscuitMachineState() { }

        public bool IsOn { get; private set; }

        public bool IsPaused { get; private set; }

        public bool IsOvenHeated { get; private set; }

        public bool IsProductionStarted { get; private set; }

        public bool IsProductionFinished { get; private set; }

        public BiscuitMachineState TurnedOn()
        {
            if (IsOn && !IsPaused)
            {
                throw new InvalidOperationException("Machine is already on.");
            }
            return On;
        }

        public BiscuitMachineState TurnedOff()
        {
            if (!IsOn)
            {
                throw new InvalidOperationException("Machine is already off.");
            }
            if (IsProductionStarted)
            {
                return FinishingProduction;
            }
            else
            {
                return Off;
            }
        }

        public BiscuitMachineState Pause()
        {
            if (!IsOn || IsPaused)
            {
                throw new InvalidOperationException("Machine is not on.");
            }
            if (!IsOvenHeated)
            {
                return Paused;
            }
            else if (!IsProductionStarted)
            {
                return OvenHeatedPaused;
            }
            else
            {
                return ProductionStartedPaused;
            }
        }

        public BiscuitMachineState Resume()
        {
            if (!IsPaused) {
                throw new InvalidOperationException("Machine is not paused.");
            }
            if (!IsOvenHeated)
            {
                return On;
            }
            else if (!IsProductionStarted)
            {
                return OvenHeated;
            }
            else
            {
                return ProductionStarted;
            }
        }

        public BiscuitMachineState HeatedOven()
        {
            if (!IsOn)
            {
                throw new InvalidOperationException("Machine is not on.");
            }
            if (IsOvenHeated || IsProductionStarted)
            {
                throw new InvalidOperationException("Oven already heated.");
            }
            return !IsPaused ? OvenHeated : OvenHeatedPaused;
        }

        public BiscuitMachineState StartedProduction()
        {
            if (!IsOn)
            {
                throw new InvalidOperationException("Machine is not on.");
            }
            if (!IsOvenHeated)
            {
                throw new InvalidOperationException("Oven is not heated.");
            }
            return ProductionStarted;
        }

        public BiscuitMachineState FinishedProduction()
        {
            if (IsOn)
            {
                throw new InvalidOperationException("Machine is not turned off.");
            }
            if (!IsProductionStarted)
            {
                throw new InvalidOperationException("Machine has not started production.");
            }
            return ProductionFinished;
        }
    }
}
