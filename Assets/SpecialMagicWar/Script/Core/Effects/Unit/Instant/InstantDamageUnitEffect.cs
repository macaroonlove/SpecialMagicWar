using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class InstantDamageUnitEffect : InstantUnitEffect
    {
        [SerializeField] protected int _repeatCount;
        [SerializeField] protected bool _isTick;
        [SerializeField] protected int _tickCycle;
        [SerializeField] protected int _tickCount;
        [SerializeField] protected EDamageType _damageType;
        [SerializeField] protected int _damage;

        public override string GetDescription()
        {
            return "즉시 데미지";
        }

        public int GetAmount(Unit casterUnit, Unit targetUnit)
        {
            float totalAmount = _damage;

            // TODO: 강화 적용하기

            return (int)totalAmount;
        }

        protected override void SkillImpact(Unit casterUnit, Unit targetUnit)
        {
            int damage = GetAmount(casterUnit, targetUnit);

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
        }

#if UNITY_EDITOR
        public override void Draw(Rect rect)
        {
            base.Draw(rect);

            var labelRect = new Rect(rect.x, lastRectY, 140, rect.height);
            var valueRect = new Rect(rect.x + 140, lastRectY, rect.width - 140, rect.height);

            labelRect.y += 40;
            valueRect.y += 40;

            GUI.Label(labelRect, "피해 횟수");
            _repeatCount = EditorGUI.IntField(valueRect, _repeatCount);
            if (_repeatCount <= 0) _repeatCount = 1;

            labelRect.y += 40;
            valueRect.y += 40;
            GUI.Label(labelRect, "주기마다 피해 사용 여부");
            _isTick = EditorGUI.Toggle(valueRect, _isTick);
            if (_isTick)
            {
                labelRect.y += 20;
                valueRect.y += 20;
                GUI.Label(labelRect, "주기(초)");
                _tickCycle = EditorGUI.IntField(valueRect, _tickCycle);

                labelRect.y += 20;
                valueRect.y += 20;
                GUI.Label(labelRect, "주기마다 피해 횟수");
                _tickCount = EditorGUI.IntField(valueRect, _tickCount);
            }

            labelRect.y += 40;
            valueRect.y += 40;
            GUI.Label(labelRect, "데미지 타입");
            _damageType = (EDamageType)EditorGUI.EnumPopup(valueRect, _damageType);

            labelRect.y += 20;
            valueRect.y += 20;
            GUI.Label(labelRect, "데미지");
            _damage = EditorGUI.IntField(valueRect, _damage);
        }

        public override int GetNumRows()
        {
            int rowNum = base.GetNumRows();

            rowNum += 7;

            if (_isTick)
            {
                rowNum += 2;
            }

            return rowNum;
        }
#endif
    }
}