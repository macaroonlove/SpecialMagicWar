using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class UnableToHealEffect : Effect
    {
        public override string GetDescription()
        {
            return "ȸ�� �Ұ�";
        }

#if UNITY_EDITOR
        public override void Draw(Rect rect)
        {
        }

        public override int GetNumRows()
        {
            return 0;
        }
#endif
    }
}
