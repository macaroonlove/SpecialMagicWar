using FrameWork.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(menuName = "Templates/Library/Enemy", fileName = "EnemyLibrary", order = 0)]
    public class EnemyLibraryTemplate : ScriptableObject
    {
        public List<EnemyTemplate> templates = new List<EnemyTemplate>();

        [Header("����")]
        [Label("��� ����"), SerializeField] private bool _isAll;

        [ContextMenu("���ͷ� EnemyTemplate ã��")]
        public void FindAllRefresh()
        {
            templates.Clear();
            var AssetsGUIDArray = AssetDatabase.FindAssets("t:EnemyTemplate", new[] { "Assets/" });
            foreach (var guid in AssetsGUIDArray)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var template = AssetDatabase.LoadAssetAtPath(path, typeof(EnemyTemplate)) as EnemyTemplate;

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
