using FrameWork.Editor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// ����, ����ü ������ �����ϴ� �����Ƽ
    /// ���ݷ�, ���ݼӵ�, ���� ��Ÿ�, ���� ���� ��, ���� ����� ����մϴ�.
    /// </summary>
    public class AttackAbility : ConditionAbility
    {
        private bool _isProjectileAttack;
        private GameObject _projectilePrefab;
        private ESpawnPoint _spawnPoint;

        private UnitAnimationAbility _unitAnimationAbility;
        private PassiveSkillAbility _passiveSkillAbility;
        private BuffAbility _buffAbility;
        private AbnormalStatusAbility _abnormalStatusAbility;
        private FindTargetAbility _findTargetAbility;
        private ProjectileAbility _projectileAbility;

        private int _baseATK;
        private float _baseAttackTerm;
        private float _baseAttackRange;
        private EAttackType _baseAttackType;
        private float _attackCooldown;
        private bool _isAttackActive;

        private AttackEventHandler _attackEventHandler;
        private bool _isEventAttack;

        private EAttackType _currentAttackType;
        private List<Unit> _currentTarget = new List<Unit>();

        private FX _casterFX;
        private FX _targetFX;

        #region ������Ƽ
        internal int baseATK => _baseATK;
        #endregion

        #region ���� ���
        internal int finalATK
        {
            get
            {
                float result = _baseATK;

                #region �߰�������
                foreach (var effect in _buffAbility.ATKAdditionalDataEffects.Keys)
                {
                    result += effect.value;
                }
                #endregion

                #region ����������
                float increase = 1;

                foreach (var effect in _buffAbility.ATKIncreaseDataEffects.Keys)
                {
                    increase += effect.value;
                }

                result *= increase;
                #endregion

                #region ��¡��϶�
                foreach (var effect in _buffAbility.ATKMultiplierDataEffects.Keys)
                {
                    result *= effect.value;
                }
                #endregion

                return (int)result;
            }
        }

        private float finalAttackTerm
        {
            get
            {
                float result = _baseAttackTerm;

                #region ����������
                float increase = 1;

                foreach (var effect in _buffAbility.AttackSpeedIncreaseDataEffects)
                {
                    increase += effect.value;
                }

                result *= increase;
                #endregion

                #region ��¡��϶�
                foreach (var effect in _buffAbility.AttackSpeedMultiplierDataEffects)
                {
                    result *= effect.value;
                }
                #endregion

                return result;
            }
        }

        private float finalAttackRange
        {
            get
            {
                float result = _baseAttackRange;

                return result;
            }
        }

        private int finalAttackCount
        {
            get
            {
                // �ִ� ���� ���� ���� 1��
                int result = 1;

                foreach (var effect in _buffAbility.AttackCountAdditionalDataEffects)
                {
                    result += effect.value;
                }

                return result;
            }
        }

        private EAttackType finalAttackType
        {
            get
            {
                EAttackType result = _baseAttackType;

                foreach (var effect in _buffAbility.SetAttackTypeEffects)
                {
                    result = effect.value;
                }

                return result;
            }
        }

        private bool finalIsAttackAble
        {
            get
            {
                // ���� �Ұ� �����̻� �ɷȴٸ�
                if (_abnormalStatusAbility.UnableToAttackEffects.Count > 0) return false;

                return true;
            }
        }
        #endregion

        internal event UnityAction onAttack;

        internal override void Initialize(Unit unit)
        {
            base.Initialize(unit);

            _unitAnimationAbility = unit.GetAbility<UnitAnimationAbility>();
            _passiveSkillAbility = unit.GetAbility<PassiveSkillAbility>();
            _buffAbility = unit.GetAbility<BuffAbility>();
            _abnormalStatusAbility = unit.GetAbility<AbnormalStatusAbility>();
            _findTargetAbility = unit.GetAbility<FindTargetAbility>();
            _projectileAbility = unit.GetAbility<ProjectileAbility>();

            if (unit is HolyAnimalUnit holyAnimalUnit)
            {
                _baseATK = holyAnimalUnit.template.ATK;
                _baseAttackTerm = holyAnimalUnit.template.AttackTerm;
                _baseAttackRange = holyAnimalUnit.template.AttackRange;
                _baseAttackType = holyAnimalUnit.template.AttackType;
                _casterFX = holyAnimalUnit.template.casterFX;
                _targetFX = holyAnimalUnit.template.targetFX;

                if (holyAnimalUnit.template.isProjectileAttack)
                {
                    _isProjectileAttack = true;
                    _projectilePrefab = holyAnimalUnit.template.projectilePrefab;
                    _spawnPoint = holyAnimalUnit.template.spawnPoint;
                }
            }
            else if (unit is EnemyUnit enemyUnit)
            {
                _baseATK = enemyUnit.template.ATK;
                _baseAttackTerm = enemyUnit.template.AttackTerm;
                _baseAttackRange = enemyUnit.template.AttackRange;
                _baseAttackType = enemyUnit.template.AttackType;
                _casterFX = enemyUnit.template.casterFX;
                _targetFX = enemyUnit.template.targetFX;

                if (enemyUnit.template.isProjectileAttack)
                {
                    _isProjectileAttack = true;
                    _projectilePrefab = enemyUnit.template.projectilePrefab;
                    _spawnPoint = enemyUnit.template.spawnPoint;
                }
            }

            _attackCooldown = finalAttackTerm;

            _attackEventHandler = GetComponentInChildren<AttackEventHandler>();
            if (_attackEventHandler != null)
            {
                _attackEventHandler.onAttack += OnAttackEvent;
                _isEventAttack = true;
            }
        }

        internal override void Deinitialize()
        {
            if (_attackEventHandler != null)
            {
                _attackEventHandler.onAttack -= OnAttackEvent;
                _isEventAttack = false;
            }
        }

        internal override bool IsExecute()
        {
            if (_isAttackActive) return true;

            return IsAction();
        }

        internal override void UpdateAbility()
        {
            if (_isAttackActive == false)
            {
                unit.ReleaseCurrentAbility();
            }
        }

        private bool IsAction()
        {
            // ���� ��Ÿ�� ����
            if (_attackCooldown > 0)
            {
                _attackCooldown -= Time.deltaTime;
                return false;
            }

            // ���� �Ұ� ���¶�� �޼��� ������
            if (finalIsAttackAble == false || finalAttackType == EAttackType.None) return false;
            
            _currentAttackType = finalAttackType;

            // ������ �������θ� ��ȯ
            bool isAction = Action();

            // ������ �������� ���
            if (isAction)
            {
                _isAttackActive = true;

                // ��Ÿ�� ���
                _attackCooldown = finalAttackTerm;
            }

            return isAction;
        }

        private bool Action()
        {
            // �ٰŸ��� ���Ÿ� �����̶��
            if (_currentAttackType == EAttackType.Near || _currentAttackType == EAttackType.Far)
            {
                return Attack();
            }
            // ȸ�� �����̶��
            else if (_currentAttackType == EAttackType.Heal)
            {
                return Heal();
            }

            return false;
        }

        #region ����, ȸ�� ���� ����
        private void AttackAnimation()
        {
            _unitAnimationAbility.Attack();

            if (!_isEventAttack)
            {
                ExecuteAction();
            }
        }

        private void OnAttackEvent()
        {
            ExecuteAction();
        }

        private void ExecuteAction()
        {
            // �ٰŸ��� ���Ÿ� �����̶��
            if (_currentAttackType == EAttackType.Near || _currentAttackType == EAttackType.Far)
            {
                ExecuteAttack();
            }
            // ȸ�� �����̶��
            else if (_currentAttackType == EAttackType.Heal)
            {
                ExecuteHeal();
            }
        }

        internal void ApplyAction(Unit target)
        {
            // �ٰŸ��� ���Ÿ� �����̶��
            if (_currentAttackType == EAttackType.Near || _currentAttackType == EAttackType.Far)
            {
                ApplyAttack(target);
            }
            // ȸ�� �����̶��
            else if (_currentAttackType == EAttackType.Heal)
            {
                ApplyHeal(target);
            }
        }
        #endregion

        #region ���� ����
        private bool Attack()
        {
            _currentTarget.Clear();

            var attackTargets = _findTargetAbility.FindAttackableTarget(ETarget.NumTargetInRange, finalAttackRange, _currentAttackType, ESkillRangeType.Circle, finalAttackCount);

            if (attackTargets.Count > 0)
            {
                _currentTarget.AddRange(attackTargets);
            }

            // ���� ��� ����
            if (_currentTarget.Count > 0)
            {
                AttackAnimation();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ExecuteAttack()
        {
            ExecuteCasterFX();

            // ����ü ������ ���
            if (_isProjectileAttack)
            {
                // ����ü ����
                foreach (var attackTarget in _currentTarget)
                {
                    _projectileAbility.SpawnProjectile(_projectilePrefab, _spawnPoint, attackTarget, (caster, target) => { ApplyAction(target); });
                }
            }
            // ��� ������ ���
            else
            {
                foreach (var attackTarget in _currentTarget)
                {
                    ApplyAttack(attackTarget);
                }
            }

            _isAttackActive = false;
        }

        private void ApplyAttack(Unit attackTarget)
        {
            ExecuteTargetFX(attackTarget);
            
            attackTarget.GetAbility<HitAbility>().Hit(unit);

            onAttack?.Invoke();

            foreach (var effect in _passiveSkillAbility.attackEventEffects)
            {
                effect.Execute(unit, attackTarget);
            }
        }
        #endregion

        #region ȸ�� ����
        private bool Heal()
        {
            _currentTarget.Clear();

            var attackTargets = _findTargetAbility.FindHealableTarget(ETarget.NumTargetInRange, finalAttackRange, finalAttackCount);

            if (attackTargets.Count > 0)
            {
                _currentTarget.AddRange(attackTargets);
            }

            // ȸ�� ��� ����
            if (_currentTarget.Count > 0)
            {
                AttackAnimation();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ExecuteHeal()
        {
            ExecuteCasterFX();

            // ����ü ȸ���� ���
            if (_isProjectileAttack)
            {
                // ����ü ����
                foreach (var healTarget in _currentTarget)
                {
                    _projectileAbility.SpawnProjectile(_projectilePrefab, _spawnPoint, healTarget, (caster, target) => { ApplyAction(target); });
                }
            }
            // ��� ȸ���� ���
            else
            {
                foreach (var healTarget in _currentTarget)
                {
                    ApplyHeal(healTarget);
                }
            }

            _isAttackActive = false;
        }

        private void ApplyHeal(Unit healTarget)
        {
            ExecuteTargetFX(healTarget);

            healTarget.GetAbility<HealthAbility>().Healed(finalATK, unit);

            onAttack?.Invoke();

            foreach (var effect in _passiveSkillAbility.attackEventEffects)
            {
                effect.Execute(unit, healTarget);
            }
        }
        #endregion

        #region FX
        private void ExecuteCasterFX()
        {
            if (_casterFX != null)
            {
                _casterFX.Play(unit);
            }
        }

        private void ExecuteTargetFX(Unit target)
        {
            if (_targetFX != null)
            {
                _targetFX.Play(target);
            }
        }
        #endregion
    }
}