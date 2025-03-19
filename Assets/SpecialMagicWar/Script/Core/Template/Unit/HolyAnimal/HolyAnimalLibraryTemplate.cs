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

        [ContextMenu("���ͷ� HolyAnimalTemplate ã��")]
        public void FindAllRefresh()
        {
            templates.Clear();
            var AssetsGUIDArray = AssetDatabase.FindAssets("t:HolyAnimalTemplate", new[] { "Assets/" });
            foreach (var guid in AssetsGUIDArray)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var template = AssetDatabase.LoadAssetAtPath(path, typeof(HolyAnimalTemplate)) as HolyAnimalTemplate;

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