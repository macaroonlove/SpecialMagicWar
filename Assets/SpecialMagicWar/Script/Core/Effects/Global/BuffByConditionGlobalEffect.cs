using ScriptableObjectArchitecture;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class BuffByConditionGlobalEffect : GlobalEffect
    {
        [SerializeField] private EUnitType _unitType;
        [SerializeField] protected bool _isInfinity;
        [SerializeField] protected float _duration;
        [SerializeField] protected BuffTemplate _buff;

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

        public override void Execute()
        {
            List<Unit> units = new List<Unit>();

            if (_unitType == EUnitType.All || _unitType == EUnitType.Agent)
            {
                units.AddRange(BattleManager.Instance.GetSubSystem<AgentSystem>().GetAllAgents());
            }

            if (_unitType == EUnitType.All || _unitType == EUnitType.Enemy)
            {
                units.AddRange(BattleManager.Instance.GetSubSystem<EnemySystem>().GetAllEnemies());
            }

            if (_isInfinity)
            {
                foreach(var unit in units)
                {
                    unit.GetAbility<BuffAbility>().ApplyBuff(_buff, int.MaxValue);
                }
            }
            else
            {
                foreach (var unit in units)
                {
                    unit.GetAbility<BuffAbility>().ApplyBuff(_buff, _duration);
                }
            }
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
            GUI.Label(labelRect, "�������� ��� ����");
            _isInfinity = EditorGUI.Toggle(valueRect, _isInfinity);
            if (!_isInfinity)
            {
                labelRect.y += 20;
                valueRect.y += 20;
                GUI.Label(labelRect, "���ӽð�");
                _duration = EditorGUI.FloatField(valueRect, _duration);
            }

            labelRect.y += 20;
            valueRect.y += 20;
            GUI.Label(labelRect, "����");
            _buff = (BuffTemplate)EditorGUI.ObjectField(valueRect, _buff, typeof(BuffTemplate), false);
        }

        public override int GetNumRows()
        {
            int rowNum = 3;

            if (!_isInfinity)
            {
                rowNum++;
            }

            return rowNum;
        }
#endif
    }
}
