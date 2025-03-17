using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// ��带 �����ϴ� �ý���
    /// </summary>
    public class GoldSystem : MonoBehaviour, ICoreSystem
    {
        [SerializeField] private ObscuredIntVariable _goldVariable;

        private GlobalStatusSystem _globalStatusSystem;

        internal int currentGold => _goldVariable.Value;

        internal event UnityAction<int> onChangeGold;

        public void Initialize()
        {
            _globalStatusSystem = CoreManager.Instance.GetSubSystem<GlobalStatusSystem>();

            // TODO: Template���� ���� ��� �޾ƿ����� ����
            SetGold(100);
        }

        public void Deinitialize()
        {
            _globalStatusSystem = null;
        }

        public void AddGold(int gold)
        {
            #region ��� ȹ�淮 ���
            float result = gold;

            #region �߰�������
            foreach (var effect in _globalStatusSystem.GoldGainAdditionalDataEffects)
            {
                result += effect.value;
            }
            #endregion

            #region ����������
            float increase = 1;

            foreach (var effect in _globalStatusSystem.GoldGainIncreaseDataEffects)
            {
                increase += effect.value;
            }

            result *= increase;
            #endregion

            #region ��¡��϶�
            foreach (var effect in _globalStatusSystem.GoldGainMultiplierDataEffects)
            {
                result *= effect.value;
            }
            #endregion

            gold = (int)result;
            #endregion

            SetGold(_goldVariable.Value + gold);
        }

        internal void PayGold(int gold)
        {

        }

        private void SetGold(int gold)
        {
            _goldVariable.SetValue(gold);

            onChangeGold?.Invoke(_goldVariable.Value);
        }
    }
}