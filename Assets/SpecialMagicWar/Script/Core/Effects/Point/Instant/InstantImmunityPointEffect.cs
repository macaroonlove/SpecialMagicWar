using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class InstantImmunityPointEffect : InstantPointEffect
    {
        [SerializeField] protected bool _isInfinity;
        [SerializeField] protected float _duration;
        [SerializeField] protected int _count;
        [SerializeField] protected EDamageType _immunityType;

        public override string GetDescription()
        {
            return "즉시 피해면역";
        }

        protected override void SkillImpact(Unit casterUnit, Unit targetUnit)
        {
            if (_isInfinity)
            {
                targetUnit.GetAbility<HitAbility>().AddDamageImmunity(_immunityType, _count);
            }
            else
            {
                if (_count == 0)
                {
                    targetUnit.GetAbility<HitAbility>().AddDamageImmunity(_immunityType, _duration);
                }
                else
                {
                    targetUnit.GetAbility<HitAbility>().AddDamageImmunity(_immunityType, _count, _duration);
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
            GUI.Label(labelRect, "개수");
            _count = EditorGUI.IntField(valueRect, _count);

            labelRect.y += 40;
            valueRect.y += 40;
            GUI.Label(labelRect, "무시할 데미지 타입");
            _immunityType = (EDamageType)EditorGUI.EnumPopup(valueRect, _immunityType);
        }

        public override int GetNumRows()
        {
            int rowNum = base.GetNumRows();

            rowNum += 6;

            if (!_isInfinity)
            {
                rowNum++;
            }
            
            return rowNum;
        }
#endif
    }
}