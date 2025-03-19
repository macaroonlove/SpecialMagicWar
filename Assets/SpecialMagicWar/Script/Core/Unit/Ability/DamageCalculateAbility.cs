using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class DamageCalculateAbility : AlwaysAbility
    {
        private EDamageType _baseDamageType;
        private int _basePhysicalResistance;
        private int _baseMagicResistance;
        private int _basePhysicalPenetration;
        private int _baseMagicPenetration;
        private float _baseCriticalHitChance;
        private float _baseCriticalHitDamage;

        private BuffAbility _buffAbility;
        private AbnormalStatusAbility _abnormalStatusAbility;

        #region ������Ƽ
        internal int basePhysicalResistance => _basePhysicalResistance;
        internal int baseMagicResistance => _baseMagicResistance;
        #endregion

        #region ��� ����
        #region ������ Ÿ��
        internal EDamageType finalDamageType
        {
            get
            {
                EDamageType result = _baseDamageType;

                foreach (var effect in _buffAbility.SetDamageTypeEffects)
                {
                    result = effect.value;
                }

                return result;
            }
        }
        #endregion

        #region ������ ������ �߰������� & ���������� & ��¡��϶�
        private int finalDamageAdditional
        {
            get
            {
                int result = 1;

                foreach (var effect in _buffAbility.DamageAdditionalDataEffects)
                {
                    result += effect.value;
                }

                return result;
            }
        }

        private float finalDamageIncrease
        {
            get
            {
                float result = 1;

                foreach(var effect in _buffAbility.DamageIncreaseDataEffects)
                {
                    result += effect.value;
                }

                return result;
            }
        }

        private float finalDamageMultiplier
        {
            get
            {
                float result = 1;

                foreach (var effect in _buffAbility.DamageMultiplierDataEffects)
                {
                    result *= effect.value;
                }

                return result;
            }
        }
        #endregion

        #region �޴� ������ �߰������� & ���������� & ��¡��϶�
        private int finalReceiveDamageAdditional
        {
            get
            {
                int result = 1;

                foreach (var effect in _buffAbility.ReceiveDamageAdditionalDataEffects)
                {
                    result += effect.value;
                }

                return result;
            }
        }

        private float finalReceiveDamageIncrease
        {
            get
            {
                float result = 1;

                foreach (var effect in _buffAbility.ReceiveDamageIncreaseDataEffects)
                {
                    result += effect.value;
                }
                foreach (var effect in _abnormalStatusAbility.ReceiveDamageIncreaseDataEffects)
                {
                    result += effect.value;
                }

                return result;
            }
        }

        private float finalReceiveDamageMultiplier
        {
            get
            {
                float result = 1;

                foreach (var effect in _buffAbility.ReceiveDamageMultiplierDataEffects)
                {
                    result *= effect.value;
                }

                return result;
            }
        }
        #endregion

        #region ���׷�
        internal int finalPhysicalResistance
        {
            get
            {
                float result = _basePhysicalResistance;

                #region �߰�������
                foreach (var effect in _buffAbility.PhysicalResistanceAdditionalDataEffects.Keys)
                {
                    result += effect.value;
                }
                #endregion

                #region ����������
                float increase = 1;

                foreach (var effect in _buffAbility.PhysicalResistanceIncreaseDataEffects.Keys)
                {
                    increase += effect.value;
                }
                foreach (var effect in _abnormalStatusAbility.PhysicalResistanceIncreaseDataEffects.Keys)
                {
                    increase += effect.value;
                }

                result *= increase;
                #endregion

                #region ��¡��϶�
                foreach (var effect in _buffAbility.PhysicalResistanceMultiplierDataEffects.Keys)
                {
                    result *= effect.value;
                }
                #endregion

                return (int)result;
            }
        }

        internal int finalMagicResistance
        {
            get
            {
                float result = _baseMagicResistance;

                #region �߰�������
                foreach (var effect in _buffAbility.MagicResistanceAdditionalDataEffects.Keys)
                {
                    result += effect.value;
                }
                #endregion

                #region ����������
                float increase = 1;

                foreach (var effect in _buffAbility.MagicResistanceIncreaseDataEffects.Keys)
                {
                    increase += effect.value;
                }
                foreach (var effect in _abnormalStatusAbility.MagicResistanceIncreaseDataEffects.Keys)
                {
                    increase += effect.value;
                }

                result *= increase;
                #endregion

                #region ��¡��϶�
                foreach (var effect in _buffAbility.MagicResistanceMultiplierDataEffects.Keys)
                {
                    result *= effect.value;
                }
                #endregion

                return (int)result;
            }
        }
        #endregion

        #region �����
        private int finalPhysicalPenetration
        {
            get
            {
                float result = _basePhysicalPenetration;

                #region �߰�������
                foreach (var effect in _buffAbility.PhysicalPenetrationAdditionalDataEffects)
                {
                    result += effect.value;
                }
                #endregion

                #region ����������
                float increase = 1;

                foreach (var effect in _buffAbility.PhysicalPenetrationIncreaseDataEffects)
                {
                    increase += effect.value;
                }

                result *= increase;
                #endregion

                #region ��¡��϶�
                foreach (var effect in _buffAbility.PhysicalPenetrationMultiplierDataEffects)
                {
                    result *= effect.value;
                }
                #endregion

                return (int)result;
            }
        }

        private int finalMagicPenetration
        {
            get
            {
                float result = _baseMagicPenetration;

                #region �߰�������
                foreach (var effect in _buffAbility.MagicPenetrationAdditionalDataEffects)
                {
                    result += effect.value;
                }
                #endregion

                #region ����������
                float increase = 1;

                foreach (var effect in _buffAbility.MagicPenetrationIncreaseDataEffects)
                {
                    increase += effect.value;
                }

                result *= increase;
                #endregion

                #region ��¡��϶�
                foreach (var effect in _buffAbility.MagicPenetrationMultiplierDataEffects)
                {
                    result *= effect.value;
                }
                #endregion

                return (int)result;
            }
        }
        #endregion

        #region ġ��Ÿ
        private bool finalIsCriticalHit
        {
            get
            {
                float chance = _baseCriticalHitChance;

                foreach (var effect in _buffAbility.CriticalHitChanceAdditionalDataEffects)
                {
                    chance += effect.value;
                }

                if (chance > 0)
                {
                    return Random.value < chance;
                }
                else
                {
                    return false;
                }
            }
        }

        private float finalCriticalHitDamage
        {
            get
            {
                float result = _baseCriticalHitDamage;

                #region �߰�������
                foreach (var effect in _buffAbility.CriticalHitDamageAdditionalDataEffects)
                {
                    result += effect.value;
                }
                #endregion

                #region ����������
                float increase = 1;

                foreach (var effect in _buffAbility.CriticalHitDamageIncreaseDataEffects)
                {
                    increase += effect.value;
                }

                result *= increase;
                #endregion

                #region ��¡��϶�
                foreach (var effect in _buffAbility.CriticalHitDamageMultiplierDataEffects)
                {
                    result *= effect.value;
                }
                #endregion

                return result;
            }
        }
        #endregion
        #endregion

        internal override void Initialize(Unit unit)
        {
            base.Initialize(unit);

            _buffAbility = unit.GetAbility<BuffAbility>();
            _abnormalStatusAbility = unit.GetAbility<AbnormalStatusAbility>();

            if (unit is AgentUnit agentUnit)
            {
                _basePhysicalResistance = agentUnit.template.PhysicalResistance;
                _baseMagicResistance = agentUnit.template.MagicResistance;
                _basePhysicalPenetration = agentUnit.template.PhysicalPenetration;
                _baseMagicPenetration = agentUnit.template.MagicPenetration;
                _baseCriticalHitChance = agentUnit.template.CriticalHitChance;
                _baseCriticalHitDamage = agentUnit.template.CriticalHitDamage;
            }
            else if (unit is EnemyUnit enemyUnit)
            {
                _baseDamageType = enemyUnit.template.DamageType;
                _basePhysicalResistance = enemyUnit.template.PhysicalResistance;
                _baseMagicResistance = enemyUnit.template.MagicResistance;
                _basePhysicalPenetration = enemyUnit.template.PhysicalPenetration;
                _baseMagicPenetration = enemyUnit.template.MagicPenetration;
                _baseCriticalHitChance = enemyUnit.template.CriticalHitChance;
                _baseCriticalHitDamage = enemyUnit.template.CriticalHitDamage;
            }
        }

        /// <summary>
        /// ���� �⺻ ���ݿ� ���� ������ ��, ������ ���
        /// </summary>
        internal int GetDamage(Unit attackedUnit, EDamageType damageType)
        {
            int finalATK = attackedUnit.GetAbility<AttackAbility>().finalATK;

            // ���׷� & �����
            float finalDamage = GetDamageByDamageType(attackedUnit, finalATK, damageType);

            // �����ϴ� ������ ������
            var attackedUnitOfDamageCalculateAbility = attackedUnit.GetAbility<DamageCalculateAbility>();
            finalDamage += attackedUnitOfDamageCalculateAbility.finalDamageAdditional;
            finalDamage *= attackedUnitOfDamageCalculateAbility.finalDamageIncrease;
            finalDamage *= attackedUnitOfDamageCalculateAbility.finalDamageMultiplier;

            // ���ݹ޴� ������ ������
            finalDamage += finalReceiveDamageAdditional;
            finalDamage *= finalReceiveDamageIncrease;
            finalDamage *= finalReceiveDamageMultiplier;

            // ġ��Ÿ�� �����ٸ�
            if (attackedUnitOfDamageCalculateAbility.finalIsCriticalHit)
            {
                // ġ��Ÿ ������
                finalDamage *= attackedUnitOfDamageCalculateAbility.finalCriticalHitDamage;
            }

            return (int)finalDamage;
        }

        /// <summary>
        /// ���� ��ų ���ݿ� ���� ������ ��, ������ ���
        /// (�⺻ �������� �̹� ������ ����)
        /// </summary>
        internal int GetDamage(Unit attackedUnit, int damage, EDamageType damageType)
        {
            // ���׷� & �����
            float finalDamage = GetDamageByDamageType(attackedUnit, damage, damageType);

            // �����ϴ� ������ ������
            var attackedUnitOfDamageCalculateAbility = attackedUnit.GetAbility<DamageCalculateAbility>();
            finalDamage += attackedUnitOfDamageCalculateAbility.finalDamageAdditional;
            finalDamage *= attackedUnitOfDamageCalculateAbility.finalDamageIncrease;
            finalDamage *= attackedUnitOfDamageCalculateAbility.finalDamageMultiplier;

            // ���ݹ޴� ������ ������
            finalDamage += finalReceiveDamageAdditional;
            finalDamage *= finalReceiveDamageIncrease;
            finalDamage *= finalReceiveDamageMultiplier;

            // ġ��Ÿ�� �����ٸ�
            if (attackedUnitOfDamageCalculateAbility.finalIsCriticalHit)
            {
                // ġ��Ÿ ������
                finalDamage *= attackedUnitOfDamageCalculateAbility.finalCriticalHitDamage;
            }

            return (int)finalDamage;
        }

        /// <summary>
        /// ������, ���� ���� ���ݿ� ���� ������ ��, ������ ���
        /// </summary>
        internal int GetDamage(int damage, EDamageType damageType)
        {
            // ���׷�
            float finalDamage = GetDamageByResistance(damage, damageType);

            // ���ݹ޴� ������ ������
            finalDamage += finalReceiveDamageAdditional;
            finalDamage *= finalReceiveDamageIncrease;
            finalDamage *= finalReceiveDamageMultiplier;

            return (int)finalDamage;
        }

        #region ���׷¡������
        /// <summary>
        /// ���׷¸� ����
        /// </summary>
        private int GetDamageByResistance(int finalATK, EDamageType damageType)
        {
            int damage = finalATK;

            switch (damageType)
            {
                case EDamageType.PhysicalDamage:
                    damage = (int)(finalATK * (100 - finalPhysicalResistance) * 0.01f);
                    break;
                case EDamageType.MagicDamage:
                    damage = (int)(finalATK * (100 - finalMagicResistance) * 0.01f);
                    break;
            }

            return damage;
        }

        /// <summary>
        /// ���׷°� ����� ��� ����
        /// </summary>
        private int GetDamageByDamageType(Unit attackedUnit, int finalATK, EDamageType damageType)
        {
            int damage = finalATK;

            switch (damageType)
            {
                case EDamageType.PhysicalDamage:
                    int finalPhysicalPenetration = attackedUnit.GetAbility<DamageCalculateAbility>().finalPhysicalPenetration;
                    damage = (int)(finalATK * (100 - (finalPhysicalResistance - finalPhysicalPenetration)) * 0.01f);
                    break;
                case EDamageType.MagicDamage:
                    int finalMagicPenetration = attackedUnit.GetAbility<DamageCalculateAbility>().finalMagicPenetration;
                    damage = (int)(finalATK * (100 - (finalMagicResistance - finalMagicPenetration)) * 0.01f);
                    break;
            }

            return damage;
        }
        #endregion
    }
}
