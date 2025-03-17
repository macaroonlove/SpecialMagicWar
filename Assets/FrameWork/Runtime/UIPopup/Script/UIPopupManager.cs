using UnityEngine;
using UnityEngine.Events;

namespace FrameWork.UIPopup
{
    public class UIPopupManager : Singleton<UIPopupManager>
    {
        [SerializeField] private UINotificationPopup _notificationPopup;
        [SerializeField] private UIConfirmPopup _confirmPopup;
        [SerializeField] private UIConfirmCancelPopup _confirmCancelPopup;
        [SerializeField] private UIInputPopup _inputPopup;

        /// <summary>
        /// ���� ��ܿ��� ������ �����ִ� �˾�
        /// </summary>
        public void ShowNotificationPopup(string title, string description, ENotificationType notificationType = ENotificationType.BasicNotificationPop, Sprite sprite = null)
        {
            _notificationPopup.Show(title, description, sprite, notificationType);
        }

        /// <summary>
        /// Ȯ�� ��ư�� �ִ� �˾�
        /// </summary>
        public void ShowConfirmPopup(string context, UnityAction action = null)
        {
            _confirmPopup.Show(context);

            _confirmPopup.OnResult += () =>
            {
                action?.Invoke();
            };
        }

        /// <summary>
        /// Ȯ��, ��� ��ư�� �ִ� �˾�
        /// </summary>
        public void ShowConfirmCancelPopup(string context, UnityAction<bool> action = null)
        {
            _confirmCancelPopup.Show(context);

            _confirmCancelPopup.OnResult += (result) =>
            {
                action?.Invoke(result);
            };
        }

        /// <summary>
        /// ��ǲ �ʵ尡 �ִ� �˾�
        /// </summary>
        public void ShowInputPopup(string context, UnityAction<string> action = null)
        {
            _inputPopup.Show(context);

            _inputPopup.OnResult += (string str) =>
            {
                action?.Invoke(str);
            };
        }
    }
}