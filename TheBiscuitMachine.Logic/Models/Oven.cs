using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheBiscuitMachine.Logic.Common;
using TheBiscuitMachine.Logic.Events;

namespace TheBiscuitMachine.Logic.Models
{
    internal class Oven : Element
    {
        private bool _isOvenMonitorRunning = false;
        private CancellationTokenSource _tokenSource;

        internal bool IsOn { get; private set; }

        internal int Temperature { get; private set; }

        internal async Task TurnOn()
        {
            if (!_isOvenMonitorRunning)
            {
                RunOvenMonitor();
            }
            IsOn = true;
            await Task.Delay(0);
            RaiseEvent(new OvenTurnedOnEvent());
        }

        internal async Task TurnOff()
        {
            IsOn = false;
            await Task.Delay(0);
            RaiseEvent(new OvenTurnedOffEvent());
        }

        internal async Task ProductionFinishedEventHandler(object domainEvent)
        {
            await TurnOff();
            _tokenSource.Cancel();
        }

        private void RunOvenMonitor()
        {
            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;
            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        if (token.IsCancellationRequested)
                        {
                            _isOvenMonitorRunning = false;
                            token.ThrowIfCancellationRequested();
                        }
                        if (IsOn)
                        {
                            Temperature++;
                            RaiseEvent(new TemperatureChangedEvent(Temperature));
                        }
                        else if (Temperature > 0)
                        {
                            Temperature--;
                            RaiseEvent(new TemperatureChangedEvent(Temperature));
                        }
                        await Task.Delay(300);
                    }
                }
                catch (OperationCanceledException)
                {
                    _tokenSource.Dispose();
                    _tokenSource = null;
                }
            }, token);
            _isOvenMonitorRunning = true;
        }

        internal void Reset()
        {
            Temperature = 0;
        }
    }
}
