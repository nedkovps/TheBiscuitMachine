using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TheBiscuitMachine.Logic.Events;

namespace TheBiscuitMachine.Web.Hubs
{
    public class BiscuitMachineUIEvents
    {
        private IHubContext<BiscuitMachineHub> _hubContext;
        private readonly IEventDispatcher _dispatcher;

        public BiscuitMachineUIEvents(IHubContext<BiscuitMachineHub> hubContext, IEventDispatcher dispatcher)
        {
            _hubContext = hubContext;
            _dispatcher = dispatcher;
        }

        public async Task MachineStateChangedEventHandler(object domainEvent)
        {
            await _hubContext.Clients.All.SendAsync("state_changed",
                JsonSerializer.Serialize(new
                {
                    ((MachineStateChangedEvent)domainEvent).State
                }));
        }

        public async Task OvenTurnedOnEventHandler(object domainEvent)
        {
            await _hubContext.Clients.All.SendAsync("oven_on");
        }

        public async Task OvenTurnedOffEventHandler(object domainEvent)
        {
            await _hubContext.Clients.All.SendAsync("oven_off");
        }

        public async Task TemperatureChangedEventHandler(object domainEvent)
        {
            await _hubContext.Clients.All.SendAsync("temperature_changed",
                JsonSerializer.Serialize(new
                {
                    ((TemperatureChangedEvent)domainEvent).Temperature
                }));
        }

        public async Task OvenHeatedEventHandler(object domainEvent)
        {
            await _hubContext.Clients.All.SendAsync("oven_heated");
        }

        public async Task MotorActivatedEventHandler(object domainEvent)
        {
            await _hubContext.Clients.All.SendAsync("motor_activated");
        }

        public async Task ConveyorMovedEventHandler(object domainEvent)
        {
            await _hubContext.Clients.All.SendAsync("conveyor_moved",
                JsonSerializer.Serialize(new
                {
                    ((ConveyorMovedEvent)domainEvent).ConveyorPositionRatio
                }));
        }

        public async Task ConveyorPositionReachedEventHandler(object domainEvent)
        {
            await _hubContext.Clients.All.SendAsync("conveyor_position_reached",
                JsonSerializer.Serialize(new
                {
                    ((ConveyorPositionReachedEvent)domainEvent).Slots
                }));
        }

        public async Task ExtruderActivatedEventHandler(object domainEvent)
        {
            await _hubContext.Clients.All.SendAsync("extruder_activated");
        }

        public async Task BiscuitExtractedEventHandler(object domainEvent)
        {
            await _hubContext.Clients.All.SendAsync("biscuit_extracted");
        }
        public async Task StamperActivatedEventHandler(object domainEvent)
        {
            await _hubContext.Clients.All.SendAsync("stamper_activated");
        }

        public async Task BiscuitStampedEventHandler(object domainEvent)
        {
            await _hubContext.Clients.All.SendAsync("biscuit_stamped");
        }

        public async Task BiscuitBakedEventHandler(object domainEvent)
        {
            await _hubContext.Clients.All.SendAsync("biscuit_baked",
                JsonSerializer.Serialize(new
                {
                    ((BiscuitBakedEvent)domainEvent).BakedBiscuitsCount
                }));
        }

        public async Task BiscuitCollectedEventHandler(object domainEvent)
        {
            await _hubContext.Clients.All.SendAsync("biscuit_collected",
                JsonSerializer.Serialize(new
                {
                    ((BiscuitCollectedEvent)domainEvent).TotalBiscuitsCollected
                }));
        }

        public async Task ProductionFinishedEventHandler(object domainEvent)
        {
            await _hubContext.Clients.All.SendAsync("production_finished");
        }

        public void RegisterEvents()
        {
            _dispatcher.RegisterHandler<MachineStateChangedEvent>(MachineStateChangedEventHandler);
            _dispatcher.RegisterHandler<OvenTurnedOnEvent>(OvenTurnedOnEventHandler);
            _dispatcher.RegisterHandler<OvenTurnedOffEvent>(OvenTurnedOffEventHandler);
            _dispatcher.RegisterHandler<TemperatureChangedEvent>(TemperatureChangedEventHandler);
            _dispatcher.RegisterHandler<OvenHeatedEvent>(OvenHeatedEventHandler);
            _dispatcher.RegisterHandler<MotorActivatedEvent>(MotorActivatedEventHandler);
            _dispatcher.RegisterHandler<ConveyorMovedEvent>(ConveyorMovedEventHandler);
            _dispatcher.RegisterHandler<ConveyorPositionReachedEvent>(ConveyorPositionReachedEventHandler);
            _dispatcher.RegisterHandler<ExtruderActivatedEvent>(ExtruderActivatedEventHandler);
            _dispatcher.RegisterHandler<BiscuitExtractedEvent>(BiscuitExtractedEventHandler);
            _dispatcher.RegisterHandler<StamperActivatedEvent>(StamperActivatedEventHandler);
            _dispatcher.RegisterHandler<BiscuitStampedEvent>(BiscuitStampedEventHandler);
            _dispatcher.RegisterHandler<BiscuitBakedEvent>(BiscuitBakedEventHandler);
            _dispatcher.RegisterHandler<BiscuitCollectedEvent>(BiscuitCollectedEventHandler);
            _dispatcher.RegisterHandler<ProductionFinishedEvent>(ProductionFinishedEventHandler);
        }
    }
}
