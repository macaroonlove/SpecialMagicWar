using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class UnableToSkillEffect : Effect
    {
        public override string GetDescription()
        {
            return "��ų ��� �Ұ�";
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
