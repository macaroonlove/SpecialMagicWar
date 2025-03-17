using TMPro;
using UnityEngine;

namespace FrameWork.GameSettings
{
    [AddComponentMenu("GameSettings/UI/DisplayName InputField")]
    public class DisplayNameInputField : MonoBehaviour
    {
        [SerializeField] private TMP_InputField targetElement;

        private void Reset()
        {
            targetElement = GetComponentInChildren<TMP_InputField>();
        }

        private void Start()
        {
            if (targetElement == null)
            {
                targetElement = GetComponentInChildren<TMP_InputField>();
                if (targetElement == null)
                {
#if UNITY_EDITOR
                    Debug.LogError("[DisplayNameInputField] TMP_InputField ������Ʈ�� ã�� �� �����ϴ�.", gameObject);
#endif
                    return;
                }
            }

            targetElement.text = GameSettingsManager.DisplayNameText;
            targetElement.onEndEdit.AddListener(OnEndEdit);
        }

        private void OnEndEdit(string value)
        {
            GameSettingsManager.DisplayNameText = value;
        }
    }
}