using System.Collections.Generic;

namespace SpecialMagicWar.Core
{
    public interface IGetTarget
    {
        public List<Unit> GetTarget(Unit casterUnit);
    }
}