using DamageNumbersPro;
using FrameWork.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    public class HealthAbility : AlwaysAbility
    {
        private PoolSystem _poolSystem;

        private PassiveSkillAbility _passiveSkillAbility;
        private BuffAbility _buffAbility;
        private AbnormalStatusAbility _abnormalStatusAbility;

        private int _baseMaxHP;
        private int _currentHP;
        private int _baseHPRecoveryPerSec;
        private float _hpRecoveryCooldown = 1;

        internal int currentHP => _currentHP;

        internal bool isAlive => _currentHP > 0;


        #region ��ȣ�� �ʵ�
        private class ShieldInstance
        {
            public Coroutine coroutine;
            public int id;
            public int amount;
            public float duration;

            // ���� ���� ��ȣ��
            public ShieldInstance(int amount, float duration)
            {
                this.amount = amount;
                this.duration = duration;
            }

            // ���ӽð��� �ִ� ��ȣ��
            public ShieldInstance(Coroutine coroutine, int id, int amount, float duration)
            {
                this.coroutine = coroutine;
                this.id = id;
                this.amount = amount;
                this.duration = duration;
            }
        }

        [SerializeField, Label("��ȣ��")] private GameObject _shieldFX;

        private List<ShieldInstance> _shields = new List<ShieldInstance>();
        private GameObject _shieldObject;
        private int _shieldIdCounter = 0;

        internal int shieldCount => _shields.Count;
        internal int shieldAmount => _shields.Sum(shield => shield.amount);
        #endregion

        internal event UnityAction<int, int> onDamage;
        internal event UnityAction<int> onChangedHealth;
        internal event UnityAction<int> onChangedShield;
        internal event UnityAction onDeath;

        [Header("�˾� �ؽ�Ʈ")]
        [SerializeField, Label("���� �������� �˾�")] private DamageNumber _physicalDamagePopup;
        [SerializeField, Label("���� �������� �˾�")] private DamageNumber _magicDamagePopup;
        [SerializeField, Label("���� �������� �˾�")] private DamageNumber _trueDamagePopup;
        [SerializeField, Label("ȸ���� �˾�")] private DamageNumber _healPopup;
        [SerializeField, Label("��ȣ�� ����� �˾�")] private DamageNumber _shieldPopup;

        #region ��� ����
        // �ִ� HP
        internal int finalMaxHP
        {
            get
            {
                float result = _baseMaxHP;

                #region �߰�������
                foreach (var effect in _buffAbility.MaxHPAdditionalDataEffects)
                {
                    result += effect.value;
                }
                #endregion

                #region ����������
                float increase = 1;

                foreach (var effect in _buffAbility.MaxHPIncreaseDataEffects)
                {
                    increase += effect.value;
                }

                result *= increase;
                #endregion

                #region ��¡��϶�
                foreach (var effect in _buffAbility.MaxHPMultiplierDataEffects)
                {
                    result *= effect.value;
                }
                #endregion

                return (int)result;
            }
        }

        // ������ �ּ� ü��
        // ������ ü���� ���� �� �ִٸ� ���� ū ���� ä��
        private int finalMinHP
        {
            get
            {
                int maxMinHP = 0;

                foreach (var effect in _buffAbility.SetMinHPEffects)
                {
                    maxMinHP = Mathf.Max(maxMinHP, effect.value);
                }

                return maxMinHP;
            }
        }

        // �ʴ� HP ȸ����
        private int finalHPRecoveryPerSec
        {
            get
            {
                int result = _baseHPRecoveryPerSec;

                // �ִ� ü���� % ��ŭ �ʴ� ȸ���� �߰�
                foreach (var effect in _abnormalStatusAbility.HPRecoveryPerSecByMaxHPIncreaseDataEffects)
                {
                    result += (int)(effect.value * finalMaxHP);
                }

                return result;
            }
        }

        #region �߰� ȸ����
        private int healingAdditional
        {
            get
            {
                int result = 0;

                foreach (var effect in _buffAbility.HealingAdditionalDataEffects)
                {
                    result += effect.value;
                }

                return result;
            }
        }

        private float healingIncrease
        {
            get
            {
                float result = 1;

                foreach (var effect in _buffAbility.HealingIncreaseDataEffects)
                {
                    result += effect.value;
                }

                return result;
            }
        }

        private float healingMultiplier
        {
            get
            {
                float result = 1;

                foreach (var effect in _buffAbility.HealingMultiplierDataEffects)
                {
                    result *= effect.value;
                }

                return result;
            }
        }
        #endregion

        internal bool finalIsHealAble
        {
            get
            {
                // ������ �׾�����
                if (isAlive == false) return false;

                // Ǯ�Ƕ��
                if (_currentHP == finalMaxHP) return false;

                // ȸ�� �Ұ� �����̻� �ɷȴٸ�
                if (_abnormalStatusAbility.UnableToHealEffects.Count > 0) return false;

                return true;
            }
        }
        #endregion

        internal override void Initialize(Unit unit)
        {
            base.Initialize(unit);

            _poolSystem = CoreManager.Instance.GetSubSystem<PoolSystem>();

            _passiveSkillAbility = unit.GetAbility<PassiveSkillAbility>();
            _buffAbility = unit.GetAbility<BuffAbility>();
            _abnormalStatusAbility = unit.GetAbility<AbnormalStatusAbility>();

            if (unit is AgentUnit agentUnit)
            {
                _baseMaxHP = agentUnit.template.MaxHP;
                _baseHPRecoveryPerSec = agentUnit.template.HPRecoveryPerSec;
            }
            else if (unit is EnemyUnit enemyUnit)
            {
                _baseMaxHP = enemyUnit.template.MaxHP;
                _baseHPRecoveryPerSec = enemyUnit.template.HPRecoveryPerSec;
            }

            SetHP(finalMaxHP);
        }

        internal override void UpdateAbility()
        {
            // �ʴ� ȸ�� ��Ÿ�� ����
            if (_hpRecoveryCooldown > 0)
            {
                _hpRecoveryCooldown -= Time.deltaTime;
                return;
            }

            var hpRecoveryAmount = finalHPRecoveryPerSec;
            
            if (hpRecoveryAmount > 0)
            {
                // ȸ�� �Ұ��� ���¶�� ����
                if (finalIsHealAble) return;

                SetHP(_currentHP + hpRecoveryAmount);
                _hpRecoveryCooldown = 1;
            }
            else if (hpRecoveryAmount < 0)
            {
                Damaged(-hpRecoveryAmount, EDamageType.TrueDamage, 1000);
                _hpRecoveryCooldown = 1;
            }
        }

        #region HP ����
        internal bool Damaged(int damage, EDamageType damageType, int id)
        {
            // �׾����� ����
            if (isAlive == false) return false;

            // �ǵ忡 ������ �������� ����
            var lostHealth = DamagedShield(damage);
            lostHealth = Mathf.Max(0, lostHealth);

            // �ǵ��� �����
            var absorption = damage - lostHealth;
            if (absorption > 0)
            {
                ShieldPopup(absorption);
            }

            // ���� HP �� ���� ��
            if (lostHealth > 0)
            {
                SetHP(_currentHP - lostHealth);
                onDamage?.Invoke(id, lostHealth);

                DamagePopup(lostHealth, damageType);

                return true;
            }

            return false;
        }

        internal void Healed(int value, Unit casterUnit = null)
        {
            // ȸ�� �Ұ��� ���¶�� ����
            if (finalIsHealAble == false) return;

            float healingAmount = value;

            // �߰� ȸ���� ����
            healingAmount += healingAdditional;
            healingAmount *= healingIncrease;
            healingAmount *= healingMultiplier;

            var lastHp = Mathf.RoundToInt(_currentHP + healingAmount);

            HealPopup(healingAmount);

            SetHP(lastHp);

            foreach (var effect in _passiveSkillAbility.healEventEffects)
            {
                effect.Execute(unit, casterUnit);
            }
        }

        private void SetHP(int hp)
        {
            _currentHP = Mathf.Clamp(hp, finalMinHP, finalMaxHP);
            if (_currentHP == 0)
            {
                onDeath?.Invoke();
                return;
            }
            onChangedHealth?.Invoke(_currentHP);
        }
        #endregion

        #region ��ȣ�� ����
        private int DamagedShield(int damage)
        {
            int finalDamage = damage;

            // �ǵ尡 ���� ��
            if (shieldCount > 0)
            {
                int totalShield = shieldAmount;

                // ��ȣ������ ��� ������
                if (totalShield >= damage)
                {
                    // �ֱٿ� �߰��� ��ȣ������ ����
                    for (int i = shieldCount - 1; i >= 0; i--)
                    {
                        var shield = _shields[i];
                        int remainingShield = shield.amount - damage;

                        if (remainingShield >= 0)
                        {
                            shield.amount = remainingShield;
                            onChangedShield?.Invoke(totalShield - damage);
                            return 0;
                        }
                        else
                        {
                            damage -= shield.amount;

                            if (shield.coroutine != null)
                            {
                                StopCoroutine(shield.coroutine);
                            }
                            _shields.RemoveAt(i);
                            UpdateShield();
                        }
                    }

                    #region ������ ��ȣ������ ����
                    //for (int i = 0; i < shieldCount; i++)
                    //{
                    //    var shield = _shields[i];
                    //    int remainingShield = shield.amount - damage;
                    //    if (remainingShield >= 0)
                    //    {
                    //        shield.amount = remainingShield;
                    //        onChangedShield?.Invoke(totalShield - damage);
                    //        return 0;
                    //    }
                    //    else
                    //    {
                    //        damage -= shield.amount;

                    //        if (shield.coroutine != null)
                    //        {
                    //            StopCoroutine(shield.coroutine);
                    //        }
                    //        _shields.RemoveAt(i);
                    //        UpdateShield();
                    //        i--;
                    //    }
                    //}
                    #endregion
                }
                // ��ȣ������ ������� ���ϰ� �ǰ� ���� ��
                else
                {
                    _shields.Clear();
                    UpdateShield();
                    onChangedShield?.Invoke(0);
                }

                finalDamage -= totalShield;
            }

            return finalDamage;
        }

        internal void AddShield(int amount)
        {
            //�׾����� ����
            if (isAlive == false) return;

            _shields.Add(new ShieldInstance(amount, int.MaxValue));
            UpdateShield();
        }

        internal void AddShield(int amount, float duration)
        {
            //�׾����� ����
            if (isAlive == false) return;

            var coroutine = StartCoroutine(CoShield(_shieldIdCounter, duration));
            _shields.Add(new ShieldInstance(coroutine, _shieldIdCounter, amount, duration));
            UpdateShield();
            _shieldIdCounter++;
        }

        private void UpdateShield()
        {
            onChangedShield?.Invoke(shieldAmount);

            if (_shieldFX == null) return;

            if (shieldCount > 0)
            {
                _shieldObject = _poolSystem.Spawn(_shieldFX, this.transform);
            }
            else
            {
                _poolSystem.DeSpawn(_shieldObject);
            }
        }

        private IEnumerator CoShield(int id, float duration)
        {
            yield return new WaitForSeconds(duration);

            for (int i = 0; i < shieldCount; i++)
            {
                if (_shields[i].id == id)
                {
                    _shields.RemoveAt(i);
                    break;
                }
            }
            UpdateShield();
        }
        #endregion

        #region �˾� �ؽ�Ʈ
        private void DamagePopup(float damage, EDamageType damageType)
        {
            if (this == null) return;

            DamageNumber popup;

            switch (damageType)
            {
                case EDamageType.PhysicalDamage:
                    popup = _physicalDamagePopup?.Spawn(transform.position, damage);
                    break;
                case EDamageType.MagicDamage:
                    popup = _magicDamagePopup?.Spawn(transform.position, damage);
                    break;
                default:
                    popup = _trueDamagePopup?.Spawn(transform.position, damage);
                    break;
            }

            popup?.SetFollowedTarget(transform);
            popup?.SetScale(0.5f);
        }

        private void HealPopup(float heal)
        {
            if (this == null) return;

            DamageNumber popup = _healPopup?.Spawn(transform.position, heal);

            popup?.SetFollowedTarget(transform);
            popup?.SetScale(0.5f);
        }

        private void ShieldPopup(float absorption)
        {
            if (this == null) return;

            DamageNumber popup = _shieldPopup?.Spawn(transform.position, absorption);

            popup?.SetFollowedTarget(transform);
            popup?.SetScale(0.5f);
        }
        #endregion
    }
}