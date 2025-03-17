using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class InstantHealPointEffect : InstantPointEffect
    {
        [SerializeField] private int _repeatCount;
        [SerializeField] private bool _isTick;
        [SerializeField] private int _tickCycle;
        [SerializeField] private int _tickCount;

        [SerializeField] private List<ApplyTypeByAmountData> _applyTypeByAmountDatas = new List<ApplyTypeByAmountData>();

        public override string GetDescription()
        {
            return "��� ȸ�� (��Ÿ����)";
        }

        public int GetAmount(Unit casterUnit, Unit targetUnit)
        {
            float totalAmount = 0;

            foreach (var applyTypeByAmountData in _applyTypeByAmountDatas)
            {
                float typeValue = 0f;
                switch (applyTypeByAmountData.applyType)
                {
                    case EApplyType.Basic:
                        typeValue = 1;
                        break;
                    case EApplyType.ATK:
                        typeValue = casterUnit.GetAbility<AttackAbility>().baseATK;
                        break;
                    case EApplyType.FinalATK:
                        typeValue = casterUnit.GetAbility<AttackAbility>().finalATK;
                        break;
                    case EApplyType.CurrentHP:
                        typeValue = casterUnit.GetAbility<HealthAbility>().currentHP;
                        break;
                    case EApplyType.MAXHP:
                        typeValue = casterUnit.GetAbility<HealthAbility>().finalMaxHP;
                        break;
                    case EApplyType.Enemy_CurrentHP:
                        typeValue = targetUnit.GetAbility<HealthAbility>().currentHP;
                        break;
                    case EApplyType.Enemy_MAXHP:
                        typeValue = targetUnit.GetAbility<HealthAbility>().finalMaxHP;
                        break;
                }

                totalAmount += typeValue * applyTypeByAmountData.amount;
            }

            return (int)totalAmount;
        }

        protected override void SkillImpact(Unit casterUnit, Unit targetUnit)
        {
            int heal = GetAmount(casterUnit, targetUnit);

            Execute_RepeatCount(casterUnit, targetUnit, heal);
        }

        private void Execute_RepeatCount(Unit casterUnit, Unit targetUnit, int heal)
        {
            if (_repeatCount > 1)
            {
                for (int i = 0; i < _repeatCount; i++)
                {
                    if (targetUnit.isDie) return;

                    Execute_Tick(casterUnit, targetUnit, heal);
                }
            }
            else
            {
                Execute_Tick(casterUnit, targetUnit, heal);
            }
        }        

        private void Execute_Tick(Unit casterUnit, Unit targetUnit, int heal)
        {
            if (_isTick)
            {
                targetUnit.StartCoroutine(CoExecute_Tick(casterUnit, targetUnit, heal));
            }
            else
            {
                targetUnit.healthAbility.Healed(heal, casterUnit);
            }
        }

        private IEnumerator CoExecute_Tick(Unit casterUnit, Unit targetUnit, int heal)
        {
            var wfs = new WaitForSeconds(_tickCycle);

            for (int i = 0; i < _tickCount; i++)
            {
                if (targetUnit.isDie) yield break;

                targetUnit.healthAbility.Healed(heal, casterUnit);
                yield return wfs;
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
            GUI.Label(labelRect, "ȸ�� Ƚ��");
            _repeatCount = EditorGUI.IntField(valueRect, _repeatCount);
            if (_repeatCount <= 0) _repeatCount = 1;

            labelRect.y += 40;
            valueRect.y += 40;
            GUI.Label(labelRect, "�ֱ⸶�� ȸ�� ��� ����");
            _isTick = EditorGUI.Toggle(valueRect, _isTick);
            if (_isTick)
            {
                labelRect.y += 20;
                valueRect.y += 20;
                GUI.Label(labelRect, "�ֱ�(��)");
                _tickCycle = EditorGUI.IntField(valueRect, _tickCycle);

                labelRect.y += 20;
                valueRect.y += 20;
                GUI.Label(labelRect, "�ֱ⸶�� ȸ�� Ƚ��");
                _tickCount = EditorGUI.IntField(valueRect, _tickCount);
            }

            labelRect.y += 20;
            valueRect.y += 20;
            GUI.Label(labelRect, "���� ���");
            if (GUI.Button(valueRect, "�߰�"))
            {
                _applyTypeByAmountDatas.Add(new ApplyTypeByAmountData());
            }

            var half = (rect.width - 24) * 0.5f;
            var applyTypeRect = new Rect(labelRect.x, labelRect.y, half, 20);
            var amountRect = new Rect(half + 24, labelRect.y, half, 20);
            var deleteRect = new Rect(rect.width, valueRect.y, 20, 20);

            for (int i = 0; i < _applyTypeByAmountDatas.Count; i++)
            {
                var data = _applyTypeByAmountDatas[i];

                applyTypeRect.y += 20;
                amountRect.y += 20;
                deleteRect.y += 20;

                data.applyType = (EApplyType)EditorGUI.EnumPopup(applyTypeRect, data.applyType);
                data.amount = EditorGUI.FloatField(amountRect, data.amount);

                if (GUI.Button(deleteRect, "X"))
                {
                    _applyTypeByAmountDatas.RemoveAt(i);
                    break;
                }
            }
        }

        public override int GetNumRows()
        {
            int rowNum = base.GetNumRows();

            rowNum += 6;

            if (_isTick)
            {
                rowNum += 2;
            }

            rowNum += (int)(_applyTypeByAmountDatas.Count * 1.2f);

            return rowNum;
        }
#endif
    }
}
