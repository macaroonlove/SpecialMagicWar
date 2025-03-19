using FrameWork;
using FrameWork.UIBinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpecialMagicWar.Core
{
    public class UIPlayingCanvas : UIBase
    {
        #region ¹ÙÀÎµù
        enum Toggles
        {
            BountyToggle,
            HolyAnimalToggle,
            EnforceToggle,
            MiningToggle,
        }
        #endregion

        [SerializeField] private InGameTemplate _template;

        private UISpellCanvas _uiSpellCanvas;
        private UIBountyCanvas _uiBountyCanvas;
        private UIBountyLockCanvas _uiBountyLockCanvas;
        private UIHolyAnimalCanvas _uiHolyAnimalCanvas;
        private UIEnforceCanvas _uiEnforceCanvas;
        private UIProbabilityInfoCanvas _uiProbabilityInfoCanvas;

        private UIGenerateSpellButton _uiGenerateSpellButton;

        private Toggle _bountyToggle;
        private Toggle _holyAnimalToggle;
        private Toggle _enforceToggle;
        private Toggle _miningToggle;

        protected override void Initialize()
        {
            BindToggle(typeof(Toggles));

            _bountyToggle = GetToggle((int)Toggles.BountyToggle);
            _holyAnimalToggle = GetToggle((int)Toggles.HolyAnimalToggle);
            _enforceToggle = GetToggle((int)Toggles.EnforceToggle);
            _miningToggle = GetToggle((int)Toggles.MiningToggle);

            _uiSpellCanvas = GetComponentInChildren<UISpellCanvas>();
            _uiBountyCanvas = GetComponentInChildren<UIBountyCanvas>(true);
            _uiBountyLockCanvas = GetComponentInChildren<UIBountyLockCanvas>();
            _uiHolyAnimalCanvas = GetComponentInChildren<UIHolyAnimalCanvas>(true);
            _uiEnforceCanvas = GetComponentInChildren<UIEnforceCanvas>(true);
            _uiProbabilityInfoCanvas = GetComponentInChildren<UIProbabilityInfoCanvas>(true);
            _uiGenerateSpellButton = GetComponentInChildren<UIGenerateSpellButton>();
            
            BattleManager.Instance.playerCreateSystem.onCreatePlayer += OnCreatePlayer;
        }

        void OnDestroy()
        {
            BattleManager.Instance.playerCreateSystem.onCreatePlayer -= OnCreatePlayer;
        }

        private void OnCreatePlayer(AgentUnit unit)
        {
            _template.Initialize();
            _uiProbabilityInfoCanvas?.Initialize();
            _uiSpellCanvas?.Initialize(unit, _template);
            _uiBountyCanvas?.Initialize(_bountyToggle, _uiBountyLockCanvas);
            _uiHolyAnimalCanvas?.Initialize(_uiSpellCanvas, _holyAnimalToggle);
            _uiEnforceCanvas?.Initialize(_uiProbabilityInfoCanvas, _uiSpellCanvas, _template);
            _uiGenerateSpellButton?.Initialize(_uiSpellCanvas);
        }
    }
}