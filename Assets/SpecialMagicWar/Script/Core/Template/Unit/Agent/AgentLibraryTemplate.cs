using FrameWork.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(menuName = "Templates/Library/Agent", fileName = "AgentLibrary", order = 0)]
    public class AgentLibraryTemplate : ScriptableObject
    {
        public List<AgentTemplate> templates = new List<AgentTemplate>();

        [Header("����")]
        [Label("��� ����"), SerializeField] private bool _isAll;
        [Condition("_isAll", false)]
        [Label("���� ����"), SerializeField] private bool _isOwned;

        [ContextMenu("���ͷ� AgentTemplate ã��")]
        public void FindAllRefresh()
        {
            templates.Clear();
            var AssetsGUIDArray = AssetDatabase.FindAssets("t:AgentTemplate", new[] { "Assets/" });
            foreach (var guid in AssetsGUIDArray)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var template = AssetDatabase.LoadAssetAtPath(path, typeof(AgentTemplate)) as AgentTemplate;

                bool isInclude = _isAll;
                isInclude |= _isOwned && template.isOwned;

                if (template != null && isInclude)
                {
                    templates.Add(template);
                }
            }
            // ���� ������ ����
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}
