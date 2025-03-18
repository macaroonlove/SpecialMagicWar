using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class InstantAbnormalStatusUnitEffect : InstantUnitEffect
    {
        [SerializeField] protected bool _isInfinity;
        [SerializeField] protected float _duration;
        [SerializeField] protected float _percentage = 100;
        [SerializeField] protected AbnormalStatusTemplate _abnormalStatus;

        public override string GetDescription()
        {
            return "��� �����̻�";
        }

        protected override void SkillImpact(Unit casterUnit, Unit targetUnit)
        {
            var value = Random.Range(0, 100.0f);
            if (value <= _percentage) return;

            if (_isInfinity)
            {
                targetUnit.GetAbility<AbnormalStatusAbility>().ApplyAbnormalStatus(_abnormalStatus, int.MaxValue);
            }
            else
            {
                targetUnit.GetAbility<AbnormalStatusAbility>().ApplyAbnormalStatus(_abnormalStatus, _duration);
            }
        }

#if UNITY_EDITOR
        public override void Draw(Rect rect)
        {
            base.Draw(rect);

            var labelRect = new Rect(rect.x, lastRectY, 140, rect.height);
            var valueRect = new Rect(rect.x + 140, lastRectY, rect.width - 140, rect.height);

            labelRect.y += 40;
            valueRect.y += 40;
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
            GUI.Label(labelRect, "Ȯ��");
            _percentage = EditorGUI.FloatField(valueRect, _percentage);
            labelRect.y += 20;
            valueRect.y += 20;
            GUI.Label(labelRect, "�����̻�");
            _abnormalStatus = (AbnormalStatusTemplate)EditorGUI.ObjectField(valueRect, _abnormalStatus, typeof(AbnormalStatusTemplate), false);
        }

        public override int GetNumRows()
        {
            int rowNum = base.GetNumRows();

            rowNum += 3;

            if (!_isInfinity)
            {
                rowNum++;
            }

            return rowNum;
        }
#endif
    }
}