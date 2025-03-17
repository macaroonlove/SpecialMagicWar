using FrameWork.Editor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// ��Ƽ�� ������ ȿ���� �����Ű�� �ý���
    /// </summary>
    public class ActiveItemSystem : MonoBehaviour, ICoreSystem
    {
        [SerializeField, ReadOnly] private List<ActiveItemTemplate> _selectedItems = new List<ActiveItemTemplate>();

        [SerializeField, ReadOnly] private List<ActiveItemTemplate> _items = new List<ActiveItemTemplate>();

#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] private List<ActiveItemTemplate> _debugItems = new List<ActiveItemTemplate>();
#endif

        public void Initialize()
        {
#if UNITY_EDITOR
            _selectedItems.AddRange(_debugItems);
#endif
        }

        public void Deinitialize()
        {
            
        }

        /// <summary>
        /// ������ �߰�
        /// </summary>
        public void AddItem(ActiveItemTemplate template)
        {
            if (_items.Contains(template))
            {
#if UNITY_EDITOR
                Debug.LogError($"�������� �ߺ��Ǿ����ϴ�. {template.displayName}");
#endif
                return;
            }

            _items.Add(template);
        }

        /// <summary>
        /// ������ �ҷ�����
        /// </summary>
        public ActiveItemTemplate GetSelectedItem(int index)
        {
            if (index >= _selectedItems.Count) return null;

            return _selectedItems[index];
        }

        private void OnDestroy()
        {            
            _items.Clear();
        }
    }
}