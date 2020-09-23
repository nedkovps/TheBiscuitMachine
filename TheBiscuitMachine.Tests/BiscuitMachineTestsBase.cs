using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheBiscuitMachine.Logic.Configuration;
using TheBiscuitMachine.Logic.Events;
using TheBiscuitMachine.Logic.Models;

namespace TheBiscuitMachine.Tests
{
    public class BiscuitMachineTestsBase
    {
        protected BiscuitMachine CreateBiscuitMachine(IEventDispatcher eventDispatcher,
            int minOvenTemperature, int maxOvenTemperature,
            int motorPulsesToReachPosition, int biscuitBakeTimeInSeconds)
        {
            var biscuitMachineOptions = new OptionsWrapper<BiscuitMachineOptions>(new BiscuitMachineOptions()
            {
                MinOvenTemperature = minOvenTemperature,
                MaxOvenTemperature = maxOvenTemperature,
                MotorPulsesToReachPosition = motorPulsesToReachPosition,
                BiscuitBakeTimeInSeconds = biscuitBakeTimeInSeconds
            });

            BiscuitMachine machine = new BiscuitMachine(biscuitMachineOptions, eventDispatcher);
            return machine;
        }

        protected async Task HeatOven(IEventDispatcher eventDispatcher)
        {
            bool isOvenHeated = false;
            eventDispatcher.RegisterHandler<OvenHeatedEvent>(async e =>
            {
                isOvenHeated = true;
                await Task.Delay(0);
            });
            await Task.Run(async () =>
            {
                while (true)
                {
                    if (isOvenHeated)
                    {
                        break;
                    }
                    await Task.Delay(200);
                }
            });
        }

        protected async Task<List<int>> LogOvenTemperatureChanges(IEventDispatcher eventDispatcher, int count)
        {
            List<int> log = new List<int>();
            eventDispatcher.RegisterHandler<TemperatureChangedEvent>(async e =>
            {
                var temperature = ((TemperatureChangedEvent)e).Temperature;
                log.Add(temperature);
                await Task.Delay(0);
            });
            await Task.Run(async () =>
            {
                while (true)
                {
                    if (log.Count >= count)
                    {
                        break;
                    }
                    await Task.Delay(200);
                }
            });
            return log;
        }

        protected async Task<int> ExtractBiscuits(IEventDispatcher eventDispatcher, int biscuitsCount)
        {
            var extractedBiscuits = 0;
            eventDispatcher.RegisterHandler<BiscuitExtractedEvent>(async e =>
            {
                extractedBiscuits++;
                await Task.Delay(0);
            });
            await Task.Run(async () =>
            {
                while (true)
                {
                    if (extractedBiscuits >= biscuitsCount)
                    {
                        break;
                    }
                    await Task.Delay(200);
                }
            });
            return extractedBiscuits;
        }

        protected async Task FinishProduction(IEventDispatcher eventDispatcher)
        {
            var productionFinished = false;
            eventDispatcher.RegisterHandler<ProductionFinishedEvent>(async e =>
            {
                productionFinished = true;
                await Task.Delay(0);
            });
            await Task.Run(async () =>
            {
                while (true)
                {
                    if (productionFinished)
                    {
                        break;
                    }
                    await Task.Delay(200);
                }
            });
        }
    }
}
