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
        private bool _isRunning = false;

        internal async Task TurnOn()
        {
            await Task.Delay(0);
            RunMotor();
        }

        internal async Task TurnOff()
        {
            if (_isRunning)
            {
                _tokenSource.Cancel();
            }
            await Task.Delay(0);
        }

        private void RunMotor()
        {
            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;
            Task.Run(async () =>
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    RaiseEvent(new MotorActivatedEvent());
                    await Task.Delay(300);
                }
            }, token);
            _isRunning = true;
        }

        internal void Reset()
        {
            if (_tokenSource != null)
            {
                _tokenSource.Dispose();
                _tokenSource = null;
            }
        }
    }
}
