using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(menuName = "Templates/Unit/Enemy", fileName = "Enemy_", order = 1)]
    public class EnemyTemplate : ScriptableObject
    {
        [HideInInspector, SerializeField] private int _id;
        [HideInInspector, SerializeField] private string _displayName;
        [HideInInspector, SerializeField] private string _description;

        [HideInInspector, SerializeField] private Sprite _sprite;
        [HideInInspector, SerializeField] private GameObject _prefab;

        [HideInInspector, SerializeField] private int _gainCost;
        [HideInInspector, SerializeField] private int _gainSoul;

        [HideInInspector, SerializeField] private float _moveSpeed;
        [HideInInspector, SerializeField] private EMoveType _moveType;

        [HideInInspector, SerializeField] private int _atk;
        [HideInInspector, SerializeField] private float _attackTerm;
        [HideInInspector, SerializeField] private float _attackRange;

        [HideInInspector, SerializeField] private bool _isProjectileAttack;
        [HideInInspector, SerializeField] private GameObject _projectilePrefab;
        [HideInInspector, SerializeField] private ESpawnPoint _spawnPoint;

        [HideInInspector, SerializeField] private EAttackType _attackType;
        [HideInInspector, SerializeField] private EDamageType _damageType;

        [HideInInspector, SerializeField] private float _criticalHitChance;
        [HideInInspector, SerializeField] private float _criticalHitDamage;

        [HideInInspector, SerializeField] private int _physicalPenetration;
        [HideInInspector, SerializeField] private int _magicPenetration;

        [HideInInspector, SerializeField] private int _maxHP;
        [HideInInspector, SerializeField] private int _physicalResistance;
        [HideInInspector, SerializeField] private int _magicResistance;

        [HideInInspector, SerializeField] private int _hpRecoveryPerSec;

        [HideInInspector, SerializeField] private FX _casterFX;
        [HideInInspector, SerializeField] private FX _targetFX;

        #region 프로퍼티
        public int id => _id;
        public string displayName => _displayName;
        public string description => _description;

        public Sprite sprite => _sprite;
        public GameObject prefab => _prefab;
        
        public int gainCost => _gainCost;
        public int gainSoul => _gainSoul;

        public float MoveSpeed => _moveSpeed;
        public EMoveType MoveType => _moveType;

        public int ATK => _atk;
        public float AttackTerm => _attackTerm;
        public float AttackRange => _attackRange;

        public bool isProjectileAttack => _isProjectileAttack;
        public GameObject projectilePrefab => _projectilePrefab;
        public ESpawnPoint spawnPoint => _spawnPoint;

        public EAttackType AttackType => _attackType;
        public EDamageType DamageType => _damageType;

        public float CriticalHitChance => _criticalHitChance;
        public float CriticalHitDamage => _criticalHitDamage;

        public int PhysicalPenetration => _physicalPenetration;
        public int MagicPenetration => _magicPenetration;

        public int MaxHP => _maxHP;
        public int PhysicalResistance => _physicalResistance;
        public int MagicResistance => _magicResistance;

        public int HPRecoveryPerSec => _hpRecoveryPerSec;

        public FX casterFX => _casterFX;
        public FX targetFX => _targetFX;
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

    [CustomEditor(typeof(EnemyTemplate)), CanEditMultipleObjects]
    public class EnemyTemplateEditor : Editor
    {
        private SerializedProperty _id;
        private SerializedProperty _displayName;
        private SerializedProperty _description;
        private SerializedProperty _sprite;
        private SerializedProperty _prefab;
        private SerializedProperty _gainCost;
        private SerializedProperty _gainSoul;
        private SerializedProperty _moveSpeed;
        private SerializedProperty _moveType;
        private SerializedProperty _atk;
        private SerializedProperty _attackTerm;
        private SerializedProperty _attackRange;
        private SerializedProperty _isProjectileAttack;
        private SerializedProperty _projectilePrefab;
        private SerializedProperty _spawnPoint;
        private SerializedProperty _attackType;
        private SerializedProperty _damageType;
        private SerializedProperty _criticalHitChance;
        private SerializedProperty _criticalHitDamage;
        private SerializedProperty _physicalPenetration;
        private SerializedProperty _magicPenetration;
        private SerializedProperty _maxHP;
        private SerializedProperty _physicalResistance;
        private SerializedProperty _magicResistance;
        private SerializedProperty _hpRecoveryPerSec;
        private SerializedProperty _casterFX;
        private SerializedProperty _targetFX;

        private void OnEnable()
        {
            _id = serializedObject.FindProperty("_id");
            _displayName = serializedObject.FindProperty("_displayName");
            _description = serializedObject.FindProperty("_description");
            _sprite = serializedObject.FindProperty("_sprite");
            _prefab = serializedObject.FindProperty("_prefab");
            _gainCost = serializedObject.FindProperty("_gainCost");
            _gainSoul = serializedObject.FindProperty("_gainSoul");
            _moveSpeed = serializedObject.FindProperty("_moveSpeed");
            _moveType = serializedObject.FindProperty("_moveType");
            _atk = serializedObject.FindProperty("_atk");
            _attackTerm = serializedObject.FindProperty("_attackTerm");
            _attackRange = serializedObject.FindProperty("_attackRange");
            _isProjectileAttack = serializedObject.FindProperty("_isProjectileAttack");
            _projectilePrefab = serializedObject.FindProperty("_projectilePrefab");
            _spawnPoint = serializedObject.FindProperty("_spawnPoint");
            _attackType = serializedObject.FindProperty("_attackType");
            _damageType = serializedObject.FindProperty("_damageType");
            _criticalHitChance = serializedObject.FindProperty("_criticalHitChance");
            _criticalHitDamage = serializedObject.FindProperty("_criticalHitDamage");
            _physicalPenetration = serializedObject.FindProperty("_physicalPenetration");
            _magicPenetration = serializedObject.FindProperty("_magicPenetration");
            _maxHP = serializedObject.FindProperty("_maxHP");
            _physicalResistance = serializedObject.FindProperty("_physicalResistance");
            _magicResistance = serializedObject.FindProperty("_magicResistance");
            _hpRecoveryPerSec = serializedObject.FindProperty("_hpRecoveryPerSec");
            _casterFX = serializedObject.FindProperty("_casterFX");
            _targetFX = serializedObject.FindProperty("_targetFX");
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
            GUILayout.Label("처치 시 획득 코스트", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_gainCost, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("처치 시 획득 영혼", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_gainSoul, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("이동 속도", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_moveSpeed, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("이동 방식", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_moveType, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("공격력", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_atk, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("공격 간격", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_attackTerm, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("공격 사거리", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_attackRange, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("투사체 사용 여부", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_isProjectileAttack, GUIContent.none);
            GUILayout.EndHorizontal();

            if (_isProjectileAttack.boolValue)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("투사체 프리팹", GUILayout.Width(192));
                EditorGUILayout.PropertyField(_projectilePrefab, GUIContent.none);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("스폰 위치", GUILayout.Width(192));
                EditorGUILayout.PropertyField(_spawnPoint, GUIContent.none);
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("공격 방식", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_attackType, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("데미지 타입", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_damageType, GUIContent.none);
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

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("공격 시, 시전자 FX", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_casterFX, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("공격 시, 대상자 FX", GUILayout.Width(192));
            EditorGUILayout.PropertyField(_targetFX, GUIContent.none);
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif