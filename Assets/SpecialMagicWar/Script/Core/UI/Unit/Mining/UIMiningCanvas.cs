using FrameWork.UIBinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpecialMagicWar.Core
{
    public class UIMiningCanvas : UIBase
    {
        #region ¹ÙÀÎµù
        enum Texts
        {
            NeedLowSoulText,
            NeedMidiumSoulText,
            NeedHighSoulText,
        }
        enum Buttons
        {
            LowScrollButton,
            MidiumScrollButton,
            HighScrollButton,
        }
        #endregion

        private SoulSystem _soulSystem;
        private UISpellCanvas _uiSpellCanvas;
        private Toggle _miningToggle;

        private TextMeshProUGUI _needLowSoulText;
        private TextMeshProUGUI _needMidiumSoulText;
        private TextMeshProUGUI _needHighSoulText;
        private Button _lowScrollButton;
        private Button _midiumScrollButton;
        private Button _highScrollButton;

        internal void Initialize(UISpellCanvas uiSpellCanvas, Toggle miningToggle)
        {
            _uiSpellCanvas = uiSpellCanvas;
            _miningToggle = miningToggle;

            BindText(typeof(Texts));
            BindButton(typeof(Buttons));

            _needLowSoulText = GetText((int)Texts.NeedLowSoulText);
            _needMidiumSoulText = GetText((int)Texts.NeedMidiumSoulText);
            _needHighSoulText = GetText((int)Texts.NeedHighSoulText);

            _lowScrollButton = GetButton((int)Buttons.LowScrollButton);
            _midiumScrollButton = GetButton((int)Buttons.MidiumScrollButton);
            _highScrollButton = GetButton((int)Buttons.HighScrollButton);

            _lowScrollButton.onClick.AddListener(BuyLowScroll);
            _midiumScrollButton.onClick.AddListener(BuyMidiumScroll);
            _highScrollButton.onClick.AddListener(BuyHighScroll);

            _soulSystem = BattleManager.Instance.GetSubSystem<SoulSystem>();
            _soulSystem.onChangedSoul += OnChangeSoul;
        }

        private void OnDestroy()
        {
            _soulSystem.onChangedSoul -= OnChangeSoul;
        }

        private void OnChangeSoul(int soul)
        {
            if (1 > soul)
            {
                _needLowSoulText.color = Color.red;
                _lowScrollButton.interactable = false;
            }
            else
            {
                _needLowSoulText.color = Color.white;
                _lowScrollButton.interactable = true;
            }

            if (3 > soul)
            {
                _needMidiumSoulText.color = Color.red;
                _midiumScrollButton.interactable = false;
            }
            else
            {
                _needMidiumSoulText.color = Color.white;
                _midiumScrollButton.interactable = true;
            }

            if (7 > soul)
            {
                _needHighSoulText.color = Color.red;
                _highScrollButton.interactable = false;
            }
            else
            {
                _needHighSoulText.color = Color.white;
                _highScrollButton.interactable = true;
            }
        }

        private void BuyLowScroll()
        {
            _soulSystem.PaySoul(1);
            _uiSpellCanvas.GenerateRarityUpperRandomSpell(ERarity.Epic);

            _miningToggle.isOn = false;
        }

        private void BuyMidiumScroll()
        {
            _soulSystem.PaySoul(3);
            _uiSpellCanvas.GenerateRarityUpperRandomSpell(ERarity.Legend);

            _miningToggle.isOn = false;
        }

        private void BuyHighScroll()
        {
            _soulSystem.PaySoul(7);
            _uiSpellCanvas.GenerateRarityUpperRandomSpell(ERarity.Beginning);

            _miningToggle.isOn = false;
        }
    }
}