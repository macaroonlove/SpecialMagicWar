using FrameWork.UIBinding;
using SpecialMagicWar.Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class UIBotPlayingCanvas : UIBase
    {
        [SerializeField] private int _matchBotIndex;
        [SerializeField] private InGameTemplate _template;

        private CostSystem _costSystem;
        private SoulSystem _soulSystem;
        private AgentSystem _agentsystem;
        private EnemySpawnSystem _enemySpawnSystem;
        private HolyAnimalCreateSystem _holyAnimalCreateSystem;
        private BuffAbility _buffAbility;

        private UISpellCanvas _uiSpellCanvas;
        private UIBountyCanvas _uiBountyCanvas;
        private UIHolyAnimalCanvas _uiHolyAnimalCanvas;
        private UIEnforceCanvas _uiEnforceCanvas;
        private UIMiningCanvas _uiMiningCanvas;

        private int _needSpellCost;

        private Vector3 _spawnPos;
        private List<Vector3> _wayPoint = new List<Vector3>();
        private bool _isBountyAble;

        private List<int> _createHolyAnimalIndexs = new List<int>();

        private int _needEnforceCost;
        private int _needLandSoul;
        private int _needFireSoul;
        private int _needWaterSoul;
        private int _needHolyAnimalSoul;

        private WaitForSeconds _wfs = new WaitForSeconds(60f);

        protected override void Initialize()
        {
            _uiSpellCanvas = GetComponentInChildren<UISpellCanvas>();
            _uiBountyCanvas = GetComponentInChildren<UIBountyCanvas>();
            _uiHolyAnimalCanvas = GetComponentInChildren<UIHolyAnimalCanvas>();
            _uiEnforceCanvas = GetComponentInChildren<UIEnforceCanvas>();
            _uiMiningCanvas = GetComponentInChildren<UIMiningCanvas>();

            BattleManager.Instance.playerCreateSystem.onCreateBot += OnCreateBot;
        }

        void OnDestroy()
        {
            BattleManager.Instance.playerCreateSystem.onCreateBot -= OnCreateBot;

            if (_matchBotIndex == 1) _costSystem.onChangedBot1Cost -= OnChangedCost;
            else if (_matchBotIndex == 2) _costSystem.onChangedBot2Cost -= OnChangedCost;
            else if (_matchBotIndex == 3) _costSystem.onChangedBot3Cost -= OnChangedCost;
        }

        private void OnCreateBot(AgentUnit unit, int index)
        {
            if (_matchBotIndex != index) return;

            unit.healthAbility.onDeath += () =>
            {
                Destroy(gameObject);
            };

            _template.Initialize();
            _uiSpellCanvas?.Initialize(unit, _template);
            _uiBountyCanvas?.Initialize(null, null);
            _uiHolyAnimalCanvas?.Initialize(_uiSpellCanvas, null);
            _uiEnforceCanvas?.Initialize(unit, null, _uiSpellCanvas, _template);
            _uiMiningCanvas?.Initialize(_uiSpellCanvas, null);

            _costSystem = BattleManager.Instance.GetSubSystem<CostSystem>();
            _soulSystem = BattleManager.Instance.GetSubSystem<SoulSystem>();
            _agentsystem = BattleManager.Instance.GetSubSystem<AgentSystem>();
            _enemySpawnSystem = BattleManager.Instance.GetSubSystem<EnemySpawnSystem>();
            _holyAnimalCreateSystem = BattleManager.Instance.GetSubSystem<HolyAnimalCreateSystem>();
            _buffAbility = unit.GetAbility<BuffAbility>();

            _needSpellCost = 20;
            _isBountyAble = true;
            _needEnforceCost = _template.GetNeedCost();
            _needLandSoul = _template.GetNeedLandSoul().needSoul;
            _needFireSoul = _template.GetNeedFireSoul().needSoul;
            _needWaterSoul = _template.GetNeedWaterSoul().needSoul;
            _needHolyAnimalSoul = _template.GetNeedHolyAnimalSoul().needSoul;

            _wayPoint.Clear();
            if (_matchBotIndex == 1)
            {
                _spawnPos = new Vector3(-1.02f, -2, 0);
                _wayPoint.Add(new Vector3(-1.02f, 4, 0));
            }
            else if (_matchBotIndex == 2)
            {
                _spawnPos = new Vector3(0.36f, -2, 0);
                _wayPoint.Add(new Vector3(0.36f, 4, 0));
            }
            else if (_matchBotIndex == 3)
            {
                _spawnPos = new Vector3(1.74f, -2, 0);
                _wayPoint.Add(new Vector3(1.74f, 4, 0));
            }

            if (_matchBotIndex == 1) _costSystem.onChangedBot1Cost += OnChangedCost;
            else if (_matchBotIndex == 2) _costSystem.onChangedBot2Cost += OnChangedCost;
            else if (_matchBotIndex == 3) _costSystem.onChangedBot3Cost += OnChangedCost;

            if (_matchBotIndex == 1) _soulSystem.onChangedBot1Soul += OnChangedSoul;
            else if (_matchBotIndex == 2) _soulSystem.onChangedBot2Soul += OnChangedSoul;
            else if (_matchBotIndex == 3) _soulSystem.onChangedBot3Soul += OnChangedSoul;

            // 시작할 때 스펠 1개는 무조건 생성하기
            GenerateRandomSpell();
        }

        private void OnChangedCost(int cost)
        {
            // 70% 확률로 스펠 소환
            if (_needSpellCost <= cost && Random.value < 0.7f)
            {
                GenerateRandomSpell();
            }

            // 10% 확률로 현상수배 몬스터 소환
            if (_isBountyAble && Random.value < 0.1f)
            {
                GenerateBountyUnit();
            }

            // 80% 확률로 소환 확률 강화
            if (_needEnforceCost <= cost && Random.value < 0.8f)
            {
                GenerateProbability();
            }
        }

        private void OnChangedSoul(int soul)
        {
            // 30% 확률로 신수 강화
            if (_needHolyAnimalSoul <= soul && Random.value < 0.3f)
            {
                HolyAnimalEnforce();
            }

            // 30% 확률로 땅 타입 강화
            if (_needLandSoul <= soul && Random.value < 0.3f)
            {
                LandEnforce();
            }

            // 30% 확률로 불 타입 강화
            if (_needFireSoul <= soul && Random.value < 0.3f)
            {
                FireEnforce();
            }

            // 30% 확률로 물 타입 강화
            if (_needWaterSoul <= soul && Random.value < 0.3f)
            {
                WaterEnforce();
            }
        }

        #region 스펠
        private void GenerateRandomSpell()
        {
            var spell = _uiSpellCanvas.GenerateRandomSpell();
            _costSystem.PayBotCost(_needSpellCost, _matchBotIndex);
            _needSpellCost++;

            if (spell.spellCount == 3)
            {
                spell.CompositeSkill();
            }

            TryCreateHolyAnimal();
        }
        #endregion

        #region 현상금
        private void GenerateBountyUnit()
        {
            var bountys = GameDataManager.Instance.bountyLibrary.templates;
            var bounty = bountys[Random.Range(0, bountys.Count)];

            var unit = _enemySpawnSystem.SpawnBountyUnit(bounty, _spawnPos, _matchBotIndex);

            if (unit == null) return;

            unit.GetAbility<MoveWayPointAbility>().InitializeWayPoint(_wayPoint);

            _isBountyAble = false;

            StartCoroutine(CoBountyCooldown());
        }

        private IEnumerator CoBountyCooldown()
        {
            yield return _wfs;

            _isBountyAble = true;
        }
        #endregion

        #region 신수
        private void TryCreateHolyAnimal()
        {
            // 플레이어가 보유하고 있는 신수를 공유받는다.
            var holyAnimals = SaveManager.Instance.profileData.ownedHolyAnimals;

            foreach (var animal in holyAnimals)
            {
                // 이미 소환했다면
                if (_createHolyAnimalIndexs.Contains(animal.id)) continue;

                var template = GameDataManager.Instance.GetHolyAnimalTemplateById(animal.id);

                if (Condition(template, _uiSpellCanvas.spells))
                {
                    _holyAnimalCreateSystem.CreateUnit(template, _matchBotIndex);
                    _createHolyAnimalIndexs.Add(animal.id);
                }
            }
        }

        private bool Condition(HolyAnimalTemplate template, List<UISpellButton> spells)
        {
            int selectedCount = 0;

            var conditions = template.conditions;
            List<(UISpellButton, int)> selectedSpells = new List<(UISpellButton, int)>();

            foreach (var condition in conditions)
            {
                foreach (var spell in spells)
                {
                    if (spell.template == condition.spellTemplate && spell.spellCount >= condition.count)
                    {
                        selectedSpells.Add((spell, condition.count));
                        selectedCount += condition.count;
                        break;
                    }
                }
            }

            return selectedSpells.Count == conditions.Count;
        }
        #endregion

        #region 강화
        private void GenerateProbability()
        {
            if (_template.isUpgradeSpellProbabilityLevel)
            {
                _template.UpgradeSpellProbabilityLevel();

                _costSystem.PayBotCost(_needEnforceCost, _matchBotIndex);
                _needEnforceCost = _template.GetNeedCost();
            }
        }

        private void LandEnforce()
        {
            if (_template.isUpgradeLandSoulLevel)
            {
                _template.UpgradeLandSoulLevel();

                var soul = _template.GetNeedLandSoul();

                _soulSystem.PayBotSoul(_needLandSoul, _matchBotIndex);
                _needLandSoul = soul.needSoul;
                if (soul.template != null) _buffAbility.ApplyBuff(soul.template, int.MaxValue);
            }
        }

        private void FireEnforce()
        {
            if (_template.isUpgradeFireSoulLevel)
            {
                _template.UpgradeFireSoulLevel();

                var soul = _template.GetNeedFireSoul();

                _soulSystem.PayBotSoul(_needFireSoul, _matchBotIndex);
                _needFireSoul = soul.needSoul;
                if (soul.template != null) _buffAbility.ApplyBuff(soul.template, int.MaxValue);
            }
        }

        private void WaterEnforce()
        {
            if (_template.isUpgradeWaterSoulLevel)
            {
                _template.UpgradeWaterSoulLevel();

                var soul = _template.GetNeedWaterSoul();

                _soulSystem.PayBotSoul(_needWaterSoul, _matchBotIndex);
                _needWaterSoul = soul.needSoul;
                if (soul.template != null) _buffAbility.ApplyBuff(soul.template, int.MaxValue);
            }
        }

        private void HolyAnimalEnforce()
        {
            if (_template.isUpgradeHolyAnimalSoulLevel)
            {
                _template.UpgradeHolyAnimalSoulLevel();

                var soul = _template.GetNeedHolyAnimalSoul();

                _soulSystem.PayBotSoul(_needHolyAnimalSoul, _matchBotIndex);
                _needHolyAnimalSoul = soul.needSoul;
                if (soul.template != null)
                {
                    var holyAnimals = _agentsystem.GetAllHolyAnimals(_matchBotIndex);
                    foreach (var holyAnimal in holyAnimals)
                    {
                        holyAnimal.GetAbility<BuffAbility>().ApplyBuff(soul.template, int.MaxValue);
                    }
                }
            }
        }
        #endregion
    }
}