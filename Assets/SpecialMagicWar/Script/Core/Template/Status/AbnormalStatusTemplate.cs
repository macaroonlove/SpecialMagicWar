using FrameWork.Editor;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(menuName = "Templates/Status/AbnormalStatus", fileName = "AbnormalStatus", order = 1)]
    public class AbnormalStatusTemplate : ScriptableObject
    {
        [HideInInspector, SerializeField] private Sprite _sprite;

        [HideInInspector, SerializeField] private string _displayName;
        [HideInInspector, SerializeField] private string _description;

        [HideInInspector, SerializeField] private float _delay;

        [HideInInspector, SerializeField] private bool _useHitCountLimit;
        [HideInInspector, SerializeField] private int _hitCount;

        [HideInInspector]
        public List<Effect> effects = new List<Effect>();

        [HideInInspector, SerializeField] private FX _applyFX;
        [HideInInspector, SerializeField] private FX _removeFX;

        #region ������Ƽ
        public Sprite sprite => _sprite;
        public string displayName => _displayName;
        public string description => _description;

        public float delay => _delay;

        public bool useHitCountLimit => _useHitCountLimit;
        public int hitCount => _hitCount;

        public FX applyFX => _applyFX;
        public FX removeFX => _removeFX;
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

    [CustomEditor(typeof(AbnormalStatusTemplate)), CanEditMultipleObjects]
    public class AbnormalStatusTemplateEditor : EffectEditor
    {
        private AbnormalStatusTemplate _target;

        private SerializedProperty _sprite;
        private SerializedProperty _displayName;
        private SerializedProperty _description;
        private SerializedProperty _delay;
        private SerializedProperty _useHitCountLimit;
        private SerializedProperty _hitCount;
        private SerializedProperty _applyFX;
        private SerializedProperty _removeFX;

        private ReorderableList _effectsList;
        private Effect _currentEffect;

        private void OnEnable()
        {
            _target = target as AbnormalStatusTemplate;

            _sprite = serializedObject.FindProperty("_sprite");
            _displayName = serializedObject.FindProperty("_displayName");
            _description = serializedObject.FindProperty("_description");
            _delay = serializedObject.FindProperty("_delay");
            _useHitCountLimit = serializedObject.FindProperty("_useHitCountLimit");
            _hitCount = serializedObject.FindProperty("_hitCount");
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
            GUILayout.Label("�����̻� �̸�", GUILayout.Width(80));
            EditorGUILayout.PropertyField(_displayName, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("�����̻� ����", GUILayout.Width(80));
            _description.stringValue = EditorGUILayout.TextArea(_description.stringValue, GUILayout.Height(74));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("���� �ð�", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_delay, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("�ǰ� ��, �����̻��� ��������", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_useHitCountLimit, GUIContent.none);
            GUILayout.EndHorizontal();

            if (_useHitCountLimit.boolValue == true)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("�ǰ� Ƚ��", GUILayout.Width(192));
                EditorGUILayout.PropertyField(_hitCount, GUIContent.none);
                GUILayout.EndHorizontal();
            }

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

            menu.AddItem(new GUIContent("�̵� �Ұ�"), false, CreateEffectCallback, typeof(UnableToMoveEffect));
            menu.AddItem(new GUIContent("���� �Ұ�"), false, CreateEffectCallback, typeof(UnableToAttackEffect));
            menu.AddItem(new GUIContent("ȸ�� �Ұ�"), false, CreateEffectCallback, typeof(UnableToHealEffect));
            menu.AddItem(new GUIContent("��ų ��� �Ұ�"), false, CreateEffectCallback, typeof(UnableToSkillEffect));
            menu.AddItem(new GUIContent("�̵��ӵ� ����"), false, CreateEffectCallback, typeof(MoveIncreaseDataEffect));
            menu.AddItem(new GUIContent("���� ���׷� ����"), false, CreateEffectCallback, typeof(PhysicalResistanceIncreaseDataEffect));
            menu.AddItem(new GUIContent("���� ���׷� ����"), false, CreateEffectCallback, typeof(MagicResistanceIncreaseDataEffect));
            menu.AddItem(new GUIContent("�޴� ���ط� ����"), false, CreateEffectCallback, typeof(ReceiveDamageIncreaseDataEffect));
            menu.AddItem(new GUIContent("�ִ� ü�� ��� �ʴ� ü�� ȸ����"), false, CreateEffectCallback, typeof(HPRecoveryPerSecByMaxHPIncreaseDataEffect));

            menu.ShowAsContext();
        }

        private void CreateEffectList()
        {
            _effectsList = SetupReorderableList("Abnormal Status Effects", _target.effects,
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

                var template = target as AbnormalStatusTemplate;
                var path = AssetDatabase.GetAssetPath(template);
                AssetDatabase.AddObjectToAsset(effect, path);
                EditorUtility.SetDirty(template);
            }
        }
        #endregion
    }
}
#endif