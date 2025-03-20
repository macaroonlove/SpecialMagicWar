using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// 전투에서 사용하는 Cost 값을 관리하는 클래스
    /// </summary>
    public class CostSystem : MonoBehaviour, IBattleSystem
    {
        [SerializeField] private ObscuredIntVariable _costVariable;
        [SerializeField] private ObscuredIntVariable _bot1CostVariable;
        [SerializeField] private ObscuredIntVariable _bot2CostVariable;
        [SerializeField] private ObscuredIntVariable _bot3CostVariable;

        internal event UnityAction<int> onChangedCost;
        internal event UnityAction<int> onChangedBot1Cost;
        internal event UnityAction<int> onChangedBot2Cost;
        internal event UnityAction<int> onChangedBot3Cost;

        public void Initialize()
        {
            SetCost(10000);

            int botCount = BattleManager.Instance.botCount;
            for (int i = 1; i <= botCount; i++)
            {
                SetBotCost(100, i);
            }
        }

        public void Deinitialize()
        {

        }

        #region 플레이어
        internal void AddCost(int value)
        {
            SetCost(_costVariable.Value + value);
        }

        internal void PayCost(int value)
        {
            int newCost = _costVariable.Value - value;
            if (newCost >= 0)
            {
                SetCost(newCost);
            }
        }

        private void SetCost(int newCost)
        {
            _costVariable.Value = newCost;

            onChangedCost?.Invoke(newCost);
        }
        #endregion
        
        #region 봇
        internal void AddBotCost(int value, int index)
        {
            if (index == 1) SetBotCost(_bot1CostVariable.Value + value, index);
            else if (index == 2) SetBotCost(_bot2CostVariable.Value + value, index);
            else if (index == 3) SetBotCost(_bot3CostVariable.Value + value, index);
        }

        internal void PayBotCost(int value, int index)
        {
            int newCost = 0;

            if (index == 1) newCost = _bot1CostVariable.Value - value;
            else if (index == 2) newCost = _bot2CostVariable.Value - value;
            else if (index == 3) newCost = _bot3CostVariable.Value - value;
            
            if (newCost >= 0)
            {
                SetBotCost(newCost, index);
            }
        }

        private void SetBotCost(int newCost, int index)
        {
            if (index == 1)
            {
                _bot1CostVariable.Value = newCost;
                onChangedBot1Cost?.Invoke(newCost);
            }
            else if (index == 2)
            {
                _bot2CostVariable.Value = newCost;
                onChangedBot2Cost?.Invoke(newCost);
            }
            else if (index == 3)
            {
                _bot3CostVariable.Value = newCost;
                onChangedBot3Cost?.Invoke(newCost);
            }
        }
        #endregion
    }
}