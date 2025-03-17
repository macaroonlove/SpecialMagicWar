using FrameWork.Editor;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(menuName = "Templates/Status/GlobalStatus", fileName = "GlobalStatus", order = 2)]
    public class GlobalStatusTemplate : ScriptableObject
    {
        [HideInInspector, SerializeField] private Sprite _sprite;

        [HideInInspector, SerializeField] private string _displayName;
        [HideInInspector, SerializeField] private string _description;

        [HideInInspector, SerializeField] private UIFX _applyFX;
        [HideInInspector, SerializeField] private UIFX _removeFX;

        [HideInInspector]
        public List<Effect> effects = new List<Effect>();

        #region ������Ƽ
        public Sprite sprite => _sprite;
        public string displayName => _displayName;
        public string description => _description;

        public UIFX applyFX => _applyFX;
        public UIFX removeFX => _removeFX;
        #endregion

        #region �� ���� �޼���
        public void SetDisplayName(string name)
        {
            _displayName = name;
        }
        #endregion
    }
}

#if UNITY_EDITOR
namespace SpecialMagicWar.Editor
{
    using System;
    using SpecialMagicWar.Core;
    using UnityEditor;
    using UnityEditorInternal;

    [CustomEditor(typeof(GlobalStatusTemplate)), CanEditMultipleObjects]
    public class GlobalStatusTemplateEditor : EffectEditor
    {
        private GlobalStatusTemplate _target;

        private SerializedProperty _sprite;
        private SerializedProperty _displayName;
        private SerializedProperty _description;
        private SerializedProperty _applyFX;
        private SerializedProperty _removeFX;

        private ReorderableList _effectsList;
        private Effect _currentEffect;

        private void OnEnable()
        {
            _target = target as GlobalStatusTemplate;

            _sprite = serializedObject.FindProperty("_sprite");
            _displayName = serializedObject.FindProperty("_displayName");
            _description = serializedObject.FindProperty("_description");
            _applyFX = serializedObject.FindProperty("_applyFX");
            _removeFX = serializedObject.FindProperty("_removeFX");

            CreateEffectList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.BeginHorizontal();

            _sprite.objectReferenceValue = EditorGUILayout.ObjectField(_sprite.objectReferenceValue, typeof(Sprite), false, GUILayout.Width(96), GUILayout.Height(96));

            EditorGUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("���� ���� �̸�", GUILayout.Width(80));
            EditorGUILayout.PropertyField(_displayName, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("���� ���� ����", GUILayout.Width(80));
            _description.stringValue = EditorGUILayout.TextArea(_description.stringValue, GUILayout.Height(74));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("�����̻� ���� �� FX", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_applyFX, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("�����̻� ���� �� FX", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_removeFX, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            _effectsList?.DoLayoutList();

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(this);
            }
        }

        #region EffectList
        private void InitMenu_Effects()
        {
            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("��� �߰� ȹ��"), false, CreateEffectCallback, typeof(GoldGainAdditionalDataEffect));
            menu.AddItem(new GUIContent("��� ȹ�淮 ����"), false, CreateEffectCallback, typeof(GoldGainIncreaseDataEffect));
            menu.AddItem(new GUIContent("��� ȹ�淮 ��¡��϶�"), false, CreateEffectCallback, typeof(GoldGainMultiplierDataEffect));

            menu.AddItem(new GUIContent("�ڽ�Ʈ ���� �ӵ� ����"), false, CreateEffectCallback, typeof(CostRecoveryTimeAdditionalDataEffect));
            menu.AddItem(new GUIContent("�ڽ�Ʈ ���� �ӵ� ����"), false, CreateEffectCallback, typeof(CostRecoveryTimeIncreaseDataEffect));
            menu.AddItem(new GUIContent("�ڽ�Ʈ ���� �ӵ� ��¡��϶�"), false, CreateEffectCallback, typeof(CostRecoveryTimeMultiplierDataEffect));

            menu.ShowAsContext();
        }

        private void CreateEffectList()
        {
            _effectsList = SetupReorderableList("Global Status Effects", _target.effects,
                (rect, x) =>
                {
                },
                (x) =>
                {
                    _currentEffect = x;
                },
                () =>
                {
                    InitMenu_Effects();
                },
                (x) =>
                {
                    DestroyImmediate(_currentEffect, true);
                    _currentEffect = null;
                    EditorUtility.SetDirty(target);
                });

            _effectsList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var element = _target.effects[index];

                if (element != null)
                {
                    rect.y += 2;
                    rect.width -= 10;
                    rect.height = EditorGUIUtility.singleLineHeight;

                    var label = element.GetDescription();
                    EditorGUI.LabelField(rect, label, EditorStyles.boldLabel);
                    rect.y += 5;
                    rect.y += EditorGUIUtility.singleLineHeight;

                    element.Draw(rect);

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(element);
                    }
                }
            };

            _effectsList.elementHeightCallback = (index) =>
            {
                var element = _target.effects[index];
                return element.GetHeight();
            };
        }

        private void CreateEffectCallback(object obj)
        {
            var effect = ScriptableObject.CreateInstance((Type)obj) as Effect;

            if (effect != null)
            {
                effect.hideFlags = HideFlags.HideInHierarchy;
                _target.effects.Add(effect);

                var template = target as GlobalStatusTemplate;
                var path = AssetDatabase.GetAssetPath(template);
                AssetDatabase.AddObjectToAsset(effect, path);
                EditorUtility.SetDirty(template);
            }
        }
        #endregion
    }
}
#endif