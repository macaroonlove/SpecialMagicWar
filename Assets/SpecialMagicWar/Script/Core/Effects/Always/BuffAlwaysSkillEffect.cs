using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class BuffAlwaysEffect : AlwaysEffect
    {
        [SerializeField] private BuffTemplate _buff;

        public override string GetDescription()
        {
            return "��� ���ֿ��� ���� ���� ���� ����";
        }

        public override void Execute(Unit casterUnit)
        {
            if (casterUnit == null) return;

            casterUnit.GetAbility<BuffAbility>().ApplyBuff(_buff, int.MaxValue);
        }

#if UNITY_EDITOR
        public override void Draw(Rect rect)
        {
            var labelRect = new Rect(rect.x, rect.y, 140, rect.height);
            var valueRect = new Rect(rect.x + 140, rect.y, rect.width - 140, rect.height);

            GUI.Label(labelRect, "����");
            _buff = (BuffTemplate)EditorGUI.ObjectField(valueRect, _buff, typeof(BuffTemplate), false);
        }
#endif
    }
}