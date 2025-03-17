using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class UnableToMoveEffect : Effect
    {
        public override string GetDescription()
        {
            return "�̵� �Ұ�";
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
