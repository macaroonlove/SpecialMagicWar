using System.Collections.Generic;

namespace SpecialMagicWar.Core
{
    public class AlwaysUnitTrigger : UnitTrigger
    {
        public override string GetLabel()
        {
            return "상시 적용";
        }
    }
}