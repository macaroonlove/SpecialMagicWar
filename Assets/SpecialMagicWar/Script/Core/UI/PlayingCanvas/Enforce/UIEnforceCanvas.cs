using FrameWork.UIBinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpecialMagicWar.Core
{
    public class UIEnforceCanvas : UIBase
    {
        #region
        enum Buttons
        {
            GenerateProbabilityButton,
            LandEnforceButton,
            FireEnforceButton,
            WaterEnforceButton,
            HolyAnimalEnforceButton,
        }
        enum Texts
        {
            NeedCostText,
            NeedLandSoulText,
            NeedFireSoulText,
            NeedWaterSoulText,
            NeedHolyAnimalSoulText,
            GenerateProbabilityLevel,
            LandEnforceLevel,
            FireEnforceLevel,
            WaterEnforceLevel,
            HolyAnimalEnforceLevel,
        }
        #endregion

        private BuffAbility _buffAbility;
        private AgentSystem _agentsystem;
        private CostSystem _costSystem;
        private SoulSystem _soulSystem;
        private UIProbabilityInfoCanvas _uiProbabilityInfoCanvas;
        private UISpellCanvas _uiSpellCanvas;

        private InGameTemplate _inGameTemplate;

        private Button _generateProbabilityButton;
        private Button _landEnforceButton;
        private Button _fireEnforceButton;
        private Button _waterEnforceButton;
        private Button _holyAnimalEnforceButton;

        private TextMeshProUGUI _needCostText;
        private TextMeshProUGUI _needLandSoulText;
        private TextMeshProUGUI _needFireSoulText;
        private TextMeshProUGUI _needWaterSoulText;
        private TextMeshProUGUI _needHolyAnimalSoulText;
        
        private TextMeshProUGUI _generateProbabilityLevel;
        private TextMeshProUGUI _landEnforceLevel;
        private TextMeshProUGUI _fireEnforceLevel;
        private TextMeshProUGUI _waterEnforceLevel;
        private TextMeshProUGUI _holyAnimalEnforceLevel;

        private int _currentNeedCost;
        private int _currentNeedLandSoul;
        private int _currentNeedFireSoul;
        private int _currentNeedWaterSoul;
        private int _currentNeedHolyAnimalSoul;

        internal void Initialize(AgentUnit unit, UIProbabilityInfoCanvas uiProbabilityInfoCanvas, UISpellCanvas uiSpellCanvas, InGameTemplate inGameTemplate)
        {
            _buffAbility = unit.GetAbility<BuffAbility>();
            _uiProbabilityInfoCanvas = uiProbabilityInfoCanvas;
            _inGameTemplate = inGameTemplate;
            _uiSpellCanvas = uiSpellCanvas;

            BindText(typeof(Texts));
            BindButton(typeof(Buttons));

            _needCostText = GetText((int)Texts.NeedCostText);
            _needLandSoulText = GetText((int)Texts.NeedLandSoulText);
            _needFireSoulText = GetText((int)Texts.NeedFireSoulText);
            _needWaterSoulText = GetText((int)Texts.NeedWaterSoulText);
            _needHolyAnimalSoulText = GetText((int)Texts.NeedHolyAnimalSoulText);

            _generateProbabilityLevel = GetText((int)Texts.GenerateProbabilityLevel);
            _landEnforceLevel = GetText((int)Texts.LandEnforceLevel);
            _fireEnforceLevel = GetText((int)Texts.FireEnforceLevel);
            _waterEnforceLevel = GetText((int)Texts.WaterEnforceLevel);
            _holyAnimalEnforceLevel = GetText((int)Texts.HolyAnimalEnforceLevel);

            _generateProbabilityButton = GetButton((int)Buttons.GenerateProbabilityButton);
            _landEnforceButton = GetButton((int)Buttons.LandEnforceButton);
            _fireEnforceButton = GetButton((int)Buttons.FireEnforceButton);
            _waterEnforceButton = GetButton((int)Buttons.WaterEnforceButton);
            _holyAnimalEnforceButton = GetButton((int)Buttons.HolyAnimalEnforceButton);

            _generateProbabilityButton.onClick.AddListener(GenerateProbability);
            _landEnforceButton.onClick.AddListener(LandEnforce);
            _fireEnforceButton.onClick.AddListener(FireEnforce);
            _waterEnforceButton.onClick.AddListener(WaterEnforce);
            _holyAnimalEnforceButton.onClick.AddListener(HolyAnimalEnforce);

            ApplyNeedCost();
            ApplyNeedLandSoul();
            ApplyNeedFireSoul();
            ApplyNeedWaterSoul();
            ApplyNeedHolyAnimalSoul();
            _uiProbabilityInfoCanvas?.Show(_inGameTemplate.GetSpellProbability());

            _agentsystem = BattleManager.Instance.GetSubSystem<AgentSystem>();
            _costSystem = BattleManager.Instance.GetSubSystem<CostSystem>();
            _soulSystem = BattleManager.Instance.GetSubSystem<SoulSystem>();
            _costSystem.onChangedCost += OnChangeCost;
            _soulSystem.onChangedSoul += OnChangeSoul;

            OnChangeCost(100);
            OnChangeSoul(0);
        }

        private void OnDestroy()
        {
            _costSystem.onChangedCost -= OnChangeCost;
            _soulSystem.onChangedSoul -= OnChangeSoul;
        }

        #region 콜백 
        private void OnChangeCost(int cost)
        {
            if (_currentNeedCost > cost)
            {
                _needCostText.color = Color.red;
                _generateProbabilityButton.interactable = false;
            }
            else
            {
                _needCostText.color = Color.white;
                _generateProbabilityButton.interactable = true;
            }
        }

        private void OnChangeSoul(int soul)
        {
            if (_currentNeedLandSoul != 0)
            {
                if (_currentNeedLandSoul > soul)
                {
                    _needLandSoulText.color = Color.red;
                    _landEnforceButton.interactable = false;
                }
                else
                {
                    _needLandSoulText.color = Color.white;
                    _landEnforceButton.interactable = true;
                }
            }

            if (_currentNeedFireSoul != 0)
            {
                if (_currentNeedFireSoul > soul)
                {
                    _needFireSoulText.color = Color.red;
                    _fireEnforceButton.interactable = false;
                }
                else
                {
                    _needFireSoulText.color = Color.white;
                    _fireEnforceButton.interactable = true;
                }
            }

            if (_currentNeedWaterSoul != 0)
            {
                if (_currentNeedWaterSoul > soul)
                {
                    _needWaterSoulText.color = Color.red;
                    _waterEnforceButton.interactable = false;
                }
                else
                {
                    _needWaterSoulText.color = Color.white;
                    _waterEnforceButton.interactable = true;
                }
            }

            if (_currentNeedHolyAnimalSoul != 0)
            {
                if (_currentNeedHolyAnimalSoul > soul)
                {
                    _needHolyAnimalSoulText.color = Color.red;
                    _holyAnimalEnforceButton.interactable = false;
                }
                else
                {
                    _needHolyAnimalSoulText.color = Color.white;
                    _holyAnimalEnforceButton.interactable = true;
                }
            }
        }
        #endregion

        #region 적용
        private void ApplyNeedCost()
        {
            _currentNeedCost = _inGameTemplate.GetNeedCost();
            _needCostText.text = _currentNeedCost.ToString();
            _generateProbabilityLevel.text = _inGameTemplate.spellProbabilityLevel.ToString();
        }

        private void ApplyNeedLandSoul()
        {
            var soul = _inGameTemplate.GetNeedLandSoul();
            
            _currentNeedLandSoul = soul.needSoul;
            _needLandSoulText.text = _currentNeedLandSoul.ToString();
            
            var level = _inGameTemplate.landSoulLevel.ToString();
            _landEnforceLevel.text = level;
            _uiSpellCanvas.UpdateLandEnforceCount(level);

            if (soul.template != null) _buffAbility.ApplyBuff(soul.template, int.MaxValue);
        }

        private void ApplyNeedFireSoul()
        {
            var soul = _inGameTemplate.GetNeedFireSoul();
            
            _currentNeedFireSoul = soul.needSoul;
            _needFireSoulText.text = _currentNeedFireSoul.ToString();

            var level = _inGameTemplate.fireSoulLevel.ToString();
            _fireEnforceLevel.text = level;
            _uiSpellCanvas.UpdateFireEnforceCount(level);

            if (soul.template != null) _buffAbility.ApplyBuff(soul.template, int.MaxValue);
        }

        private void ApplyNeedWaterSoul()
        {
            var soul = _inGameTemplate.GetNeedWaterSoul();
            
            _currentNeedWaterSoul = soul.needSoul;
            _needWaterSoulText.text = _currentNeedWaterSoul.ToString();
            
            var level = _inGameTemplate.waterSoulLevel.ToString();
            _waterEnforceLevel.text = level;
            _uiSpellCanvas.UpdateWaterEnforceCount(level);

            if (soul.template != null) _buffAbility.ApplyBuff(soul.template, int.MaxValue);
        }

        private void ApplyNeedHolyAnimalSoul()
        {
            var soul = _inGameTemplate.GetNeedHolyAnimalSoul();

            _currentNeedHolyAnimalSoul = soul.needSoul;
            _needHolyAnimalSoulText.text = _currentNeedHolyAnimalSoul.ToString();
            _holyAnimalEnforceLevel.text = _inGameTemplate.holyAnimalSoulLevel.ToString();

            if (soul.template != null) 
            {
                var holyAnimals = _agentsystem.GetAllHolyAnimals();
                foreach (var holyAnimal in holyAnimals)
                {
                    holyAnimal.GetAbility<BuffAbility>().ApplyBuff(soul.template, int.MaxValue);
                }
            }
        }
        #endregion

        private void GenerateProbability()
        {
            if (_inGameTemplate.isUpgradeSpellProbabilityLevel)
            {
                bool isMax = _inGameTemplate.UpgradeSpellProbabilityLevel();

                _costSystem.PayCost(_currentNeedCost);
                ApplyNeedCost();
                
                if (isMax)
                {
                    _costSystem.onChangedCost -= OnChangeCost;
                    _generateProbabilityButton.interactable = false;
                    _needCostText.text = "Max";
                }

                _uiProbabilityInfoCanvas?.Show(_inGameTemplate.GetSpellProbability());
            }
        }

        private void LandEnforce()
        {
            if (_inGameTemplate.isUpgradeLandSoulLevel)
            {
                bool isMax = _inGameTemplate.UpgradeLandSoulLevel();

                _soulSystem.PaySoul(_currentNeedLandSoul);
                ApplyNeedLandSoul();
                
                if (isMax)
                {
                    _landEnforceButton.interactable = false;
                    _needLandSoulText.text = "Max";
                }
            }
        }

        private void FireEnforce()
        {
            if (_inGameTemplate.isUpgradeFireSoulLevel)
            {
                bool isMax = _inGameTemplate.UpgradeFireSoulLevel();

                _soulSystem.PaySoul(_currentNeedFireSoul);
                ApplyNeedFireSoul();

                if (isMax)
                {
                    _fireEnforceButton.interactable = false;
                    _needFireSoulText.text = "Max";
                }
            }
        }

        private void WaterEnforce()
        {
            if (_inGameTemplate.isUpgradeWaterSoulLevel)
            {
                bool isMax = _inGameTemplate.UpgradeWaterSoulLevel();

                _soulSystem.PaySoul(_currentNeedWaterSoul);
                ApplyNeedWaterSoul();

                if (isMax)
                {
                    _waterEnforceButton.interactable = false;
                    _needWaterSoulText.text = "Max";
                }
            }
        }

        private void HolyAnimalEnforce()
        {
            if (_inGameTemplate.isUpgradeHolyAnimalSoulLevel)
            {
                bool isMax = _inGameTemplate.UpgradeHolyAnimalSoulLevel();

                _soulSystem.PaySoul(_currentNeedHolyAnimalSoul);
                ApplyNeedHolyAnimalSoul();

                if (isMax)
                {
                    _holyAnimalEnforceButton.interactable = false;
                    _needHolyAnimalSoulText.text = "Max";
                }
            }
        }
    }
}
