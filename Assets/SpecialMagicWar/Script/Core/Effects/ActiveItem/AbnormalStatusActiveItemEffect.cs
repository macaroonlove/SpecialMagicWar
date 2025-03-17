using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class AbnormalStatusActiveItemEffect : ActiveItemEffect
    {
        [SerializeField] protected bool _isInfinity;
        [SerializeField] protected float _duration;
        [SerializeField] protected AbnormalStatusTemplate _abnormalStatus;

        public override string GetDescription()
        {
            if (_isInfinity)
            {
                return "��Ƽ�� ������ ��� ���ֵ鿡�� ���� ���� �����̻� ����";
            }
            return $"��Ƽ�� ������ ��� ���ֵ鿡�� {_duration}�� �� �����̻� ����";
        }

        public override void Execute(List<Unit> targetUnits)
        {
            foreach (var targetUnit in targetUnits)
            {
                if (targetUnit == null || targetUnit.isDie) continue;

                if (_isInfinity)
                {
                    targetUnit.GetAbility<AbnormalStatusAbility>().ApplyAbnormalStatus(_abnormalStatus, int.MaxValue);
                }
                else
                {
                    targetUnit.GetAbility<AbnormalStatusAbility>().ApplyAbnormalStatus(_abnormalStatus, _duration);
                }
            }
        }

#if UNITY_EDITOR
        public override void Draw(Rect rect)
        {
            var labelRect = new Rect(rect.x, rect.y, 140, rect.height);
            var valueRect = new Rect(rect.x + 140, rect.y, rect.width - 140, rect.height);

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
            GUI.Label(labelRect, "�����̻�");
            _abnormalStatus = (AbnormalStatusTemplate)EditorGUI.ObjectField(valueRect, _abnormalStatus, typeof(AbnormalStatusTemplate), false);
        }

        public override int GetNumRows()
        {
            int rowNum = 1;

            if (!_isInfinity)
            {
                rowNum++;
            }

            return rowNum;
        }
#endif
    }
}