using System.Collections.Generic;

namespace SpecialMagicWar.Core
{
    public class AttackEventUnitTrigger : UnitTrigger
    {
        public override string GetLabel()
        {
            return "기본 공격/회복 시 적용";
        }
    }
}