using FrameWork.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(menuName = "Templates/Library/Player", fileName = "PlayerLibrary", order = 0)]
    public class PlayerLibraryTemplate : ScriptableObject
    {
        public List<AgentTemplate> templates = new List<AgentTemplate>();

        [Header("����")]
        [Label("��� ����"), SerializeField] private bool _isAll;

        [ContextMenu("���ͷ� PlayerTemplate ã��")]
        public void FindAllRefresh()
        {
            templates.Clear();
            var AssetsGUIDArray = AssetDatabase.FindAssets("t:AgentTemplate", new[] { "Assets/" });
            foreach (var guid in AssetsGUIDArray)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var template = AssetDatabase.LoadAssetAtPath(path, typeof(AgentTemplate)) as AgentTemplate;

                bool isInclude = _isAll;

                if (template != null && isInclude)
                {
                    templates.Add(template);
                }
            }
            templates.Sort((t1, t2) => t1.id.CompareTo(t2.id));

            // ���� ������ ����
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}
