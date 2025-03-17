using UnityEngine;
using UnityEngine.UI;

namespace FrameWork.GameSettings
{
    [AddComponentMenu("GameSettings/UI/Controll Mode Toggle")]
    public class ControllModeToggle : MonoBehaviour
    {
        [SerializeField] private Toggle targetElement;
        [SerializeField] private GameSettingsManager.ControlTarget controlTarget = GameSettingsManager.ControlTarget.Sprint;

        private void Reset()
        {
            targetElement = GetComponentInChildren<Toggle>();
        }

        private void Start()
        {
            if (targetElement == null)
            {
                targetElement = GetComponentInChildren<Toggle>();
                if (targetElement == null)
                {
#if UNITY_EDITOR
                    Debug.LogError("[ControllModeToggle] Toggle ������Ʈ�� ã�� �� �����ϴ�.", gameObject);
#endif
                    return;
                }
            }

            targetElement.isOn = GameSettingsManager.GetControlMode(controlTarget);
            targetElement.onValueChanged.AddListener(OnValueChange);
        }

        private void OnValueChange(bool isOn)
        {
            GameSettingsManager.SetControlMode(controlTarget, isOn);
        }
    }
}