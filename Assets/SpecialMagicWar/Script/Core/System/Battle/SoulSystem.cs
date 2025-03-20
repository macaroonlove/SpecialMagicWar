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

        internal event UnityAction<int> onChangedSoul;

        public void Initialize()
        {
            SetSoul(100);
        }

        public void Deinitialize()
        {

        }

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
    }
}