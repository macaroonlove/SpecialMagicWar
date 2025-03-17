using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class UnableToTargetOfAttackEffect : Effect
    {
        public override string GetDescription()
        {
            return "���� ����� ���� �ʽ��ϴ�.";
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
