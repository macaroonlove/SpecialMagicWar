using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(menuName = "Templates/Item/Passive Item", fileName = "PassiveItem", order = 1)]
    public class PassiveItemTemplate : ScriptableObject
    {
        [HideInInspector, SerializeField] private Sprite _sprite;

        [HideInInspector, SerializeField] private int _id;
        [HideInInspector, SerializeField] private string _displayName;
        [HideInInspector, SerializeField] private string _description;
        [HideInInspector, SerializeField] private RarityTemplate _rarity;
        [HideInInspector, SerializeField] private int _price;

        [HideInInspector, SerializeField] private FX _casterFX;

        [HideInInspector]
        public List<GameTrigger> triggers = new List<GameTrigger>();

        #region 프로퍼티
        public Sprite sprite => _sprite;

        public int id => _id;
        public string displayName => _displayName;
        public string description => _description;
        public RarityTemplate rarity => _rarity;
        public int price => _price;

        public FX casterFX => _casterFX;
        #endregion

        #region 값 변경 메서드
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

    [CustomEditor(typeof(PassiveItemTemplate)), CanEditMultipleObjects]
    public class PassiveItemTemplateEditor : EffectEditor
    {
        private PassiveItemTemplate _target;

        private SerializedProperty _sprite;
        private SerializedProperty _id;
        private SerializedProperty _displayName;
        private SerializedProperty _description;
        private SerializedProperty _rarity;
        private SerializedProperty _price;
        private SerializedProperty _casterFX;

        private ReorderableList _triggersList;
        private GameTrigger _currentTrigger;

        private ReorderableList _effectsList;
        private Effect _currentEffect;

        private void OnEnable()
        {
            _target = target as PassiveItemTemplate;

            _sprite = serializedObject.FindProperty("_sprite");
            _id = serializedObject.FindProperty("_id");
            _displayName = serializedObject.FindProperty("_displayName");
            _description = serializedObject.FindProperty("_description");
            _rarity = serializedObject.FindProperty("_rarity");
            _price = serializedObject.FindProperty("_price");
            _casterFX = serializedObject.FindProperty("_casterFX");

            CreateEventTriggerList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.BeginHorizontal();

            _sprite.objectReferenceValue = EditorGUILayout.ObjectField(_sprite.objectReferenceValue, typeof(Sprite), false, GUILayout.Width(96), GUILayout.Height(96));

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("식별번호", GUILayout.Width(80));
            EditorGUILayout.PropertyField(_id, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("아이템 이름", GUILayout.Width(80));
            EditorGUILayout.PropertyField(_displayName, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("아이템 설명", GUILayout.Width(80));
            _description.stringValue = EditorGUILayout.TextArea(_description.stringValue, GUILayout.Height(50));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("등급", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_rarity, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("가격", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_price, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            DrawEventTrigger();

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(this);
            }
        }

        private void DrawEventTrigger()
        {
            _triggersList.DoLayoutList();

            if (_currentTrigger != null)
            {
                _currentTrigger.Draw();

                GUILayout.Space(10);
                if (_currentTrigger is UnitEventGameTrigger)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("유닛 아이템 발동 시, 시전자 FX", GUILayout.Width(192));
                    EditorGUILayout.PropertyField(_casterFX, GUIContent.none);
                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(10);
                _effectsList?.DoLayoutList();
            }
        }

        #region TriggerList
        private void InitMenu_EffectTriggers()
        {
            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("획득 시"), false, CreateEventTriggerCallback, typeof(GetGameTrigger));
            menu.AddItem(new GUIContent("전투 시 상시 적용"), false, CreateEventTriggerCallback, typeof(AlwaysGameTrigger));
            menu.AddItem(new GUIContent("특정 글로벌 이벤트 발생 시"), false, CreateEventTriggerCallback, typeof(GlobalEventGameTrigger));
            menu.AddItem(new GUIContent("특정 유닛 이벤트 발생 시"), false, CreateEventTriggerCallback, typeof(UnitEventGameTrigger));

            menu.ShowAsContext();
        }

        private void CreateEventTriggerList()
        {
            _triggersList = SetupReorderableList("Trigger", _target.triggers, 
                (rect, x) =>
                {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), x.GetLabel());
                },
                x =>
                {
                    _currentTrigger = x;
                    CreateEffectList();
                },
                () =>
                {
                    InitMenu_EffectTriggers();
                },
                x =>
                {
                    DestroyImmediate(_currentTrigger, true);
                    _currentTrigger = null;
                });
        }

        private void CreateEventTriggerCallback(object obj)
        {
            var trigger = ScriptableObject.CreateInstance((Type)obj) as GameTrigger;
            if (trigger != null)
            {
                trigger.hideFlags = HideFlags.HideInHierarchy;
                _target.triggers.Add(trigger);

                var template = target as PassiveItemTemplate;
                var path = AssetDatabase.GetAssetPath(template);
                AssetDatabase.AddObjectToAsset(trigger, path);
                EditorUtility.SetDirty(template);
            }
        }

        #endregion

        #region EffectList
        private void InitMenu_Effects()
        {
            var menu = new GenericMenu();

            if (_currentTrigger is AlwaysGameTrigger)
            {
                menu.AddItem(new GUIContent("모든 유닛에게 무한 지속 버프 적용"), false, CreateEffectCallback, typeof(BuffAlwaysEffect));
                menu.AddItem(new GUIContent("특정 그룹의 유닛에게 무한 지속 버프 적용"), false, CreateEffectCallback, typeof(BuffByConditionAlwaysEffect));
            }
            else
            {
                menu.AddItem(new GUIContent("Int 변수 변경"), false, CreateEffectCallback, typeof(ChangeIntVariableGlobalEffect));
                menu.AddItem(new GUIContent("Float 변수 변경"), false, CreateEffectCallback, typeof(ChangeFloatVariableGlobalEffect));
                
                if (_currentTrigger is UnitEventGameTrigger || _currentTrigger is GlobalEventGameTrigger)
                {
                    menu.AddItem(new GUIContent("특정 그룹의 유닛에게 버프 적용"), false, CreateEffectCallback, typeof(BuffByConditionGlobalEffect));
                    menu.AddItem(new GUIContent("전역 상태 적용"), false, CreateEffectCallback, typeof(GlobalStatusGlobalEffect));

                    if (_currentTrigger is UnitEventGameTrigger)
                    {
                        menu.AddItem(new GUIContent("즉시 데미지 스킬"), false, CreateEffectCallback, typeof(InstantDamageUnitEffect));
                        menu.AddItem(new GUIContent("즉시 상태이상 스킬"), false, CreateEffectCallback, typeof(InstantAbnormalStatusUnitEffect));
                    }
                }
            }
            
            menu.ShowAsContext();
        }

        private void CreateEffectList()
        {
            _effectsList = SetupReorderableList("Passive Item Effects", _currentTrigger.effects,
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
                var element = _currentTrigger.effects[index];

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
                var element = _currentTrigger.effects[index];
                if (element == null)
                {
                    return 20;
                }
                return element.GetHeight();
            };
        }

        private void CreateEffectCallback(object obj)
        {
            var effect = ScriptableObject.CreateInstance((Type)obj) as Effect;

            if (effect != null)
            {
                effect.hideFlags = HideFlags.HideInHierarchy;
                _currentTrigger.effects.Add(effect);

                var template = target as PassiveItemTemplate;
                var path = AssetDatabase.GetAssetPath(template);
                AssetDatabase.AddObjectToAsset(effect, path);
                EditorUtility.SetDirty(template);
            }
        }
        #endregion
    }
}
#endif