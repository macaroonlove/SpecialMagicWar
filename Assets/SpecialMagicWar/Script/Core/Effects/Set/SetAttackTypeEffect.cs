using System;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [Serializable]
    public class SetAttackTypeEffect : Effect
    {
        [SerializeField] private EAttackType _value;

        public EAttackType value => _value;

        public override string GetDescription()
        {
            return $"���� ����� {_value}���� ����";
        }

#if UNITY_EDITOR
        public override int GetNumRows()
        {
            return 2;
        }

        public override void Draw(Rect rect)
        {
            var labelRect = new Rect(rect.x, rect.y, 100, rect.height);
            var valueRect = new Rect(rect.x + 100, rect.y, rect.width - 100, rect.height);
            GUI.Label(labelRect, "Ÿ��");
            _value = (EAttackType)EditorGUI.EnumPopup(valueRect, _value);
        }
#endif
    }
}