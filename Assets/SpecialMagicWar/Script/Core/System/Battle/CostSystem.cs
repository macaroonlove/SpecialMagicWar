using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// �������� ����ϴ� Cost ���� �����ϴ� Ŭ����
    /// </summary>
    public class CostSystem : MonoBehaviour, IBattleSystem
    {
        [SerializeField] private ObscuredIntVariable _costVariable;

        internal event UnityAction<int> onChangedCost;

        public void Initialize()
        {
            SetCost(1000);
        }

        public void Deinitialize()
        {

        }

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
    }
}