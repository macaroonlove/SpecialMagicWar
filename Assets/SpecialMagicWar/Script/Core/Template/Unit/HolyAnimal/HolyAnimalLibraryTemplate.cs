using FrameWork.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(menuName = "Templates/Library/HolyAnimal", fileName = "HolyAnimalLibrary", order = 0)]
    public class HolyAnimalLibraryTemplate : ScriptableObject
    {
        public List<HolyAnimalTemplate> templates = new List<HolyAnimalTemplate>();

        [Header("����")]
        [Label("��� ����"), SerializeField] private bool _isAll;
        [Condition("_isAll", false)]
        [Label("���� ����"), SerializeField] private bool _isOwned;

        [ContextMenu("���ͷ� AgentTemplate ã��")]
        public void FindAllRefresh()
        {
            templates.Clear();
            var AssetsGUIDArray = AssetDatabase.FindAssets("t:HolyAnimalTemplate", new[] { "Assets/" });
            foreach (var guid in AssetsGUIDArray)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var template = AssetDatabase.LoadAssetAtPath(path, typeof(HolyAnimalTemplate)) as HolyAnimalTemplate;

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