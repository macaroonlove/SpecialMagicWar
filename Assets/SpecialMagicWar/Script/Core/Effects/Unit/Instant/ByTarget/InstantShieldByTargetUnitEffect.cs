using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class InstantShieldByTargetUnitEffect : InstantShieldUnitEffect, IGetTarget
    {
        [SerializeField] private ETarget _target;
        [SerializeField] private float _radius;
        [SerializeField] private int _numberOfTarget;

        public List<Unit> GetTarget(Unit casterUnit)
        {
            return casterUnit.GetAbility<FindTargetAbility>().FindAllyTarget(_target, _radius, _numberOfTarget);
        }

#if UNITY_EDITOR
        public override void Draw(Rect rect)
        {
            var labelRect = new Rect(rect.x, rect.y, 140, rect.height);
            var valueRect = new Rect(rect.x + 140, rect.y, rect.width - 140, rect.height);

            GUI.Label(labelRect, "대상");
            _target = (ETarget)EditorGUI.EnumPopup(valueRect, _target);

            if (_target != ETarget.Myself && _target != ETarget.AllTarget)
            {
                labelRect.y += 20;
                valueRect.y += 20;
                GUI.Label(labelRect, "범위");
                _radius = EditorGUI.FloatField(valueRect, _radius);
            }

            if (_target == ETarget.NumTargetInRange)
            {
                labelRect.y += 20;
                valueRect.y += 20;
                GUI.Label(labelRect, "보호막 적용할 아군의 수");
                _numberOfTarget = EditorGUI.IntField(valueRect, _numberOfTarget);
            }

            rect.y = labelRect.y + 40;
            base.Draw(rect);
        }

        public override int GetNumRows()
        {
            int rowNum = base.GetNumRows();

            rowNum += 2;

            if (_target != ETarget.Myself && _target != ETarget.AllTarget)
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
