using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using TheBiscuitMachine.Logic.Configuration;
using TheBiscuitMachine.Logic.DomainServices;
using TheBiscuitMachine.Logic.Events;
using TheBiscuitMachine.Logic.Models;

namespace TheBiscuitMachine.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var eventDispatcher = new EventDispatcherService();
            //eventDispatcher.RegisterHandler<TemperatureChangedEvent>(LogEvent);
            eventDispatcher.RegisterHandler<OvenHeatedEvent>(LogEvent);
            eventDispatcher.RegisterHandler<MotorActivatedEvent>(LogEvent);
            eventDispatcher.RegisterHandler<ConveyorPositionReachedEvent>(LogEvent);
            eventDispatcher.RegisterHandler<BiscuitExtractedEvent>(LogEvent);
            eventDispatcher.RegisterHandler<BiscuitStampedEvent>(LogEvent);
            eventDispatcher.RegisterHandler<BiscuitBakedEvent>(LogEvent);
            eventDispatcher.RegisterHandler<BiscuitCollectedEvent>(LogEvent);
            eventDispatcher.RegisterHandler<ProductionFinishedEvent>(LogEvent);

            var biscuitMachineOptions = new OptionsWrapper<BiscuitMachineOptions>(new BiscuitMachineOptions() 
            { 
                MinOvenTemperature = 20, 
                MaxOvenTemperature = 40,
                MotorPulsesToReachPosition = 10,
                BiscuitBakeTimeInSeconds = 3
            });

            BiscuitMachine machine = new BiscuitMachine(biscuitMachineOptions, eventDispatcher);
            await machine.TurnOn();
            await Task.Delay(10000);
            await machine.TurnOff();
            Console.ReadLine();
        }

        public static Task LogEvent(object domainEvent)
        {
            var message = string.Empty;
            if (domainEvent is TemperatureChangedEvent)
            {
                message = $"Oven temperature changed to {((TemperatureChangedEvent)domainEvent).Temperature}."; ;
            }
            else if (domainEvent is OvenHeatedEvent)
            {
                message = "Oven heated suffciently.";
            }
            else if (domainEvent is MotorActivatedEvent)
            {
                message = "Motor moved.";
            }
            else if (domainEvent is ConveyorPositionReachedEvent)
            {
                message = "Position reached.";
            }
            else if (domainEvent is BiscuitExtractedEvent)
            {
                message = "Biscuit extracted.";
            }
            else if (domainEvent is BiscuitStampedEvent)
            {
                message = "Biscuit stamped.";
            }
            else if (domainEvent is BiscuitBakedEvent)
            {
                message = "Biscuit baked.";
            }
            else if (domainEvent is BiscuitCollectedEvent)
            {
                message = $"Biscuit collected. Total collected biscuits: {((BiscuitCollectedEvent)domainEvent).TotalBiscuitsCollected}.";
            }
            else if (domainEvent is ProductionFinishedEvent)
            {
                message = "Production finised.";
            }
            Console.WriteLine(message);
            return Task.CompletedTask;
        }
    }
}
