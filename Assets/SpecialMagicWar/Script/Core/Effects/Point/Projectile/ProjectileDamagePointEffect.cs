using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class ProjectileDamagePointEffect : ProjectilePointEffect
    {
        [SerializeField] private int _repeatCount;
        [SerializeField] private bool _isTick;
        [SerializeField] private int _tickCycle;
        [SerializeField] private int _tickCount;
        [SerializeField] private EDamageType _damageType;
        [SerializeField] private int _damage;

        [SerializeField] private bool _isAbnormalStatus;
        [SerializeField] private bool _isInfinity;
        [SerializeField] private float _duration;
        [SerializeField] private AbnormalStatusTemplate _abnormalStatus;

        public override string GetDescription()
        {
            return "투사체 데미지";
        }

        public int GetAmount(Unit casterUnit)
        {
            float totalAmount = _damage;

            var buffAbility = casterUnit.GetAbility<BuffAbility>();

            float result = 1;

            switch (_spellType)
            {
                case ESpellType.Land:
                    foreach (var effect in buffAbility.LandATKIncreaseDataEffects)
                    {
                        result += effect.value;
                    }
                    break;
                case ESpellType.Fire:
                    foreach (var effect in buffAbility.FireATKIncreaseDataEffects)
                    {
                        result += effect.value;
                    }
                    break;
                case ESpellType.Water:
                    foreach (var effect in buffAbility.WaterATKIncreaseDataEffects)
                    {
                        result += effect.value;
                    }
                    break;
            }

            totalAmount *= result;

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

            if (_isAbnormalStatus)
            {
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


            labelRect.y += 40;
            valueRect.y += 40;
            GUI.Label(labelRect, "상태이상 사용 여부");
            _isAbnormalStatus = EditorGUI.Toggle(valueRect, _isAbnormalStatus);

            if (_isAbnormalStatus)
            {
                labelRect.y += 20;
                valueRect.y += 20;
                GUI.Label(labelRect, "무한지속 사용 여부");
                _isInfinity = EditorGUI.Toggle(valueRect, _isInfinity);
                if (!_isInfinity)
                {
                    labelRect.y += 20;
                    valueRect.y += 20;
                    GUI.Label(labelRect, "지속시간");
                    _duration = EditorGUI.FloatField(valueRect, _duration);
                }

                labelRect.y += 20;
                valueRect.y += 20;
                GUI.Label(labelRect, "상태이상");
                _abnormalStatus = (AbnormalStatusTemplate)EditorGUI.ObjectField(valueRect, _abnormalStatus, typeof(AbnormalStatusTemplate), false);                
            }

            labelRect.y += 20;
            valueRect.y += 20;
            GUI.Label(labelRect, "데미지");
            _damage = EditorGUI.IntField(valueRect, _damage);
        }

        public override int GetNumRows()
        {
            int rowNum = base.GetNumRows();

            rowNum += 8;

            if (_isTick)
            {
                rowNum += 2;
            }

            if (_isAbnormalStatus)
            {
                rowNum += 4;

                if (!_isInfinity)
                {
                    rowNum++;
                }
            }

            return rowNum;
        }
#endif
    }
}