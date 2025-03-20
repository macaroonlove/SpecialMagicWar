using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// 전투에서 사용하는 Soul 값을 관리하는 클래스
    /// </summary>
    public class SoulSystem : MonoBehaviour, IBattleSystem
    {
        [SerializeField] private ObscuredIntVariable _soulVariable;
        [SerializeField] private ObscuredIntVariable _bot1SoulVariable;
        [SerializeField] private ObscuredIntVariable _bot2SoulVariable;
        [SerializeField] private ObscuredIntVariable _bot3SoulVariable;

        internal event UnityAction<int> onChangedSoul;
        internal event UnityAction<int> onChangedBot1Soul;
        internal event UnityAction<int> onChangedBot2Soul;
        internal event UnityAction<int> onChangedBot3Soul;

        public void Initialize()
        {
            SetSoul(100);

            int botCount = BattleManager.Instance.botCount;
            for (int i = 1; i <= botCount; i++)
            {
                SetBotSoul(0, i);
            }
        }

        public void Deinitialize()
        {

        }

        #region 플레이어
        internal void AddSoul(int value)
        {
            SetSoul(_soulVariable.Value + value);
        }

        internal void PaySoul(int value)
        {
            int newSoul = _soulVariable.Value - value;
            if (newSoul >= 0)
            {
                SetSoul(newSoul);
            }
        }

        private void SetSoul(int newSoul)
        {
            _soulVariable.Value = newSoul;

            onChangedSoul?.Invoke(newSoul);
        }
        #endregion

        #region 봇
        internal void AddBotSoul(int value, int index)
        {
            if (index == 1) SetBotSoul(_bot1SoulVariable.Value + value, index);
            else if (index == 2) SetBotSoul(_bot2SoulVariable.Value + value, index);
            else if (index == 3) SetBotSoul(_bot3SoulVariable.Value + value, index);
        }

        internal void PayBotSoul(int value, int index)
        {
            int newSoul = 0;

            if (index == 1) newSoul = _bot1SoulVariable.Value - value;
            else if (index == 2) newSoul = _bot2SoulVariable.Value - value;
            else if (index == 3) newSoul = _bot3SoulVariable.Value - value;

            if (newSoul >= 0)
            {
                SetBotSoul(newSoul, index);
            }
        }

        private void SetBotSoul(int newSoul, int index)
        {
            if (index == 1)
            {
                _bot1SoulVariable.Value = newSoul;
                onChangedBot1Soul?.Invoke(newSoul);
            }
            else if (index == 2)
            {
                _bot2SoulVariable.Value = newSoul;
                onChangedBot2Soul?.Invoke(newSoul);
            }
            else if (index == 3)
            {
                _bot3SoulVariable.Value = newSoul;
                onChangedBot3Soul?.Invoke(newSoul);
            }
        }
        #endregion
    }
}