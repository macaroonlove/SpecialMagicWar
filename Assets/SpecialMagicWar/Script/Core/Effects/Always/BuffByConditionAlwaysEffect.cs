using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class BuffByConditionAlwaysEffect : AlwaysEffect
    {
        [SerializeField] private EUnitType _unitType;
        [SerializeField] private BuffTemplate _buff;

        public override string GetDescription()
        {
            string unitLabel = "���";
            if (_unitType == EUnitType.Agent)
            {
                unitLabel = "�Ʊ�";
            }
            else if (_unitType == EUnitType.Enemy)
            {
                unitLabel = "����";
            }

            return $"{unitLabel} ���ֿ��� ���� ���� ���� ����";
        }

        public override void Execute(Unit casterUnit)
        {
            if (casterUnit == null) return;
            if (UnitCondition(casterUnit) == false) return;
            // ���� �߰�

            casterUnit.GetAbility<BuffAbility>().ApplyBuff(_buff, int.MaxValue);
        }

        private bool UnitCondition(Unit unit)
        {
            if (_unitType == EUnitType.All)
            {
                return true;
            }
            if (_unitType == EUnitType.Agent && unit is AgentUnit)
            {
                return true;
            }
            if (_unitType == EUnitType.Enemy && unit is EnemyUnit)
            {
                return true;
            }

            return false;
        }

#if UNITY_EDITOR
        public override void Draw(Rect rect)
        {
            var labelRect = new Rect(rect.x, rect.y, 140, rect.height);
            var valueRect = new Rect(rect.x + 140, rect.y, rect.width - 140, rect.height);

            GUI.Label(labelRect, "���� Ÿ��");
            _unitType = (EUnitType)EditorGUI.EnumPopup(valueRect, _unitType);

            labelRect.y += 20;
            valueRect.y += 20;
            GUI.Label(labelRect, "����");
            _buff = (BuffTemplate)EditorGUI.ObjectField(valueRect, _buff, typeof(BuffTemplate), false);
        }

        public override int GetNumRows()
        {
            return 2;
        }
#endif
    }
}