using System;
using System.Collections.Generic;
using System.Text;
using TheBiscuitMachine.Logic.Common;

namespace TheBiscuitMachine.Logic.Models
{
    public class Biscuit
    {
        public static readonly Biscuit Extracted = new Biscuit(BiscuitState.Extracted);

        private static readonly Biscuit Stamped = new Biscuit(BiscuitState.Stamped);

        private static readonly Biscuit HalfBaked = new Biscuit(BiscuitState.HalfBaked);

        private static readonly Biscuit Baked = new Biscuit(BiscuitState.Baked);

        private static readonly Biscuit Collected = new Biscuit(BiscuitState.Collected);

        private Biscuit(BiscuitState state)
        {
            State = state;
        }

        public BiscuitState State { get; private set; }

        public Biscuit Stamp()
        {
            if (State != BiscuitState.Extracted)
            {
                throw new InvalidOperationException("Biscuit has already been stamped.");
            }
            return Stamped;
        }

        public Biscuit Bake()
        {
            if (State == BiscuitState.Stamped)
            {
                return HalfBaked;
            }
            else if (State == BiscuitState.HalfBaked)
            {
                return Baked;
            }
            else
            {
                throw new InvalidOperationException("Biscuit is not ready for oven.");
            }
        }

        public Biscuit Collect()
        {
            if (State != BiscuitState.Baked)
            {
                throw new InvalidOperationException("Biscuit is not fully baked.");
            }
            return Collected;
        }
    }
}
