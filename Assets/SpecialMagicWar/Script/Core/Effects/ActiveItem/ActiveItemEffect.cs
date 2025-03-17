using System.Collections.Generic;

namespace SpecialMagicWar.Core
{
    public abstract class ActiveItemEffect : Effect
    {
        public abstract void Execute(List<Unit> targetUnits);
    }
}