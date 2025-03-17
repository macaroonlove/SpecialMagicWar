using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace SpecialMagicWar.Core
{
    public class InputSystem : MonoBehaviour, IBattleSystem
    {
        private PlayerInput _playerInput;

        public event UnityAction onSkill_1;
        public event UnityAction onSkill_2;
        public event UnityAction onSkill_3;
        public event UnityAction onSkill_4;

        public event UnityAction onItem_1;
        public event UnityAction onItem_2;
        public event UnityAction onItem_3;

        public void Initialize()
        {
            _playerInput = GetComponent<PlayerInput>();
            if (_playerInput != null)
                _playerInput.enabled = true;
        }

        public void Deinitialize()
        {
            if (_playerInput != null)
                _playerInput.enabled = false;
        }

#if ENABLE_INPUT_SYSTEM
        public void OnSkill1(InputValue value)
        {
            onSkill_1?.Invoke();
        }

        public void OnSkill2(InputValue value)
        {
            onSkill_2?.Invoke();
        }

        public void OnSkill3(InputValue value)
        {
            onSkill_3?.Invoke();
        }

        public void OnSkill4(InputValue value)
        {
            onSkill_4?.Invoke();
        }

        public void OnItem1(InputValue value)
        {
            onItem_1?.Invoke();
        }

        public void OnItem2(InputValue value)
        {
            onItem_2?.Invoke();
        }

        public void OnItem3(InputValue value)
        {
            onItem_3?.Invoke();
        }
#endif
    }
}
