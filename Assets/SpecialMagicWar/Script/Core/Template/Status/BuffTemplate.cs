using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(menuName = "Templates/Status/Buff", fileName = "Buff", order = 0)]
    public class BuffTemplate : ScriptableObject
    {
        [HideInInspector, SerializeField] private string _displayName;
        [HideInInspector, SerializeField] private string _description;

        [HideInInspector, SerializeField] private float _delay;

        [HideInInspector, SerializeField] private bool _useAttackCountLimit;
        [HideInInspector, SerializeField] private int _attackCount;

        [HideInInspector]
        public List<Effect> effects = new List<Effect>();

        [HideInInspector, SerializeField] private FX _applyFX;
        [HideInInspector, SerializeField] private FX _removeFX;

        #region ������Ƽ
        public string displayName => _displayName;
        public string description => _description;

        public float delay => _delay;

        public bool useAttackCountLimit => _useAttackCountLimit;
        public int attackCount => _attackCount;

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

    [CustomEditor(typeof(BuffTemplate)), CanEditMultipleObjects]
    public class BuffTemplateEditor : EffectEditor
    {
        private BuffTemplate _target;

        private SerializedProperty _displayName;
        private SerializedProperty _description;
        private SerializedProperty _delay;
        private SerializedProperty _useAttackCountLimit;
        private SerializedProperty _attackCount;
        private SerializedProperty _applyFX;
        private SerializedProperty _removeFX;

        private ReorderableList _effectsList;
        private Effect _currentEffect;

        private void OnEnable()
        {
            _target = target as BuffTemplate;

            _displayName = serializedObject.FindProperty("_displayName");
            _description = serializedObject.FindProperty("_description");
            _delay = serializedObject.FindProperty("_delay");
            _useAttackCountLimit = serializedObject.FindProperty("_useAttackCountLimit");
            _attackCount = serializedObject.FindProperty("_attackCount");
            _applyFX = serializedObject.FindProperty("_applyFX");
            _removeFX = serializedObject.FindProperty("_removeFX");

            CreateEffectList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.BeginHorizontal();
            GUILayout.Label("���� �̸�", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_displayName, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("���� ����", GUILayout.Width(192));
            _description.stringValue = EditorGUILayout.TextArea(_description.stringValue, GUILayout.Height(50));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("���� �ð�", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_delay, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("���� ��, ������ ��������", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_useAttackCountLimit, GUIContent.none);
            GUILayout.EndHorizontal();

            if (_useAttackCountLimit.boolValue == true)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("���� Ƚ��", GUILayout.Width(192));
                EditorGUILayout.PropertyField(_attackCount, GUIContent.none);
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

            menu.AddItem(new GUIContent("�̵��ӵ� ����"), false, CreateEffectCallback, typeof(MoveIncreaseDataEffect));
            menu.AddItem(new GUIContent("�̵��ӵ� ��¡��϶�"), false, CreateEffectCallback, typeof(MoveMultiplierDataEffect));

            menu.AddItem(new GUIContent("���ݷ� �߰�"), false, CreateEffectCallback, typeof(ATKAdditionalDataEffect));
            menu.AddItem(new GUIContent("���ݷ� ����"), false, CreateEffectCallback, typeof(ATKIncreaseDataEffect));
            menu.AddItem(new GUIContent("���ݷ� ��¡��϶�"), false, CreateEffectCallback, typeof(ATKMultiplierDataEffect));

            menu.AddItem(new GUIContent("�ִ� ���� ���� �� �߰�"), false, CreateEffectCallback, typeof(AttackCountAdditionalDataEffect));

            menu.AddItem(new GUIContent("���ݼӵ� ����"), false, CreateEffectCallback, typeof(AttackSpeedIncreaseDataEffect));
            menu.AddItem(new GUIContent("���ݼӵ� ��¡��϶�"), false, CreateEffectCallback, typeof(AttackSpeedMultiplierDataEffect));

            menu.AddItem(new GUIContent("ȸ���� �߰�"), false, CreateEffectCallback, typeof(AvoidanceAdditionalDataEffect));

            menu.AddItem(new GUIContent("���� ����� �߰�"), false, CreateEffectCallback, typeof(PhysicalPenetrationAdditionalDataEffect));
            menu.AddItem(new GUIContent("���� ����� ����"), false, CreateEffectCallback, typeof(PhysicalPenetrationIncreaseDataEffect));
            menu.AddItem(new GUIContent("���� ����� ��¡��϶�"), false, CreateEffectCallback, typeof(PhysicalPenetrationMultiplierDataEffect));

            menu.AddItem(new GUIContent("���� ���׷� �߰�"), false, CreateEffectCallback, typeof(PhysicalResistanceAdditionalDataEffect));
            menu.AddItem(new GUIContent("���� ���׷� ����"), false, CreateEffectCallback, typeof(PhysicalResistanceIncreaseDataEffect));
            menu.AddItem(new GUIContent("���� ���׷� ��¡��϶�"), false, CreateEffectCallback, typeof(PhysicalResistanceMultiplierDataEffect));

            menu.AddItem(new GUIContent("���� ����� �߰�"), false, CreateEffectCallback, typeof(MagicPenetrationAdditionalDataEffect));
            menu.AddItem(new GUIContent("���� ����� ����"), false, CreateEffectCallback, typeof(MagicPenetrationIncreaseDataEffect));
            menu.AddItem(new GUIContent("���� ����� ��¡��϶�"), false, CreateEffectCallback, typeof(MagicPenetrationMultiplierDataEffect));

            menu.AddItem(new GUIContent("���� ���׷� �߰�"), false, CreateEffectCallback, typeof(MagicResistanceAdditionalDataEffect));
            menu.AddItem(new GUIContent("���� ���׷� ����"), false, CreateEffectCallback, typeof(MagicResistanceIncreaseDataEffect));
            menu.AddItem(new GUIContent("���� ���׷� ��¡��϶�"), false, CreateEffectCallback, typeof(MagicResistanceMultiplierDataEffect));

            menu.AddItem(new GUIContent("���ط� �߰�"), false, CreateEffectCallback, typeof(DamageAdditionalDataEffect));
            menu.AddItem(new GUIContent("���ط� ����"), false, CreateEffectCallback, typeof(DamageIncreaseDataEffect));
            menu.AddItem(new GUIContent("���ط� ��¡��϶�"), false, CreateEffectCallback, typeof(DamageMultiplierDataEffect));

            menu.AddItem(new GUIContent("�޴� ���ط� �߰�"), false, CreateEffectCallback, typeof(ReceiveDamageAdditionalDataEffect));
            menu.AddItem(new GUIContent("�޴� ���ط� ����"), false, CreateEffectCallback, typeof(ReceiveDamageIncreaseDataEffect));
            menu.AddItem(new GUIContent("�޴� ���ط� ��¡��϶�"), false, CreateEffectCallback, typeof(ReceiveDamageMultiplierDataEffect));

            menu.AddItem(new GUIContent("ġ��Ÿ Ȯ�� �߰�"), false, CreateEffectCallback, typeof(CriticalHitChanceAdditionalDataEffect));
            menu.AddItem(new GUIContent("ġ��Ÿ ������ �߰�"), false, CreateEffectCallback, typeof(CriticalHitDamageAdditionalDataEffect));
            menu.AddItem(new GUIContent("ġ��Ÿ ������ ����"), false, CreateEffectCallback, typeof(CriticalHitDamageIncreaseDataEffect));
            menu.AddItem(new GUIContent("ġ��Ÿ ������ ��¡��϶�"), false, CreateEffectCallback, typeof(CriticalHitDamageMultiplierDataEffect));

            menu.AddItem(new GUIContent("�ִ� ü�� �߰�"), false, CreateEffectCallback, typeof(MaxHPAdditionalDataEffect));
            menu.AddItem(new GUIContent("�ִ� ü�� ����"), false, CreateEffectCallback, typeof(MaxHPIncreaseDataEffect));
            menu.AddItem(new GUIContent("�ִ� ü�� ��¡��϶�"), false, CreateEffectCallback, typeof(MaxHPMultiplierDataEffect));

            menu.AddItem(new GUIContent("ȸ���� �߰�"), false, CreateEffectCallback, typeof(HealingAdditionalDataEffect));
            menu.AddItem(new GUIContent("ȸ���� ����"), false, CreateEffectCallback, typeof(HealingIncreaseDataEffect));
            menu.AddItem(new GUIContent("ȸ���� ��¡��϶�"), false, CreateEffectCallback, typeof(HealingMultiplierDataEffect));

            menu.AddItem(new GUIContent("�ʴ� ȸ���� ����"), false, CreateEffectCallback, typeof(HPRecoveryPerSecByMaxHPIncreaseDataEffect));

            menu.AddItem(new GUIContent("�ּ� ü�� ����"), false, CreateEffectCallback, typeof(SetMinHPEffect));
            menu.AddItem(new GUIContent("���� ��� ����"), false, CreateEffectCallback, typeof(SetAttackTypeEffect));
            menu.AddItem(new GUIContent("���ط� ���� ��� ����"), false, CreateEffectCallback, typeof(SetDamageTypeEffect));

            menu.AddItem(new GUIContent("���� ����� ���� �ʽ��ϴ�."), false, CreateEffectCallback, typeof(UnableToTargetOfAttackEffect));

            menu.AddItem(new GUIContent("�� Ÿ�� ���ݷ� ����"), false, CreateEffectCallback, typeof(LandATKIncreaseDataEffect));
            menu.AddItem(new GUIContent("�� Ÿ�� ���ݷ� ����"), false, CreateEffectCallback, typeof(FireATKIncreaseDataEffect));
            menu.AddItem(new GUIContent("�� Ÿ�� ���ݷ� ����"), false, CreateEffectCallback, typeof(WaterATKIncreaseDataEffect));

            menu.ShowAsContext();
        }

        private void CreateEffectList()
        {
            _effectsList = SetupReorderableList("Buff Effects", _target.effects,
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

                var template = target as BuffTemplate;
                var path = AssetDatabase.GetAssetPath(template);
                AssetDatabase.AddObjectToAsset(effect, path);
                EditorUtility.SetDirty(template);
            }
        }
        #endregion
    }
}
#endif