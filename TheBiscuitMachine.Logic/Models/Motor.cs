using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheBiscuitMachine.Logic.Common;
using TheBiscuitMachine.Logic.Events;

namespace TheBiscuitMachine.Logic.Models
{
    internal class Motor : Element
    {
        private CancellationTokenSource _tokenSource;

        internal async Task TurnOn()
        {
            await Task.Delay(200);
            RunMotor();
        }

        internal async Task TurnOff()
        {
            _tokenSource?.Cancel();
            await Task.Delay(0);
        }

        private void RunMotor()
        {
            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;
            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        token.ThrowIfCancellationRequested();
                        RaiseEvent(new MotorActivatedEvent());
                        await Task.Delay(200);
                    }
                }
                catch (OperationCanceledException)
                {
                    _tokenSource.Dispose();
                    _tokenSource = null;
                }
            }, token);
        }
    }
}
