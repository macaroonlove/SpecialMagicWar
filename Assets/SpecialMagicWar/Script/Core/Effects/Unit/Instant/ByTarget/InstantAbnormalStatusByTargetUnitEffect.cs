using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class InstantAbnormalStatusByTargetUnitEffect : InstantAbnormalStatusUnitEffect, IGetTarget
    {
        [SerializeField] private ETarget _target;
        [SerializeField] private EAttackType _attackType;
        [SerializeField] private ESkillRangeType _skillRangeType;
        [SerializeField] private bool _isRandomTarget;
        [SerializeField] private float _radius;
        [SerializeField] private int _numberOfTarget;

        public List<Unit> GetTarget(Unit casterUnit)
        {
            if (_target == ETarget.AllTarget && _isRandomTarget)
            {
                var targets = casterUnit.GetAbility<FindTargetAbility>().FindAttackableTarget(_target, _radius, _attackType, _skillRangeType);
                
                if (targets.Count > 0)
                {
                    var units = new List<Unit>();
                    units.Add(targets[Random.Range(0, targets.Count)]);

                    return units;
                }
            }

            return casterUnit.GetAbility<FindTargetAbility>().FindAttackableTarget(_target, _radius, _attackType, _skillRangeType, _numberOfTarget);
        }

#if UNITY_EDITOR
        public override void Draw(Rect rect)
        {
            var labelRect = new Rect(rect.x, rect.y, 140, rect.height);
            var valueRect = new Rect(rect.x + 140, rect.y, rect.width - 140, rect.height);

            GUI.Label(labelRect, "피해 대상");
            _target = (ETarget)EditorGUI.EnumPopup(valueRect, _target);

            if (_target != ETarget.Myself && _target != ETarget.AllTarget)
            {
                labelRect.y += 20;
                valueRect.y += 20;
                GUI.Label(labelRect, "스킬 범위 방식");
                _skillRangeType = (ESkillRangeType)EditorGUI.EnumPopup(valueRect, _skillRangeType);
                labelRect.y += 20;
                valueRect.y += 20;
                GUI.Label(labelRect, "범위");
                _radius = EditorGUI.FloatField(valueRect, _radius);
            }

            if (_target == ETarget.AllTarget)
            {
                labelRect.y += 20;
                valueRect.y += 20;
                GUI.Label(labelRect, "랜덤한 적 한명 받아오기");
                _isRandomTarget = EditorGUI.Toggle(valueRect, _isRandomTarget);
            }

            if (_target == ETarget.NumTargetInRange)
            {
                labelRect.y += 20;
                valueRect.y += 20;
                GUI.Label(labelRect, "공격할 적의 수");
                _numberOfTarget = EditorGUI.IntField(valueRect, _numberOfTarget);
            }

            labelRect.y += 20;
            valueRect.y += 20;
            GUI.Label(labelRect, "공격 방식");
            _attackType = (EAttackType)EditorGUI.EnumPopup(valueRect, _attackType);

            rect.y = labelRect.y + 40;
            base.Draw(rect);
        }

        public override int GetNumRows()
        {
            int rowNum = base.GetNumRows();

            rowNum += 4;

            if (_target != ETarget.Myself && _target != ETarget.AllTarget)
            {
                rowNum += 2;
            }

            if (_target == ETarget.AllTarget)
            {
                rowNum++;
            }

            if (_target == ETarget.NumTargetInRange)
            {
                rowNum++;
            }

            return rowNum;
        }
#endif
    }
}