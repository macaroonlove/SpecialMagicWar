using FrameWork.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(menuName = "Templates/Item/Active Item", fileName = "ActiveItem", order = 0)]
    public class ActiveItemTemplate : ScriptableObject
    {
        [HideInInspector, SerializeField] private Sprite _sprite;

        [HideInInspector, SerializeField] private int _id;
        [HideInInspector, SerializeField] private string _displayName;
        [HideInInspector, SerializeField] private string _description;

        [HideInInspector, SerializeField] private int _needCost;
        [HideInInspector, SerializeField] private float _cooldownTime;
        [HideInInspector, SerializeField] private float _delay;
        
        [HideInInspector, SerializeField] private ERangeType _rangeType;
        [HideInInspector, SerializeField] private float _range;
        [HideInInspector, SerializeField] private EUnitType _unitType;
        // ���� �߰�

        [HideInInspector, SerializeField] private FX _itemFX;
        [HideInInspector, SerializeField] private FX _afterDelayFX;

        [HideInInspector]
        public List<Effect> effects = new List<Effect>();

        #region ������Ƽ
        public Sprite sprite => _sprite;

        public int id => _id;
        public string displayName => _displayName;
        public string description => _description;

        public int needCost => _needCost;
        public float cooldownTime => _cooldownTime;
        public float delay => _delay;
        
        public ERangeType rangeType => _rangeType;
        public float range => _range;
        public EUnitType unitType => _unitType;

        public FX itemFX => _itemFX;
        public FX afterDelayFX => _afterDelayFX;
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

    [CustomEditor(typeof(ActiveItemTemplate)), CanEditMultipleObjects]
    public class ActiveItemTemplateEditor : EffectEditor
    {
        private ActiveItemTemplate _target;

        private SerializedProperty _sprite;
        private SerializedProperty _id;
        private SerializedProperty _displayName;
        private SerializedProperty _description;
        private SerializedProperty _needCost;
        private SerializedProperty _cooldownTime;
        private SerializedProperty _delay;
        private SerializedProperty _rangeType;
        private SerializedProperty _range;
        private SerializedProperty _unitType;
        private SerializedProperty _itemFX;
        private SerializedProperty _afterDelayFX;

        private ReorderableList _effectsList;
        private Effect _currentEffect;

        private void OnEnable()
        {
            _target = target as ActiveItemTemplate;

            _sprite = serializedObject.FindProperty("_sprite");
            _id = serializedObject.FindProperty("_id");
            _displayName = serializedObject.FindProperty("_displayName");
            _description = serializedObject.FindProperty("_description");
            _needCost = serializedObject.FindProperty("_needCost");
            _cooldownTime = serializedObject.FindProperty("_cooldownTime");
            _delay = serializedObject.FindProperty("_delay");
            _rangeType = serializedObject.FindProperty("_rangeType");
            _range = serializedObject.FindProperty("_range");
            _unitType = serializedObject.FindProperty("_unitType");
            _itemFX = serializedObject.FindProperty("_itemFX");
            _afterDelayFX = serializedObject.FindProperty("_afterDelayFX");

            CreateEffectList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.BeginHorizontal();
            
            _sprite.objectReferenceValue = EditorGUILayout.ObjectField(_sprite.objectReferenceValue, typeof(Sprite), false, GUILayout.Width(96), GUILayout.Height(96));
            
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("�ĺ���ȣ", GUILayout.Width(80));
            EditorGUILayout.PropertyField(_id, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("��ų �̸�", GUILayout.Width(80));
            EditorGUILayout.PropertyField(_displayName, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("��ų ����", GUILayout.Width(80));
            _description.stringValue = EditorGUILayout.TextArea(_description.stringValue, GUILayout.Height(50));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("�ʿ� �ڽ�Ʈ", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_needCost, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("��Ÿ��", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_cooldownTime, GUIContent.none);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("���� �ð�", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_delay, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("���� Ÿ��", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_unitType, GUIContent.none);
            GUILayout.EndHorizontal();

            if (_unitType.enumValueIndex != (int)EUnitType.None)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("���� ���", GUILayout.Width(192));
                EditorGUILayout.PropertyField(_rangeType, GUIContent.none);
                GUILayout.EndHorizontal();

                if (_rangeType.enumValueIndex == (int)ERangeType.Circle)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("����", GUILayout.Width(192));
                    EditorGUILayout.PropertyField(_range, GUIContent.none);
                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("������ �ߵ� ��, FX", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_itemFX, GUIContent.none);
            GUILayout.EndHorizontal();

            if (_delay.floatValue > 0)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("������ �ߵ� ���� �ð� ����, FX", GUILayout.Width(192));
                EditorGUILayout.PropertyField(_afterDelayFX, GUIContent.none);
                GUILayout.EndHorizontal();
            }

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

            if (_target.unitType == EUnitType.None)
            {
                menu.AddItem(new GUIContent("Int ���� ����"), false, CreateEffectCallback, typeof(ChangeIntVariableGlobalEffect));
                menu.AddItem(new GUIContent("Float ���� ����"), false, CreateEffectCallback, typeof(ChangeFloatVariableGlobalEffect));
                menu.AddItem(new GUIContent("Ư�� �׷��� ���ֿ��� ���� ����"), false, CreateEffectCallback, typeof(BuffByConditionGlobalEffect));
                menu.AddItem(new GUIContent("���� ���� ����"), false, CreateEffectCallback, typeof(GlobalStatusGlobalEffect));
            }
            else
            {
                menu.AddItem(new GUIContent("��Ƽ�� ������ ��� ���ֵ鿡�� ������ ����"), false, CreateEffectCallback, typeof(DamageActiveItemEffect));
                menu.AddItem(new GUIContent("��Ƽ�� ������ ��� ���ֵ鿡�� ȸ�� ����"), false, CreateEffectCallback, typeof(HealActiveItemEffect));
                menu.AddItem(new GUIContent("��Ƽ�� ������ ��� ���ֵ鿡�� ��ȣ�� ����"), false, CreateEffectCallback, typeof(ShieldActiveItemEffect));
                menu.AddItem(new GUIContent("��Ƽ�� ������ ��� ���ֵ鿡�� ���� ����"), false, CreateEffectCallback, typeof(BuffActiveItemEffect));
                menu.AddItem(new GUIContent("��Ƽ�� ������ ��� ���ֵ鿡�� �����̻� ����"), false, CreateEffectCallback, typeof(AbnormalStatusActiveItemEffect));
            }

            menu.ShowAsContext();
        }

        private void CreateEffectList()
        {
            _effectsList = SetupReorderableList("Active Item Effects", _target.effects,
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

                var template = target as ActiveItemTemplate;
                var path = AssetDatabase.GetAssetPath(template);
                AssetDatabase.AddObjectToAsset(effect, path);
                EditorUtility.SetDirty(template);
            }
        }
        #endregion
    }
}
#endif