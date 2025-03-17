using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FrameWork.UIPopup
{
    public class UIItemNotificationPop : UINotificationPop
    {
        #region ���ε�
        enum Images
        {
            Sprite,
        }
        #endregion

        [SerializeField] private List<Sprite> sprites = new List<Sprite>();

        protected override void Initialize()
        {
            base.Initialize();

            BindImage(typeof(Images));
        }

        public override void Initialize(string title, string desciption, ENotificationType notificationType, UnityAction<RectTransform, ENotificationType> action)
        {
            int index = GetIndex(title);

            if (index == -1)
            {
                action?.Invoke(transform as RectTransform, notificationType);
            }

            GetImage((int)Images.Sprite).sprite = sprites[index];

            base.Initialize(title, desciption, notificationType, action);
        }

        private int GetIndex(string title)
        {
            switch (title)
            {
                case "���":
                    return 0;
                default:
                    Debug.LogError($"�����ۿ� {title} �̹����� ��ϵǾ� ���� �ʽ��ϴ�.");
                    return -1;
            }
        }
    }
}