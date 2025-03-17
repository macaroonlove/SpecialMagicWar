using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// ������ ������ �����մϴ�.
    /// </summary>
    public class ManaAbility : AlwaysAbility
    {
        private BuffAbility _buffAbility;
        private AbnormalStatusAbility _abnormalStatusAbility;

        private int _baseMaxMana;
        private int _currentMana;
        private float _baseManaRecoveryPerSec;
        private float _manaRecoveryCooldown = 1;
        private EManaRecoveryType _manaRecoveryType;

        internal event UnityAction<int> onChangedMana;

        #region ���� ���
        internal int finalMaxMana
        {
            get
            {
                int result = _baseMaxMana;

                return result;
            }
        }

        internal int finalManaRecoveryPerSec
        {
            get
            {
                float result = _baseManaRecoveryPerSec;

                #region �߰�������
                foreach (var effect in _buffAbility.ManaRecoveryPerSecAdditionalDataEffects)
                {
                    result += effect.value;
                }
                #endregion

                #region ����������
                float increase = 1;

                foreach (var effect in _buffAbility.ManaRecoveryPerSecIncreaseDataEffects)
                {
                    increase += effect.value;
                }

                result *= increase;
                #endregion

                #region ��¡��϶�
                foreach (var effect in _buffAbility.ManaRecoveryPerSecMultiplierDataEffects)
                {
                    result *= effect.value;
                }
                #endregion

                return (int)result;
            }
        }

        private bool finalIsManaRecoveryAble
        {
            get
            {
                // ���� ȸ�� �Ұ� �����̻� �ɷȴٸ� (ex. ���� ȸ�� �Ұ��� ��������)
                //if (_abnormalStatusAbility.UnableToManaRecoveryEffects.Count > 0) return false;

                return true;
            }
        }
        #endregion

        internal override void Initialize(Unit unit)
        {
            if (unit is AgentUnit agentUnit)
            {
                _buffAbility = unit.GetAbility<BuffAbility>();
                _abnormalStatusAbility = unit.GetAbility<AbnormalStatusAbility>();

                _baseMaxMana = agentUnit.template.MaxMana;
                _currentMana = agentUnit.template.StartMana;
                _baseManaRecoveryPerSec = agentUnit.template.ManaRecoveryPerSec;
                _manaRecoveryType = agentUnit.template.ManaRecoveryType;
            }
            else if (unit is EnemyUnit enemyUnit)
            {
                _baseMaxMana = enemyUnit.template.MaxMana;
                _currentMana = enemyUnit.template.StartMana;
                _baseManaRecoveryPerSec = enemyUnit.template.ManaRecoveryPerSec;
                _manaRecoveryType = enemyUnit.template.ManaRecoveryType;
            }

            SetManaRecoveryType(true);
        }

        internal override void Deinitialize()
        {
            SetManaRecoveryType(false);
        }

        internal override void UpdateAbility()
        {
            if (_manaRecoveryType == EManaRecoveryType.Automatic)
            {
                OnRecoveryWhenAutomatic();
            }
        }

        #region ���� ȸ�� �̺�Ʈ
        private void SetManaRecoveryType(bool isActive)
        {
            switch (_manaRecoveryType)
            {
                case EManaRecoveryType.Attack:
                    if (isActive) unit.GetAbility<AttackAbility>().onAttack += OnRecoveryWhenAttack;
                    else unit.GetAbility<AttackAbility>().onAttack -= OnRecoveryWhenAttack;
                    break;
                case EManaRecoveryType.Hit:
                    if (isActive) unit.GetAbility<HitAbility>().onHit += OnRecoveryWhenHit;
                    else unit.GetAbility<HitAbility>().onHit -= OnRecoveryWhenHit;
                    break;
            }
        }

        private void OnRecoveryWhenAutomatic()
        {
            // 1�� ���� ���� ȸ��
            if (_manaRecoveryCooldown > 0)
            {
                _manaRecoveryCooldown -= Time.deltaTime;
                return;
            }

            Recovery(finalManaRecoveryPerSec);
            _manaRecoveryCooldown = 1;
        }

        private void OnRecoveryWhenAttack()
        {
            Recovery(finalManaRecoveryPerSec);
        }

        private void OnRecoveryWhenHit()
        {
            Recovery(finalManaRecoveryPerSec);
        }
        #endregion

        #region ���� ����
        /// <summary>
        /// ���� ȸ��
        /// </summary>
        internal void Recovery(int mana)
        {
            if (finalIsManaRecoveryAble == false) return;

            int finalMana = _currentMana + mana;

            SetMana(finalMana);
        }

        private void SetMana(int mana)
        {
            _currentMana = Mathf.Min(_baseMaxMana, mana);
            onChangedMana?.Invoke(_currentMana);
        }
        #endregion

        #region ��ų ���
        internal bool TryExecuteSkill(int needMana)
        {
            // �ʿ��� �������� ���� ������ ���ٸ�
            if (_currentMana < needMana) return false;

            int finalMana = _currentMana - needMana;

            SetMana(finalMana);

            return true;
        }

        /// <summary>
        /// ������ ����ϴٸ� True, ������� �ʴٸ� False
        /// </summary>
        internal bool CheckMana(int needMana)
        {
            return _currentMana > needMana;
        }
        #endregion
    }
}
