using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBiscuitMachine.Logic.Configuration;
using TheBiscuitMachine.Logic.DomainServices;
using TheBiscuitMachine.Logic.Events;
using TheBiscuitMachine.Logic.Models;
using Xunit;

namespace TheBiscuitMachine.Tests
{
    public class BiscuitMachineTests : BiscuitMachineTestsBase
    {
        [Fact]
        public async Task ShouldHeatOven()
        {
            //Arrange
            const int minOvenTemperature = 5;
            const int maxOvenTemperature = 15;
            var isOvenHeated = false;
            var ovenTemperatureBeforeHeatedLog = new List<int>();
            var ovenTemperatureAfterHeatedLog = new List<int>();
            var eventDispatcher = new EventDispatcherService();
            eventDispatcher.RegisterHandler<TemperatureChangedEvent>(async e => 
            {
                var temperature = ((TemperatureChangedEvent)e).Temperature;
                if (!isOvenHeated)
                {
                    ovenTemperatureBeforeHeatedLog.Add(temperature);
                }
                else
                {
                    ovenTemperatureAfterHeatedLog.Add(temperature);
                }
                await Task.Delay(0);
            });

            BiscuitMachine machine = CreateBiscuitMachine(eventDispatcher, minOvenTemperature, maxOvenTemperature, 2, 2);

            //Act
            await machine.TurnOn();
            await HeatOven(eventDispatcher);
            isOvenHeated = true;
            await Task.Run(async () =>
            {
                while (true)
                {
                    if (ovenTemperatureAfterHeatedLog.Count >= 5)
                    {
                        break;
                    }
                    await Task.Delay(200);
                }
            });
            await machine.TurnOff();

            //Assert
            Assert.True(ovenTemperatureBeforeHeatedLog.TrueForAll(x => x > 0 && x <= minOvenTemperature));
            Assert.Equal(minOvenTemperature, ovenTemperatureBeforeHeatedLog.Last());
            Assert.True(ovenTemperatureAfterHeatedLog.TrueForAll(x => x > minOvenTemperature));
        }

        [Fact]
        public async Task ShouldStartProductionAfterOvenHeated()
        {
            //Arrange
            var isOvenHeated = false;
            var extractedBiscuitsBeforeOvenHeated = 0;
            var extractedBiscuitsAfterOvenHeated = 0;
            var eventDispatcher = new EventDispatcherService();
            eventDispatcher.RegisterHandler<BiscuitExtractedEvent>(async e =>
            {
                if (!isOvenHeated)
                {
                    extractedBiscuitsBeforeOvenHeated++;
                }
                else
                {
                    extractedBiscuitsAfterOvenHeated++;
                }
                await Task.Delay(0);
            });

            BiscuitMachine machine = CreateBiscuitMachine(eventDispatcher, 40, 60, 2, 2);

            //Act
            await machine.TurnOn();
            await Task.Delay(2000);
            await HeatOven(eventDispatcher);
            isOvenHeated = true;
            await Task.Delay(2000);
            await machine.TurnOff();

            //Assert
            Assert.Equal(0, extractedBiscuitsBeforeOvenHeated);
            Assert.True(extractedBiscuitsAfterOvenHeated > 0);
        }

        [Fact]
        public async Task ShouldMaintainOvenTemperature()
        {
            //Arrange
            const int minOvenTemperature = 5;
            const int maxOvenTemperature = 15;

            var eventDispatcher = new EventDispatcherService();
            BiscuitMachine machine = CreateBiscuitMachine(eventDispatcher, minOvenTemperature, maxOvenTemperature, 2, 2);

            //Act
            await machine.TurnOn();
            await HeatOven(eventDispatcher);
            var temperatureLog = await LogOvenTemperatureChanges(eventDispatcher, 20);
            await machine.TurnOff();

            //Assert
            Assert.True(temperatureLog.TrueForAll(x => x >= minOvenTemperature && x <= maxOvenTemperature));
        }

        [Fact]
        public async Task ShouldMaintainOvenTemperatureWhenPaused()
        {
            //Arrange
            const int minOvenTemperature = 5;
            const int maxOvenTemperature = 15;

            var eventDispatcher = new EventDispatcherService();
            BiscuitMachine machine = CreateBiscuitMachine(eventDispatcher, minOvenTemperature, maxOvenTemperature, 2, 2);

            //Act
            await machine.TurnOn();
            await HeatOven(eventDispatcher);
            await machine.Pause();
            var temperatureLog = await LogOvenTemperatureChanges(eventDispatcher, 20);
            await machine.TurnOff();

            //Assert
            Assert.True(temperatureLog.TrueForAll(x => x >= minOvenTemperature && x <= maxOvenTemperature));
        }

        [Fact]
        public async Task ShouldStopConveyorMovementWhenPaused()
        {
            //Arrange
            var motorMovementsBeforePause = 0;
            var motorMovementsAfterPause = 0;
            var isPaused = false;
            var eventDispatcher = new EventDispatcherService();

            eventDispatcher.RegisterHandler<MotorActivatedEvent>(async e =>
            { 
                if (!isPaused)
                {
                    motorMovementsBeforePause++;
                }
                else
                {
                    motorMovementsAfterPause++;
                }
                await Task.Delay(0);
            });

            BiscuitMachine machine = CreateBiscuitMachine(eventDispatcher, 5, 15, 2, 2);

            //Act
            await machine.TurnOn();
            await HeatOven(eventDispatcher);
            await Task.Delay(2000);
            await machine.Pause();
            isPaused = true;
            await Task.Delay(2000);
            
            await machine.TurnOff();

            //Assert
            Assert.True(isPaused);
            Assert.True(motorMovementsBeforePause > 0);
            Assert.Equal(0, motorMovementsAfterPause);
        }

        [Fact]
        public async Task ShouldFinishProductionWhenTurnedOff()
        {
            //Arrange
            var collectedBiscuitsBeforeProductionFinished = 0;
            var collectedBiscuits = 0;

            var eventDispatcher = new EventDispatcherService();
            eventDispatcher.RegisterHandler<BiscuitCollectedEvent>(async e =>
            {
                collectedBiscuits++;
                await Task.Delay(0);
            });

            BiscuitMachine machine = CreateBiscuitMachine(eventDispatcher, 5, 15, 2, 2);

            //Act
            await machine.TurnOn();
            var extractedBiscuits = await ExtractBiscuits(eventDispatcher, 5);
            await machine.TurnOff();
            collectedBiscuitsBeforeProductionFinished = collectedBiscuits;
            await FinishProduction(eventDispatcher);

            //Assert
            Assert.True(collectedBiscuitsBeforeProductionFinished < extractedBiscuits);
            Assert.Equal(extractedBiscuits, collectedBiscuits);
        }
    }
}
