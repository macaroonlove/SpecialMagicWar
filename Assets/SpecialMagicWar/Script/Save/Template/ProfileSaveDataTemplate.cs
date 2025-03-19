using CodeStage.AntiCheat.ObscuredTypes;
using FrameWork.Editor;
using ScriptableObjectArchitecture;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Save
{
    [Serializable]
    public class ProfileSaveData
    {
        public string displayName;

        [Tooltip("튜토리얼 클리어 여부")]
        public bool isClearTutorial;

        [Tooltip("골드")]
        public ObscuredInt gold;

        [Tooltip("적용하고 있는 플레이어 스킨")]
        public ObscuredInt playerSkin = 0;

        [Tooltip("보유하고 있는 플레이어 스킨들")]
        public List<int> ownedPlayerSkinIds = new List<int>();

        [Tooltip("보유하고 있는 신수들")]
        public List<Agent> ownedHolyAnimals = new List<Agent>();

        [Tooltip("보유하고 있는 패시브 아이템들의 아이디")]
        public List<int> ownedPassiveItemIds = new List<int>();

        [Tooltip("보유하고 있는 액티브 아이템들의 아이디")]
        public List<int> ownedActiveItemIds = new List<int>();

        #region 데이터 모델
        [Serializable]
        public class Agent
        {
            public int id;
            public int unitCount;
            public int level;

            public Agent(int id)
            {
                this.id = id;
                this.unitCount = 1;
                this.level = 1;
            }
        }
        #endregion
    }

    [CreateAssetMenu(menuName = "Templates/SaveData/ProfileSaveData", fileName = "ProfileSaveData", order = 0)]
    public class ProfileSaveDataTemplate : SaveDataTemplate
    {
        [SerializeField, ReadOnly] private ProfileSaveData _data;

        [Header("Variable 연동")]
        [SerializeField] private ObscuredIntVariable _goldVariable;

        public bool isLoaded { get; private set; }

        public string displayName { get => _data.displayName; set => _data.displayName = value; }
        public int playerSkin { get => _data.playerSkin; set => _data.playerSkin = value; }
        public bool isClearTutorial { get => _data.isClearTutorial; set => _data.isClearTutorial = value; }

        public List<ProfileSaveData.Agent> ownedHolyAnimals => _data.ownedHolyAnimals;

        public override void SetDefaultValues()
        {
            _data = new ProfileSaveData();

            // 초기 플레이어 스킨 추가
            AddPlayerSkin(0);

            // 초기 신수 추가
            AddHolyAnimal(0);
            AddHolyAnimal(1);
            AddHolyAnimal(2);
            AddHolyAnimal(3);
            AddHolyAnimal(4);
            AddHolyAnimal(5);
            AddHolyAnimal(6);
            AddHolyAnimal(7);

            // 초기 골드 추가
            _data.gold = 100;

            // 초기 플레이어 스킨 적용
            _data.playerSkin = 0;

            isLoaded = true;
        }

        public override bool Load(string json)
        {
            _data = JsonUtility.FromJson<ProfileSaveData>(json);

            if (_data != null)
            {
                isLoaded = _data.ownedHolyAnimals.Count > 0;
                
                _goldVariable.Value = _data.gold;
            }

            return isLoaded;
        }

        public override string ToJson()
        {
            if (_data == null) return null;

            _data.gold = _goldVariable.Value;

            return JsonUtility.ToJson(_data);
        }

        public override void Clear()
        {
            _data = null;
            isLoaded = false;
        }

        /// <summary>
        /// 플레이어 스킨 추가
        /// </summary>
        public void AddPlayerSkin(int id)
        {
            if (_data.ownedPlayerSkinIds.Contains(id) == false)
            {
                _data.ownedPlayerSkinIds.Add(id);
            }
        }

        #region 신수 유닛
        /// <summary>
        /// 신수를 레벨업 하는데 요구하는 개수
        /// </summary>
        private static readonly ObscuredInt[] _holyAnimalUpgradeRequirements = { 0, 1, 3, 5, 7, 10, 15, 30, 50, 90, 150 };

        /// <summary>
        /// 신수 추가
        /// </summary>
        public void AddHolyAnimal(int id)
        {
            var modifyUnit = _data.ownedHolyAnimals.Find(x => x.id == id);

            // 유닛이 없었다면 유닛 추가
            if (modifyUnit == null)
            {
                _data.ownedHolyAnimals.Add(new ProfileSaveData.Agent(id));
            }
            // 유닛이 있었다면 유닛의 개수 추가
            else
            {
                modifyUnit.unitCount++;
            }
        }

        /// <summary>
        /// 신수 레벨업
        /// </summary>
        public bool UpgradeHolyAnimal(int id)
        {
            var modifyUnit = _data.ownedHolyAnimals.Find(x => x.id == id);

            // 유닛이 있다면 && 최대 레벨이 아니라면
            if (modifyUnit != null && modifyUnit.level < _holyAnimalUpgradeRequirements.Length - 1)
            {
                int requiredCount = _holyAnimalUpgradeRequirements[modifyUnit.level];

                if (modifyUnit.unitCount >= requiredCount)
                {
                    modifyUnit.unitCount -= requiredCount;
                    modifyUnit.level++;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 레벨업 가능한 유닛인지 판별
        /// </summary>
        public bool GetUpgradeableHolyAnimal(int id)
        {
            var modifyUnit = _data.ownedHolyAnimals.Find(x => x.id == id);

            return modifyUnit != null
                && modifyUnit.level < _holyAnimalUpgradeRequirements.Length - 1
                && modifyUnit.unitCount >= _holyAnimalUpgradeRequirements[modifyUnit.level];
        }
        #endregion

        /// <summary>
        /// 패시브 아이템 추가
        /// </summary>
        public void AddPassiveItem(int id)
        {
            if (_data.ownedPassiveItemIds.Contains(id) == false)
            {
                _data.ownedPassiveItemIds.Add(id);
            }
        }

        /// <summary>
        /// 액티브 아이템 추가
        /// </summary>
        public void AddActiveItem(int id)
        {
            if (_data.ownedActiveItemIds.Contains(id) == false)
            {
                _data.ownedActiveItemIds.Add(id);
            }
        }
    }
}