using FrameWork.Editor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(menuName = "Templates/Unit/Agent", fileName = "Agent_", order = 0)]
    public class AgentTemplate : ScriptableObject
    {
        [HideInInspector, SerializeField] private int _id;
        [HideInInspector, SerializeField] private string _displayName;
        [HideInInspector, SerializeField] private string _description;

        [HideInInspector, SerializeField] private Sprite _sprite;
        [HideInInspector, SerializeField] private GameObject _prefab;

        [HideInInspector, SerializeField] private float _criticalHitChance;
        [HideInInspector, SerializeField] private float _criticalHitDamage;

        [HideInInspector, SerializeField] private int _physicalPenetration;
        [HideInInspector, SerializeField] private int _magicPenetration;

        [HideInInspector, SerializeField] private int _maxHP;
        [HideInInspector, SerializeField] private int _physicalResistance;
        [HideInInspector, SerializeField] private int _magicResistance;

        [HideInInspector, SerializeField] private int _hpRecoveryPerSec;

        [HideInInspector, ReadOnly] public bool isOwned;

        #region 프로퍼티
        public int id => _id;
        public string displayName => _displayName;
        public string description => _description;

        public Sprite sprite => _sprite;
        public GameObject prefab => _prefab;

        public float CriticalHitChance => _criticalHitChance;
        public float CriticalHitDamage => _criticalHitDamage;

        public int PhysicalPenetration => _physicalPenetration;
        public int MagicPenetration => _magicPenetration;

        public int MaxHP => _maxHP;
        public int PhysicalResistance => _physicalResistance;
        public int MagicResistance => _magicResistance;

        public int HPRecoveryPerSec => _hpRecoveryPerSec;
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
    using SpecialMagicWar.Core;
    using UnityEditor;

    [CustomEditor(typeof(AgentTemplate)), CanEditMultipleObjects]
    public class AgentTemplateEditor : Editor
    {
        private SerializedProperty _id;
        private SerializedProperty _displayName;
        private SerializedProperty _description;
        private SerializedProperty _sprite;
        private SerializedProperty _prefab;
        private SerializedProperty _criticalHitChance;
        private SerializedProperty _criticalHitDamage;
        private SerializedProperty _physicalPenetration;
        private SerializedProperty _magicPenetration;
        private SerializedProperty _maxHP;
        private SerializedProperty _physicalResistance;
        private SerializedProperty _magicResistance;
        private SerializedProperty _hpRecoveryPerSec;
        private SerializedProperty _isOwned;

        private void OnEnable()
        {
            _id = serializedObject.FindProperty("_id");
            _displayName = serializedObject.FindProperty("_displayName");
            _description = serializedObject.FindProperty("_description");
            _sprite = serializedObject.FindProperty("_sprite");
            _prefab = serializedObject.FindProperty("_prefab");
            _criticalHitChance = serializedObject.FindProperty("_criticalHitChance");
            _criticalHitDamage = serializedObject.FindProperty("_criticalHitDamage");
            _physicalPenetration = serializedObject.FindProperty("_physicalPenetration");
            _magicPenetration = serializedObject.FindProperty("_magicPenetration");
            _maxHP = serializedObject.FindProperty("_maxHP");
            _physicalResistance = serializedObject.FindProperty("_physicalResistance");
            _magicResistance = serializedObject.FindProperty("_magicResistance");
            _hpRecoveryPerSec = serializedObject.FindProperty("_hpRecoveryPerSec");
            _isOwned = serializedObject.FindProperty("isOwned");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.BeginHorizontal();

            _sprite.objectReferenceValue = EditorGUILayout.ObjectField(_sprite.objectReferenceValue, typeof(Sprite), false, GUILayout.Width(108), GUILayout.Height(108));

            EditorGUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("식별번호", GUILayout.Width(80));
            EditorGUILayout.PropertyField(_id, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("유닛 이름", GUILayout.Width(80));
            EditorGUILayout.PropertyField(_displayName, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("유닛 설명", GUILayout.Width(80));
            _description.stringValue = EditorGUILayout.TextArea(_description.stringValue, GUILayout.Height(50));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("프리팹", GUILayout.Width(80));
            EditorGUILayout.PropertyField(_prefab, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("치명타 확률", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_criticalHitChance, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("치명타 데미지", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_criticalHitDamage, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("물리 관통력", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_physicalPenetration, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("마법 관통력", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_magicPenetration, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("최대 체력", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_maxHP, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("방어력", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_physicalResistance, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("마법저항력", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_magicResistance, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("초당 체력 회복량", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_hpRecoveryPerSec, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.Label("소유 여부", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_isOwned, GUIContent.none);
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif