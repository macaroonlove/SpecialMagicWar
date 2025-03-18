using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(menuName = "Templates/Skill/Active Skill", fileName = "ActiveSkill", order = 0)]
    public class ActiveSkillTemplate : ScriptableObject
    {
        [HideInInspector, SerializeField] private Sprite _sprite;

        [HideInInspector, SerializeField] private int _id;
        [HideInInspector, SerializeField] private string _displayName;
        [HideInInspector, SerializeField] private string _description;

        [HideInInspector, SerializeField] private RarityTemplate _rarity;
        [HideInInspector, SerializeField] private int _needMana;
        [HideInInspector, SerializeField] private float _cooldownTime;

        [HideInInspector, SerializeField] private ESpellType _spellType;
        [HideInInspector, SerializeField] private EActiveSkillType _skillType;
        [HideInInspector, SerializeField] private EUnitType _unitType;
        [HideInInspector, SerializeField] private float _skillRange;

        [HideInInspector, SerializeField] private string _parameterName;
        [HideInInspector, SerializeField] private int _parameterHash;

        [HideInInspector, SerializeField] private FX _casterFX;
        [HideInInspector, SerializeField] private FX _targetFX;

        [HideInInspector]
        public List<Effect> effects = new List<Effect>();

        #region ������Ƽ
        public Sprite sprite => _sprite;

        public int id => _id;
        public string displayName => _displayName;
        public string description => _description;

        public RarityTemplate rarity => _rarity;
        public int needMana => _needMana;
        public float cooldownTime => _cooldownTime;

        public ESpellType spellType => _spellType;
        public EActiveSkillType skillType => _skillType;
        public EUnitType unitType => _unitType;
        public float skillRange => _skillRange;

        public int parameterHash => _parameterHash;

        public FX casterFX => _casterFX;
        public FX targetFX => _targetFX;
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

    [CustomEditor(typeof(ActiveSkillTemplate)), CanEditMultipleObjects]
    public class ActiveSkillTemplateEditor : EffectEditor
    {
        private ActiveSkillTemplate _target;

        private SerializedProperty _sprite;
        private SerializedProperty _id;
        private SerializedProperty _displayName;
        private SerializedProperty _description;
        private SerializedProperty _rarity;
        private SerializedProperty _needMana;
        private SerializedProperty _cooldownTime;
        private SerializedProperty _spellType;
        private SerializedProperty _skillType;
        private SerializedProperty _unitType;
        private SerializedProperty _skillRange;
        private SerializedProperty _parameterName;
        private SerializedProperty _parameterHash;
        private SerializedProperty _casterFX;
        private SerializedProperty _targetFX;

        private ReorderableList _effectsList;
        private Effect _currentEffect;

        private void OnEnable()
        {
            _target = target as ActiveSkillTemplate;

            _sprite = serializedObject.FindProperty("_sprite");
            _id = serializedObject.FindProperty("_id");
            _displayName = serializedObject.FindProperty("_displayName");
            _description = serializedObject.FindProperty("_description");
            _rarity = serializedObject.FindProperty("_rarity");
            _needMana = serializedObject.FindProperty("_needMana");
            _cooldownTime = serializedObject.FindProperty("_cooldownTime");
            _spellType = serializedObject.FindProperty("_spellType");
            _skillType = serializedObject.FindProperty("_skillType");
            _unitType = serializedObject.FindProperty("_unitType");
            _skillRange = serializedObject.FindProperty("_skillRange");
            _parameterName = serializedObject.FindProperty("_parameterName");
            _parameterHash = serializedObject.FindProperty("_parameterHash");
            _casterFX = serializedObject.FindProperty("_casterFX");
            _targetFX = serializedObject.FindProperty("_targetFX");

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
            GUILayout.Label("���", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_rarity, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("�Ҹ� ������", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_needMana, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("��Ÿ��", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_cooldownTime, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("�Ӽ�", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_spellType, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("��ų Ÿ��", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_skillType, GUIContent.none);
            GUILayout.EndHorizontal();

            if (_skillType.enumValueIndex == (int)EActiveSkillType.Targeting)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("��ų ����", GUILayout.Width(192));
                EditorGUILayout.PropertyField(_skillRange, GUIContent.none);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("���� Ÿ��", GUILayout.Width(192));
                EditorGUILayout.PropertyField(_unitType, GUIContent.none);
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("�ִϸ��̼� �Ķ����", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_parameterName, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("�Ķ���� �ؽ� ��", GUILayout.Width(192));
            GUI.enabled = false;
            EditorGUILayout.PropertyField(_parameterHash, GUIContent.none);
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            GUILayout.Space(4);
            if (GUILayout.Button("�ؽ� �� ����"))
            {
                _parameterHash.intValue = Animator.StringToHash(_parameterName.stringValue);
            }

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("��ų ��� ��, ������ FX", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_casterFX, GUIContent.none);
            GUILayout.EndHorizontal();

            if (_skillType.enumValueIndex != (int)EActiveSkillType.Instant)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("��ų ��� ��, ���콺 ��ġ�� ������ FX", GUILayout.Width(192));
                EditorGUILayout.PropertyField(_targetFX, GUIContent.none);
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

            if (_skillType.enumValueIndex == (int)EActiveSkillType.NonTargeting)
            {
                menu.AddItem(new GUIContent("��� ������ ��ų (��Ÿ����)"), false, CreateEffectCallback, typeof(InstantDamagePointEffect));
                menu.AddItem(new GUIContent("����ü ������ ��ų (��Ÿ����)"), false, CreateEffectCallback, typeof(ProjectileDamagePointEffect));
                menu.AddItem(new GUIContent("��� ȸ�� ��ų (��Ÿ����)"), false, CreateEffectCallback, typeof(InstantHealPointEffect));
                menu.AddItem(new GUIContent("����ü ȸ�� ��ų (��Ÿ����)"), false, CreateEffectCallback, typeof(ProjectileHealPointEffect));
                menu.AddItem(new GUIContent("��� ��ȣ�� ��ų (��Ÿ����)"), false, CreateEffectCallback, typeof(InstantShieldPointEffect));
                menu.AddItem(new GUIContent("����ü ��ȣ�� ��ų (��Ÿ����)"), false, CreateEffectCallback, typeof(ProjectileShieldPointEffect));
                menu.AddItem(new GUIContent("��� ���� ��ų (��Ÿ����)"), false, CreateEffectCallback, typeof(InstantBuffPointEffect));
                menu.AddItem(new GUIContent("����ü ���� ��ų (��Ÿ����)"), false, CreateEffectCallback, typeof(ProjectileBuffPointEffect));
                menu.AddItem(new GUIContent("��� �����̻� ��ų (��Ÿ����)"), false, CreateEffectCallback, typeof(InstantAbnormalStatusPointEffect));
                menu.AddItem(new GUIContent("����ü �����̻� ��ų (��Ÿ����)"), false, CreateEffectCallback, typeof(ProjectileAbnormalStatusPointEffect));
            }
            else
            {
                menu.AddItem(new GUIContent("��� ������ ��ų (Ÿ����)"), false, CreateEffectCallback, typeof(InstantDamageByTargetUnitEffect));
                menu.AddItem(new GUIContent("����ü ������ ��ų (Ÿ����)"), false, CreateEffectCallback, typeof(ProjectileDamageByTargetUnitEffect));
                menu.AddItem(new GUIContent("��� ȸ�� ��ų (Ÿ����)"), false, CreateEffectCallback, typeof(InstantHealByTargetUnitEffect));
                menu.AddItem(new GUIContent("����ü ȸ�� ��ų (Ÿ����)"), false, CreateEffectCallback, typeof(ProjectileHealByTargetUnitEffect));
                menu.AddItem(new GUIContent("��� ��ȣ�� ��ų (Ÿ����)"), false, CreateEffectCallback, typeof(InstantShieldByTargetUnitEffect));
                menu.AddItem(new GUIContent("����ü ��ȣ�� ��ų (Ÿ����)"), false, CreateEffectCallback, typeof(ProjectileShieldByTargetUnitEffect));
                menu.AddItem(new GUIContent("��� ���� ��ų (Ÿ����)"), false, CreateEffectCallback, typeof(InstantBuffByTargetUnitEffect));
                menu.AddItem(new GUIContent("����ü ���� ��ų (Ÿ����)"), false, CreateEffectCallback, typeof(ProjectileBuffByTargetUnitEffect));
                menu.AddItem(new GUIContent("��� �����̻� ��ų (Ÿ����)"), false, CreateEffectCallback, typeof(InstantAbnormalStatusByTargetUnitEffect));
                menu.AddItem(new GUIContent("����ü �����̻� ��ų (Ÿ����)"), false, CreateEffectCallback, typeof(ProjectileAbnormalStatusByTargetUnitEffect));
                menu.AddItem(new GUIContent("��� ���ظ鿪 ��ų (Ÿ����)"), false, CreateEffectCallback, typeof(InstantImmunityByTargetUnitEffect));
            }

            menu.ShowAsContext();
        }

        private void CreateEffectList()
        {
            _effectsList = SetupReorderableList("Active Skill Effects", _target.effects,
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

                var template = target as ActiveSkillTemplate;
                var path = AssetDatabase.GetAssetPath(template);
                AssetDatabase.AddObjectToAsset(effect, path);
                EditorUtility.SetDirty(template);
            }
        }
        #endregion
    }
}
#endif