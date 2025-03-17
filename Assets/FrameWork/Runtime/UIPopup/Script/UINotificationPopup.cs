using DG.Tweening;
using FrameWork.Editor;
using FrameWork.UIBinding;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.UIPopup
{
    public enum ENotificationType
    {
        BasicNotificationPop,
        SpriteNotificationPop,
        ItemNotificationPop,
    }

    public class UINotificationPopup : UIBase
    {
        [SerializeField] private List<GameObject> _notificationPops = new List<GameObject>();

        [SerializeField, Label("�˾� ���� ������Ʈ")] private Transform _parent;
        [SerializeField, Label("�˾� ���� Y�� ����")] private float _popupOffset;

        private Dictionary<ENotificationType, Stack<GameObject>> _popPools = new Dictionary<ENotificationType, Stack<GameObject>>();
        private List<RectTransform> _activePopList = new List<RectTransform>();

        private void Start()
        {
            foreach (ENotificationType type in Enum.GetValues(typeof(ENotificationType)))
            {
                _popPools[type] = new Stack<GameObject>();
            }
        }

        public void Show(string title, string description, Sprite sprite, ENotificationType notificationType)
        {
            // �˾� ����
            GameObject popup;

            if (_popPools[notificationType].Count > 0)
            {
                popup = _popPools[notificationType].Pop();
                popup.SetActive(true);
            }
            else
            {
                popup = Instantiate(_notificationPops[(int)notificationType], _parent);
            }

            // �˾� Y�� ��ġ ���
            if (popup.TryGetComponent(out RectTransform rect))
            {
                int popupCount = _activePopList.Count;
                rect.anchoredPosition += new Vector2(0, -rect.sizeDelta.y * popupCount - _popupOffset * popupCount);

                _activePopList.Add(rect);
            }

            // �˾� �ʱ�ȭ
            if (popup.TryGetComponent(out UINotificationPop pop))
            {
                if (pop is UISpriteNotificationPop spritePop)
                {
                    spritePop.Initialize(title, description, sprite, notificationType, OnPopupClose);
                }
                else
                {
                    pop.Initialize(title, description, notificationType, OnPopupClose);
                }
            }
        }

        private void OnPopupClose(RectTransform rect, ENotificationType notificationType)
        {
            _activePopList.Remove(rect);

            rect.gameObject.SetActive(false);
            _popPools[notificationType].Push(rect.gameObject);

            for (int index = 0; index < _activePopList.Count; index++)
            {
                _activePopList[index].DOLocalMoveY(rect.rect.height + _popupOffset, 0.3f).SetRelative(true).SetUpdate(true);
            }
        }
    }
}