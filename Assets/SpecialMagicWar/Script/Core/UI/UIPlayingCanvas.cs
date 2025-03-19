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

        private UISpellCanvas _uiSpellCanvas;
        private UIBountyCanvas _uiBountyCanvas;
        private UIBountyLockCanvas _uiBountyLockCanvas;
        private UIHolyAnimalCanvas _uiHolyAnimalCanvas;

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
            _uiGenerateSpellButton = GetComponentInChildren<UIGenerateSpellButton>();
            
            BattleManager.Instance.playerCreateSystem.onCreatePlayer += OnCreatePlayer;
        }

        void OnDestroy()
        {
            BattleManager.Instance.playerCreateSystem.onCreatePlayer -= OnCreatePlayer;
        }

        private void OnCreatePlayer(AgentUnit unit)
        {
            _uiSpellCanvas?.Initialize(unit);
            _uiBountyCanvas?.Initialize(_bountyToggle, _uiBountyLockCanvas);
            _uiHolyAnimalCanvas?.Initialize(_uiSpellCanvas, _holyAnimalToggle);
            _uiGenerateSpellButton?.Initialize(_uiSpellCanvas);
        }
    }
}