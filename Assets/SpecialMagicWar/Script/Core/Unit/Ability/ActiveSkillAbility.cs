using UnityEngine;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// ������ ��Ƽ�� ��ų ����� �����մϴ�.
    /// </summary>
    public class ActiveSkillAbility : ConditionAbility
    {
        private UnitAnimationAbility _unitAnimationAbility;
        private AbnormalStatusAbility _abnormalStatusAbility;

        private ActiveSkillTemplate _template;

        private bool _isSkillActive;
        private Unit _targetUnit;
        private Vector3 _targetVector;

        private SkillEventHandler _skillEventHandler;
        private bool _isEventSkill;

        #region ���� ���
        private bool finalIsSkillAble
        {
            get
            {
                // �̹� ��ų�� ��� ���̶��
                if (_isSkillActive) return false;

                // ��ų ��� �Ұ� �����̻� �ɷȴٸ�
                if (_abnormalStatusAbility.UnableToSkillEffects.Count > 0) return false;

                return true;
            }
        }
        #endregion

        internal override void Initialize(Unit unit)
        {
            base.Initialize(unit);

            _unitAnimationAbility = unit.GetAbility<UnitAnimationAbility>();
            _abnormalStatusAbility = unit.GetAbility<AbnormalStatusAbility>();

            _skillEventHandler = GetComponentInChildren<SkillEventHandler>();
            if (_skillEventHandler != null)
            {
                _skillEventHandler.onSkill += OnSkillEvent;
                _skillEventHandler.onSkillEnd += OnSkillEndEvent;
                _isEventSkill = true;
            }
        }

        internal override void Deinitialize()
        {
            if (_skillEventHandler != null)
            {
                _skillEventHandler.onSkill -= OnSkillEvent;
                _skillEventHandler.onSkillEnd -= OnSkillEndEvent;
                _isEventSkill = false;
            }
        }

        internal override bool IsExecute()
        {
            // ��ų�� ��� ���̶�� true
            return _isSkillActive;
        }

        #region ��ų �ߵ�
        internal bool TryExecuteSkill(ActiveSkillTemplate template)
        {
            // ��ų ����� �Ұ����ϴٸ�
            if (finalIsSkillAble == false) return false;

            switch (template.skillType)
            {
                case EActiveSkillType.Instant:
                    return TryExecuteInstantSkill(template);
                case EActiveSkillType.Targeting:
                    return TryExecuteTargetingSkill(template);
                case EActiveSkillType.NonTargeting:
                    return TryExecuteNonTargetingSkill(template);
            }

            return false;
        }

        #region ��ų �ߵ� ��� �� �õ� ����
        private bool TryExecuteInstantSkill(ActiveSkillTemplate template)
        {
            foreach (var effect in template.effects)
            {
                if (effect is IGetTarget targetEffect)
                {
                    var targets = targetEffect.GetTarget(unit);

                    if (targets.Count > 0 && targets[0] != null)
                    {
                        return SkillAnimation(template);
                    }
                }
            }

            return false;
        }

        private bool TryExecuteTargetingSkill(ActiveSkillTemplate template)
        {
            LayerMask layerMask;
            switch (template.unitType)
            {
                case EUnitType.All:
                    layerMask = LayerMask.GetMask("Agent", "Enemy");
                    break;
                case EUnitType.Agent:
                    layerMask = LayerMask.GetMask("Agent");
                    break;
                case EUnitType.Enemy:
                    layerMask = LayerMask.GetMask("Enemy");
                    break;
                default:
                    return false;
            }

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10, layerMask))
            {
                var distance = hit.distance;

                if (distance <= template.skillRange)
                {
                    _targetUnit = hit.collider.GetComponent<Unit>();
                    return SkillAnimation(template);
                }
            }

            return false;
        }

        private bool TryExecuteNonTargetingSkill(ActiveSkillTemplate template)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10))
            {
                _targetVector = hit.point;

                return SkillAnimation(template);
            }

            return false;
        }
        #endregion

        private bool SkillAnimation(ActiveSkillTemplate template)
        {
            _template = template;

            bool isSuccess = false;
            if (template.parameterHash != 0 && _isEventSkill)
            {
                isSuccess = _unitAnimationAbility.TrySetTrigger(template.parameterHash);
                _isSkillActive = true;
            }

            if (isSuccess == false)
            {
                ExecuteSkill();
            }

            return true;
        }

        private void OnSkillEvent()
        {
            ExecuteSkill();
        }

        private void ExecuteSkill()
        {
            ExecuteCasterFX(_template);

            switch (_template.skillType)
            {
                case EActiveSkillType.Instant:
                    ExecuteInstantSkill(_template);
                    break;
                case EActiveSkillType.Targeting:
                    ExecuteTargetingSkill(_template);
                    break;
                case EActiveSkillType.NonTargeting:
                    ExecuteNonTargetingSkill(_template);
                    break;
            }

            if (!_isEventSkill)
            {
                OnSkillEndEvent();
            }
        }

        #region ��ų �ߵ� ��� �� ���� ����
        private void ExecuteInstantSkill(ActiveSkillTemplate template)
        {
            foreach (var effect in template.effects)
            {
                if (effect is UnitEffect unitEffect)
                {
                    if (unitEffect is IGetTarget targetEffect)
                    {
                        var targets = targetEffect.GetTarget(unit);

                        foreach (var target in targets)
                        {
                            unitEffect.Execute(unit, target);
                        }
                    }
                }
            }
        }

        private void ExecuteTargetingSkill(ActiveSkillTemplate template)
        {
            foreach (var effect in template.effects)
            {
                if (effect is UnitEffect unitEffect)
                {
                    unitEffect.Execute(unit, _targetUnit);
                }
            }

            ExecuteTargetFX(template, _targetUnit);
        }

        private void ExecuteNonTargetingSkill(ActiveSkillTemplate template)
        {
            foreach (var effect in template.effects)
            {
                if (effect is PointEffect pointEffect)
                {
                    pointEffect.Execute(unit, _targetVector);
                }
            }

            ExecuteTargetFX(template, _targetVector);
        }
        #endregion

        private void OnSkillEndEvent()
        {
            _isSkillActive = false;
        }
        #endregion

        #region FX
        private void ExecuteCasterFX(ActiveSkillTemplate template)
        {
            if (template.casterFX != null)
            {
                template.casterFX.Play(unit);
            }
        }

        private void ExecuteTargetFX(ActiveSkillTemplate template, Unit target)
        {
            if (template.targetFX != null)
            {
                template.targetFX.Play(target);
            }
        }

        private void ExecuteTargetFX(ActiveSkillTemplate template, Vector3 targetVector)
        {
            if (template.targetFX != null)
            {
                template.targetFX.Play(targetVector);
            }
        }
        #endregion
    }
}