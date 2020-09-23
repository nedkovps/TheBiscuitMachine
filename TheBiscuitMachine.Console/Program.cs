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
        static void Main(string[] args)
        {
            var eventDispatcher = new EventDispatcherService();
            eventDispatcher.RegisterHandler<TemperatureChangedEvent>(LogEvent);
            eventDispatcher.RegisterHandler<OvenHeatedEvent>(LogEvent);
            eventDispatcher.RegisterHandler<MotorActivatedEvent>(LogEvent);
            eventDispatcher.RegisterHandler<BiscuitExtractedEvent>(LogEvent);
            eventDispatcher.RegisterHandler<BiscuitStampedEvent>(LogEvent);
            eventDispatcher.RegisterHandler<BiscuitBakedEvent>(LogEvent);
            eventDispatcher.RegisterHandler<BiscuitCollectedEvent>(LogEvent);

            var biscuitMachineOptions = new OptionsWrapper<BiscuitMachineOptions>(new BiscuitMachineOptions() 
            { 
                MinOvenTemperature = 220, 
                MaxOvenTemperature = 240,
                MotorPulsesToReachPosition = 2,
                BiscuitBakeTimeInSeconds = 2
            });

            BiscuitMachine machine = new BiscuitMachine(biscuitMachineOptions, eventDispatcher);
            machine.TurnOn().Wait();
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
            Console.WriteLine(message);
            return Task.CompletedTask;
        }
    }
}
