using FrameWork.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// ���� ���µ��� �����ϴ� Ŭ����
    /// </summary>
    public class GlobalStatusSystem : MonoBehaviour, ICoreSystem
    {
        #region Effect List
        private List<GoldGainAdditionalDataEffect> _goldGainAdditionalDataEffects = new List<GoldGainAdditionalDataEffect>();
        private List<GoldGainIncreaseDataEffect> _goldGainIncreaseDataEffects = new List<GoldGainIncreaseDataEffect>();
        private List<GoldGainMultiplierDataEffect> _goldGainMultiplierDataEffects = new List<GoldGainMultiplierDataEffect>();

        private List<CostRecoveryTimeAdditionalDataEffect> _costRecoveryTimeAdditionalDataEffects = new List<CostRecoveryTimeAdditionalDataEffect>();
        private List<CostRecoveryTimeIncreaseDataEffect> _costRecoveryTimeIncreaseDataEffects = new List<CostRecoveryTimeIncreaseDataEffect>();
        private List<CostRecoveryTimeMultiplierDataEffect> _costRecoveryTimeMultiplierDataEffects = new List<CostRecoveryTimeMultiplierDataEffect>();

        #region ������Ƽ
        internal IReadOnlyList<GoldGainAdditionalDataEffect> GoldGainAdditionalDataEffects => _goldGainAdditionalDataEffects;
        internal IReadOnlyList<GoldGainIncreaseDataEffect> GoldGainIncreaseDataEffects => _goldGainIncreaseDataEffects;
        internal IReadOnlyList<GoldGainMultiplierDataEffect> GoldGainMultiplierDataEffects => _goldGainMultiplierDataEffects;
        
        internal IReadOnlyList<CostRecoveryTimeAdditionalDataEffect> CostRecoveryTimeAdditionalDataEffects => _costRecoveryTimeAdditionalDataEffects;
        internal IReadOnlyList<CostRecoveryTimeIncreaseDataEffect> CostRecoveryTimeIncreaseDataEffects => _costRecoveryTimeIncreaseDataEffects;
        internal IReadOnlyList<CostRecoveryTimeMultiplierDataEffect> CostRecoveryTimeMultiplierDataEffects => _costRecoveryTimeMultiplierDataEffects;
        #endregion
        #endregion

        private Dictionary<GlobalStatusTemplate, StatusInstance> statusDic = new Dictionary<GlobalStatusTemplate, StatusInstance>();

#if UNITY_EDITOR
        [SerializeField, ReadOnly] private List<GlobalStatusTemplate> statusList = new List<GlobalStatusTemplate>();
#endif

        public void Initialize()
        {
        }

        public void Deinitialize()
        {
            ClearStatusEffects();
        }

        internal void ApplyGlobalStatus(GlobalStatusTemplate template, float duration)
        {
            if (this == null || gameObject == null) return;

            var isContained = false;

            if (statusDic.ContainsKey(template))
            {
                isContained = true;

                var instance = statusDic[template];
                if (instance.IsOld(duration))
                {
                    instance.duration = duration;
                    instance.startTime = Time.time;
                }
                else
                {
                    return;
                }
            }

            AddStatus(template, duration, isContained);
        }

        /// <summary>
        /// ���� ���� �߰�
        /// </summary>
        private void AddStatus(GlobalStatusTemplate template, float duration, bool isContained)
        {
            StatusInstance statusInstance = new StatusInstance(duration, Time.time);

            // ���������� �ƴ϶��
            if (duration != int.MaxValue)
            {
                var corutine = StartCoroutine(CoStatus(statusInstance, template));
                statusInstance.corutine = corutine;
            }

            statusDic.Add(template, statusInstance);

#if UNITY_EDITOR
            statusList.Add(template);
#endif

            // ���� ���� ���� (������ ���� ���´� �ߺ����� ����)
            if (isContained == false)
            {
                ExecuteApplyFX(template);

                foreach (var effect in template.effects)
                {
                    if (effect is GoldGainAdditionalDataEffect goldGainAdditionalDataEffect)
                    {
                        _goldGainAdditionalDataEffects.Add(goldGainAdditionalDataEffect);
                    }
                    else if (effect is GoldGainIncreaseDataEffect goldGainIncreaseDataEffect)
                    {
                        _goldGainIncreaseDataEffects.Add(goldGainIncreaseDataEffect);
                    }
                    else if (effect is GoldGainMultiplierDataEffect goldGainMultiplierDataEffect)
                    {
                        _goldGainMultiplierDataEffects.Add(goldGainMultiplierDataEffect);
                    }
                    
                    else if (effect is CostRecoveryTimeAdditionalDataEffect costRecoveryTimeAdditionalDataEffect)
                    {
                        _costRecoveryTimeAdditionalDataEffects.Add(costRecoveryTimeAdditionalDataEffect);
                    }
                    else if (effect is CostRecoveryTimeIncreaseDataEffect costRecoveryTimeIncreaseDataEffect)
                    {
                        _costRecoveryTimeIncreaseDataEffects.Add(costRecoveryTimeIncreaseDataEffect);
                    }
                    else if (effect is CostRecoveryTimeMultiplierDataEffect costRecoveryTimeMultiplierDataEffect)
                    {
                        _costRecoveryTimeMultiplierDataEffects.Add(costRecoveryTimeMultiplierDataEffect);
                    }

                }
            }
        }

        private IEnumerator CoStatus(StatusInstance statusInstance, GlobalStatusTemplate template)
        {
            while (statusInstance.IsCompete == false)
            {
                yield return null;
            }

            RemoveStatus(template.effects);

            if (statusDic.ContainsKey(template))
            {
                statusDic.Remove(template);

#if UNITY_EDITOR
                statusList.Remove(template);
#endif

                ExecuteRemoveFX(template);
            }
        }

        private void ClearStatusEffects()
        {
            foreach (var status in statusDic)
            {
                var instance = status.Value;

                RemoveStatus(status.Key.effects);

                ExecuteRemoveFX(status.Key);

                if (instance.corutine != null)
                {
                    StopCoroutine(instance.corutine);
                    instance.corutine = null;
                }
            }

            statusDic.Clear();

#if UNITY_EDITOR
            statusList.Clear();
#endif
        }

        /// <summary>
        /// ���� ���� ����
        /// </summary>
        private void RemoveStatus(List<Effect> effects)
        {
            foreach (var effect in effects)
            {
                if (effect is GoldGainAdditionalDataEffect goldGainAdditionalDataEffect)
                {
                    _goldGainAdditionalDataEffects.Remove(goldGainAdditionalDataEffect);
                }
                else if (effect is GoldGainIncreaseDataEffect goldGainIncreaseDataEffect)
                {
                    _goldGainIncreaseDataEffects.Remove(goldGainIncreaseDataEffect);
                }
                else if (effect is GoldGainMultiplierDataEffect goldGainMultiplierDataEffect)
                {
                    _goldGainMultiplierDataEffects.Remove(goldGainMultiplierDataEffect);
                }

                else if (effect is CostRecoveryTimeAdditionalDataEffect costRecoveryTimeAdditionalDataEffect)
                {
                    _costRecoveryTimeAdditionalDataEffects.Remove(costRecoveryTimeAdditionalDataEffect);
                }
                else if (effect is CostRecoveryTimeIncreaseDataEffect costRecoveryTimeIncreaseDataEffect)
                {
                    _costRecoveryTimeIncreaseDataEffects.Remove(costRecoveryTimeIncreaseDataEffect);
                }
                else if (effect is CostRecoveryTimeMultiplierDataEffect costRecoveryTimeMultiplierDataEffect)
                {
                    _costRecoveryTimeMultiplierDataEffects.Remove(costRecoveryTimeMultiplierDataEffect);
                }
            }
        }

        #region ��ƿ��Ƽ �޼���
        internal bool Contains(GlobalStatusTemplate template)
        {
            return statusDic.ContainsKey(template);
        }

        internal bool Contains(List<GlobalStatusTemplate> templates)
        {
            var isContains = false;
            foreach (var template in templates)
            {
                if (statusDic.ContainsKey(template))
                {
                    isContains = true;
                }
            }
            return isContains;
        }
        #endregion

        #region FX
        private void ExecuteApplyFX(GlobalStatusTemplate template)
        {
            if (template.applyFX != null)
            {
                template.applyFX.Play(null);
            }
        }

        private void ExecuteRemoveFX(GlobalStatusTemplate template)
        {
            if (template.removeFX != null)
            {
                template.removeFX.Play(null);
            }
        }
        #endregion
    }
}