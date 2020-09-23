using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheBiscuitMachine.Logic.Common;
using TheBiscuitMachine.Logic.Configuration;
using TheBiscuitMachine.Logic.Events;

namespace TheBiscuitMachine.Logic.Models
{
    public class BiscuitMachine : Machine
    {
        private CancellationTokenSource _machineTokenSource;
        private readonly int _minOvenTemperature;
        private readonly int _maxOvenTemperature;
        private readonly int _motorPulsesToReachPosition;
        private readonly int _biscuitBakeTimeInSeconds;
        private BiscuitMachineState _state;

        public BiscuitMachine(IOptions<BiscuitMachineOptions> options, IEventDispatcher eventDispatcher) : base(eventDispatcher)
        {
            _minOvenTemperature = options.Value.MinOvenTemperature;
            _maxOvenTemperature = options.Value.MaxOvenTemperature;
            _motorPulsesToReachPosition = options.Value.MotorPulsesToReachPosition;
            _biscuitBakeTimeInSeconds = options.Value.BiscuitBakeTimeInSeconds;
            Conveyor.SetMotorPulsesToReachPosition(_motorPulsesToReachPosition);
            State = BiscuitMachineState.Initial;
            RegisterHandlers();
        }

        protected override void InitElements()
        {
            Motor = new Motor();
            Conveyor = new Conveyor(Motor);
            Extruder = new Extruder();
            Stamper = new Stamper();
            Oven = new Oven();
        }

        private void RegisterHandlers()
        {
            RegisterHandler<TemperatureChangedEvent>(TemperatureChangedEventHandler);
            RegisterHandler<OvenHeatedEvent>(OvenHeatedEventHandler);
            RegisterHandler<MotorActivatedEvent>(Conveyor.MotorActivatedEventHandler);
            RegisterHandler<ConveyorPositionReachedEvent>(ConveyorPositionReachedEventHandler);
            RegisterHandler<ProductionFinishedEvent>(Oven.ProductionFinishedEventHandler);
            RegisterHandler<ProductionFinishedEvent>(ProductionFinishedEventHandler);
        }

        public BiscuitMachineState State {
            get
            {
                return _state;
            }
            private set
            {
                _state = value;
                RaiseEvent(new MachineStateChangedEvent(_state));
            } 
        }

        internal Conveyor Conveyor { get; private set; }

        internal Motor Motor { get; private set; }

        internal Extruder Extruder { get; private set; }

        internal Stamper Stamper { get; private set; }

        internal Oven Oven { get; private set; }

        public override async Task TurnOn()
        {
            if (!State.IsPaused)
            {
                await StartMachine();
            }
            else
            {
                State = State.Resume();
                if (State.IsOvenHeated)
                {
                    await StartOrResumeProduction();
                }
            }
        }

        public override async Task TurnOff()
        {
            await StopMachine();
        }

        public override async Task Pause()
        {
            State = State.Pause();
            await Conveyor.Stop();
        }

        private void ResetMachine()
        {
            State = BiscuitMachineState.Initial;
            Conveyor.Reset();
            Oven.Reset();
        }

        private async Task StartMachine()
        {
            RunMachine();
            State = State.TurnedOn();
            await Task.Delay(0);
        }

        private async Task StopMachine()
        {
            if (!State.IsProductionStarted)
            {
                RaiseEvent(new ProductionFinishedEvent());
            }
            _machineTokenSource.Cancel();
            if (State.IsPaused && State.IsProductionStarted)
            {
                State = State.Resume();
                await Conveyor.ReachNextPosition();
            }
            if (State.IsProductionStarted)
            {
                State = State.TurnedOff();
            }
            await Task.Delay(0);
        }

        private void RunMachine()
        {
            _machineTokenSource = new CancellationTokenSource();
            var token = _machineTokenSource.Token;
            Task.Run(async () =>
            {
                try
                {
                    await Oven.TurnOn();
                    while (true)
                    {
                        HandleEvents();
                        if (token.IsCancellationRequested && (!State.IsProductionStarted || State.IsProductionFinished))
                        {
                            token.ThrowIfCancellationRequested();
                        }
                        await Task.Delay(100);
                    }
                }
                catch (OperationCanceledException)
                {
                    HandleEvents();
                    _machineTokenSource.Dispose();
                    _machineTokenSource = null;
                    ResetMachine();
                }
            }, token);
        }

        private async Task StartOrResumeProduction()
        {
            if (State.IsProductionStarted)
            {
                await Conveyor.ReachNextPosition();
            }
            else
            {
                State = State.StartedProduction();
                await ExecuteProductionCycle();
            }
        }

        private async Task TemperatureChangedEventHandler(object domainEvent)
        {
            if (State.IsProductionFinished)
            {
                return;
            }
            var temperature = ((TemperatureChangedEvent)domainEvent).Temperature;
            await MaintainOvenTemperature(temperature);
        }

        private async Task MaintainOvenTemperature(int temperature)
        {
            if (!State.IsOvenHeated && temperature >= _minOvenTemperature)
            {
                State = State.HeatedOven();
                Oven.RaiseEvent(new OvenHeatedEvent());
            }
            if (temperature <= _minOvenTemperature && !Oven.IsOn)
            {
                await Oven.TurnOn();
            }
            else if (temperature >= _maxOvenTemperature && Oven.IsOn)
            {
                await Oven.TurnOff();
            }
        }

        private async Task OvenHeatedEventHandler(object domainEvent)
        {
            if (!State.IsPaused)
            {
                await StartOrResumeProduction();
            }
        }

        private async Task ConveyorPositionReachedEventHandler(object domainEvent)
        {
            await ExecuteProductionCycle();
        }

        private async Task ExecuteProductionCycle()
        {
            var tasks = new List<Task>
            {
                BakeBiscuits(),
                StampBiscuit()
            };
            if (!_machineTokenSource.Token.IsCancellationRequested)
            {
                tasks.Add(ExtractBiscuit());
            }
            await Task.WhenAll(tasks);

            if (_machineTokenSource.Token.IsCancellationRequested && !Conveyor.Slots.Any(x => x != null))
            {
                RaiseEvent(new ProductionFinishedEvent());
                return;
            }

            if (!State.IsPaused)
            {
                await Conveyor.ReachNextPosition();
            }
        }

        private async Task ExtractBiscuit()
        {
            await Extruder.Activate();
            var biscuit = Biscuit.Extracted;
            Conveyor.Slots[0] = biscuit;
        }

        private async Task StampBiscuit()
        {
            if (Conveyor.Slots[1] != null)
            {
                await Stamper.Activate();
                Conveyor.Slots[1] = Conveyor.Slots[1].Stamp();
            }
        }

        private async Task BakeBiscuits()
        {
            if ((Conveyor.Slots[3] == null || Conveyor.Slots[3].State != BiscuitState.Stamped) &&
                (Conveyor.Slots[4] == null || Conveyor.Slots[4].State != BiscuitState.HalfBaked))
            {
                return;
            }

            //Time to half bake a biscuit
            await Task.Delay(1000 * _biscuitBakeTimeInSeconds / 2);
            int bakedBiscuits = 0;
            if (Conveyor.Slots[3] != null && Conveyor.Slots[3].State == BiscuitState.Stamped)
            {
                Conveyor.Slots[3] = Conveyor.Slots[3].Bake();
                bakedBiscuits++;
            }
            if (Conveyor.Slots[4] != null && Conveyor.Slots[4].State == BiscuitState.HalfBaked)
            {
                Conveyor.Slots[4] = Conveyor.Slots[4].Bake();
                bakedBiscuits++;
            }
            if (bakedBiscuits > 0)
            {
                Oven.RaiseEvent(new BiscuitBakedEvent(bakedBiscuits));
            }
        }

        private async Task ProductionFinishedEventHandler(object domainEvent)
        {
            if (State.IsProductionStarted)
            {
                State = State.FinishedProduction();
            }
            else
            {
                State = State.TurnedOff();
            }
            await Task.Delay(0);
        }
    }
}
