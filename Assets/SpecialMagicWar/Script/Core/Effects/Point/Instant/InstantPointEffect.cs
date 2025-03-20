using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public abstract class InstantPointEffect : PointEffect
    {
        [SerializeField] protected ETarget _target;
        [SerializeField] protected float _skillRange;
        [SerializeField] protected bool _isPoint;
        
        [SerializeField] protected FX _targetFX;

        public override void Execute(Unit casterUnit, Vector3 targetVector)
        {
            if (casterUnit == null) return;

            GetTargetStraight(casterUnit);
        }

        private void GetTargetStraight(Unit casterUnit)
        {
            var targets = casterUnit.GetAbility<FindTargetAbility>().FindAttackableTarget(_target, _skillRange, EAttackType.Near, ESkillRangeType.Straight);

            foreach (var target in targets)
            {
                SkillImpact(casterUnit, target);
            }

            for (float y = 0; y <= _skillRange; y += 1)
            {
                ExecuteTargetFX(casterUnit.transform.position + new Vector3(0, -y, 0));
            }
        }

        protected abstract void SkillImpact(Unit casterUnit, Unit targetUnit);

        #region FX
        private void ExecuteTargetFX(Vector3 targetPos)
        {
            if (_targetFX != null)
            {
                _targetFX.Play(targetPos);
            }
        }
        #endregion

#if UNITY_EDITOR
        protected float lastRectY { get; private set; }

        public override void Draw(Rect rect)
        {
            var labelRect = new Rect(rect.x, rect.y, 140, rect.height);
            var valueRect = new Rect(rect.x + 140, rect.y, rect.width - 140, rect.height);

            GUI.Label(labelRect, "대상자 FX");
            _targetFX = (FX)EditorGUI.ObjectField(valueRect, _targetFX, typeof(FX), false);

            labelRect.y += 40;
            valueRect.y += 40;
            GUI.Label(labelRect, "특정 지점에 적용 여부");
            _isPoint = EditorGUI.Toggle(valueRect, _isPoint);

            if (_isPoint)
            {

            }

            labelRect.y += 40;
            valueRect.y += 40;
            GUI.Label(labelRect, "타겟 방식");
            _target = (ETarget)EditorGUI.EnumPopup(valueRect, _target);

            if (!(_target == ETarget.Myself || _target == ETarget.AllTarget))
            {
                labelRect.y += 20;
                valueRect.y += 20;
                GUI.Label(labelRect, "범위");
                _skillRange = EditorGUI.FloatField(valueRect, _skillRange);
            }

            lastRectY = labelRect.y;
        }

        public override int GetNumRows()
        {
            return 6;
        }
#endif
    }
}
