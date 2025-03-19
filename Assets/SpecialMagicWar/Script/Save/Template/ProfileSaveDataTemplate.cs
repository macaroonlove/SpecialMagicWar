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

        [Tooltip("Ʃ�丮�� Ŭ���� ����")]
        public bool isClearTutorial;

        [Tooltip("���")]
        public ObscuredInt gold;

        [Tooltip("�����ϰ� �ִ� �÷��̾� ��Ų")]
        public ObscuredInt playerSkin = 0;

        [Tooltip("�����ϰ� �ִ� �÷��̾� ��Ų��")]
        public List<int> ownedPlayerSkinIds = new List<int>();

        [Tooltip("�����ϰ� �ִ� �ż���")]
        public List<Agent> ownedHolyAnimals = new List<Agent>();

        [Tooltip("�����ϰ� �ִ� �нú� �����۵��� ���̵�")]
        public List<int> ownedPassiveItemIds = new List<int>();

        [Tooltip("�����ϰ� �ִ� ��Ƽ�� �����۵��� ���̵�")]
        public List<int> ownedActiveItemIds = new List<int>();

        #region ������ ��
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

        [Header("Variable ����")]
        [SerializeField] private ObscuredIntVariable _goldVariable;

        public bool isLoaded { get; private set; }

        public string displayName { get => _data.displayName; set => _data.displayName = value; }
        public int playerSkin { get => _data.playerSkin; set => _data.playerSkin = value; }
        public bool isClearTutorial { get => _data.isClearTutorial; set => _data.isClearTutorial = value; }

        public List<ProfileSaveData.Agent> ownedHolyAnimals => _data.ownedHolyAnimals;

        public override void SetDefaultValues()
        {
            _data = new ProfileSaveData();

            // �ʱ� �÷��̾� ��Ų �߰�
            AddPlayerSkin(0);

            // �ʱ� �ż� �߰�
            AddHolyAnimal(0);
            AddHolyAnimal(1);
            AddHolyAnimal(2);
            AddHolyAnimal(3);
            AddHolyAnimal(4);
            AddHolyAnimal(5);
            AddHolyAnimal(6);
            AddHolyAnimal(7);

            // �ʱ� ��� �߰�
            _data.gold = 100;

            // �ʱ� �÷��̾� ��Ų ����
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
        /// �÷��̾� ��Ų �߰�
        /// </summary>
        public void AddPlayerSkin(int id)
        {
            if (_data.ownedPlayerSkinIds.Contains(id) == false)
            {
                _data.ownedPlayerSkinIds.Add(id);
            }
        }

        #region �ż� ����
        /// <summary>
        /// �ż��� ������ �ϴµ� �䱸�ϴ� ����
        /// </summary>
        private static readonly ObscuredInt[] _holyAnimalUpgradeRequirements = { 0, 1, 3, 5, 7, 10, 15, 30, 50, 90, 150 };

        /// <summary>
        /// �ż� �߰�
        /// </summary>
        public void AddHolyAnimal(int id)
        {
            var modifyUnit = _data.ownedHolyAnimals.Find(x => x.id == id);

            // ������ �����ٸ� ���� �߰�
            if (modifyUnit == null)
            {
                _data.ownedHolyAnimals.Add(new ProfileSaveData.Agent(id));
            }
            // ������ �־��ٸ� ������ ���� �߰�
            else
            {
                modifyUnit.unitCount++;
            }
        }

        /// <summary>
        /// �ż� ������
        /// </summary>
        public bool UpgradeHolyAnimal(int id)
        {
            var modifyUnit = _data.ownedHolyAnimals.Find(x => x.id == id);

            // ������ �ִٸ� && �ִ� ������ �ƴ϶��
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
        /// ������ ������ �������� �Ǻ�
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
        /// �нú� ������ �߰�
        /// </summary>
        public void AddPassiveItem(int id)
        {
            if (_data.ownedPassiveItemIds.Contains(id) == false)
            {
                _data.ownedPassiveItemIds.Add(id);
            }
        }

        /// <summary>
        /// ��Ƽ�� ������ �߰�
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