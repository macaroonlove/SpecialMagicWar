using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class UIPlayingCanvas : MonoBehaviour
    {
        private UISpellCanvas _uiSpellCanvas;
        private UIHolyAnimalCanvas _uiHolyAnimalCanvas;

        private UIGenerateSpellButton _uiGenerateSpellButton;

        private void Awake()
        {
            _uiSpellCanvas = GetComponentInChildren<UISpellCanvas>();
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
            _uiHolyAnimalCanvas?.Initialize(_uiSpellCanvas);
            _uiGenerateSpellButton?.Initialize(_uiSpellCanvas);
        }
    }
}