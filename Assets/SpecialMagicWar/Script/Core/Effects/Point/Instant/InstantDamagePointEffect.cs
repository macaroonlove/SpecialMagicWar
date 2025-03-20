using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class InstantDamagePointEffect : InstantPointEffect
    {
        [SerializeField] private int _repeatCount;
        [SerializeField] private bool _isTick;
        [SerializeField] private int _tickCycle;
        [SerializeField] private int _tickCount;
        [SerializeField] private EDamageType _damageType;
        [SerializeField] private int _damage;

        [SerializeField] private bool _isHeal;
        [SerializeField] private float _healAmountByMaxHp;

        public override string GetDescription()
        {
            return "��� ������";
        }

        public int GetAmount(Unit casterUnit)
        {
            float totalAmount = _damage;

            // TODO: ���� ����

            return (int)totalAmount;
        }

        protected override void SkillImpact(Unit casterUnit, Unit targetUnit)
        {
            int damage = GetAmount(casterUnit);

            Execute_RepeatCount(casterUnit, targetUnit, damage);
        }

        private void Execute_RepeatCount(Unit casterUnit, Unit targetUnit, int damage)
        {
            if (_repeatCount > 1)
            {
                for (int i = 0; i < _repeatCount; i++)
                {
                    if (targetUnit.isDie) return;

                    Execute_Tick(casterUnit, targetUnit, damage);
                }
            }
            else
            {
                Execute_Tick(casterUnit, targetUnit, damage);
            }
        }        

        private void Execute_Tick(Unit casterUnit, Unit targetUnit, int damage)
        {
            if (_isTick)
            {
                targetUnit.StartCoroutine(CoExecute_Tick(casterUnit, targetUnit, damage));
            }
            else
            {
                Execute_DamageType(casterUnit, targetUnit, damage);
            }
        }

        private IEnumerator CoExecute_Tick(Unit casterUnit, Unit targetUnit, int damage)
        {
            var wfs = new WaitForSeconds(_tickCycle);

            for (int i = 0; i < _tickCount; i++)
            {
                if (targetUnit.isDie) yield break;

                Execute_DamageType(casterUnit, targetUnit, damage);
                yield return wfs;
            }
        }

        private void Execute_DamageType(Unit casterUnit, Unit targetUnit, int damage)
        {
            if (_damageType == EDamageType.TrueDamage)
            {
                targetUnit.GetAbility<HitAbility>().Hit(damage, casterUnit.id);
            }
            else
            {
                targetUnit.GetAbility<HitAbility>().Hit(damage, _damageType, casterUnit.id);
            }

            if (_isHeal)
            {
                int healAmount = (int)(_healAmountByMaxHp * casterUnit.healthAbility.finalMaxHP);
                casterUnit.healthAbility.Healed(healAmount, casterUnit);
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
            GUI.Label(labelRect, "���� Ƚ��");
            _repeatCount = EditorGUI.IntField(valueRect, _repeatCount);
            if (_repeatCount <= 0) _repeatCount = 1;

            labelRect.y += 40;
            valueRect.y += 40;
            GUI.Label(labelRect, "�ֱ⸶�� ���� ��� ����");
            _isTick = EditorGUI.Toggle(valueRect, _isTick);
            if (_isTick)
            {
                labelRect.y += 20;
                valueRect.y += 20;
                GUI.Label(labelRect, "�ֱ�(��)");
                _tickCycle = EditorGUI.IntField(valueRect, _tickCycle);

                labelRect.y += 20;
                valueRect.y += 20;
                GUI.Label(labelRect, "�ֱ⸶�� ���� Ƚ��");
                _tickCount = EditorGUI.IntField(valueRect, _tickCount);
            }

            labelRect.y += 40;
            valueRect.y += 40;
            GUI.Label(labelRect, "ȸ�� ��� ����");
            _isHeal = EditorGUI.Toggle(valueRect, _isHeal);
            if (_isHeal)
            {
                labelRect.y += 20;
                valueRect.y += 20;
                GUI.Label(labelRect, "�ִ� ü�� ��� ȸ����");
                _healAmountByMaxHp = EditorGUI.FloatField(valueRect, _healAmountByMaxHp);
            }

            labelRect.y += 40;
            valueRect.y += 40;
            GUI.Label(labelRect, "������ Ÿ��");
            _damageType = (EDamageType)EditorGUI.EnumPopup(valueRect, _damageType);

            labelRect.y += 40;
            valueRect.y += 40;
            GUI.Label(labelRect, "������");
            _damage = EditorGUI.IntField(valueRect, _damage);
        }

        public override int GetNumRows()
        {
            int rowNum = base.GetNumRows();

            rowNum += 10;

            if (_isTick)
            {
                rowNum += 2;
            }

            if (_isHeal)
            {
                rowNum++;
            }

            return rowNum;
        }
#endif
    }
}