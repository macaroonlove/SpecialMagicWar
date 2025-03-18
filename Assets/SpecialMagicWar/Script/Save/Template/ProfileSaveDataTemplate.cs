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

        [Tooltip("�����ϰ� �ִ� �Ʊ� ���ֵ�")]
        public List<Agent> ownedAgents = new List<Agent>();

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
        public bool isClearTutorial { get => _data.isClearTutorial; set => _data.isClearTutorial = value; }

        public override void SetDefaultValues()
        {
            _data = new ProfileSaveData();

            // �ʱ� ĳ���� �߰�
            AddAgent(0);
            AddAgent(1);

            // �ʱ� ��� �߰�
            _data.gold = 100;

            isLoaded = true;
        }

        public override bool Load(string json)
        {
            _data = JsonUtility.FromJson<ProfileSaveData>(json);

            if (_data != null)
            {
                isLoaded = _data.ownedAgents.Count > 0;

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

        #region ����
        /// <summary>
        /// ������ ������ �ϴµ� �䱸�ϴ� ����
        /// </summary>
        private static readonly ObscuredInt[] _agentUpgradeRequirements = { 0, 1, 3, 5, 7, 10, 15, 30, 50, 90, 150 };

        /// <summary>
        /// ���� �߰�
        /// </summary>
        public void AddAgent(int id)
        {
            var modifyUnit = _data.ownedAgents.Find(x => x.id == id);

            // ������ �����ٸ� ���� �߰�
            if (modifyUnit == null)
            {
                _data.ownedAgents.Add(new ProfileSaveData.Agent(id));
            }
            // ������ �־��ٸ� ������ ���� �߰�
            else
            {
                modifyUnit.unitCount++;
            }
        }

        /// <summary>
        /// ���� ���׷��̵�
        /// </summary>
        public bool UpgradeAgent(int id)
        {
            var modifyUnit = _data.ownedAgents.Find(x => x.id == id);

            // ������ �ִٸ� && �ִ� ������ �ƴ϶��
            if (modifyUnit != null && modifyUnit.level < _agentUpgradeRequirements.Length - 1)
            {
                int requiredCount = _agentUpgradeRequirements[modifyUnit.level];

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
        /// ���׷��̵� ������ �������� �Ǻ�
        /// </summary>
        public bool GetUpgradeableUnit(int id)
        {
            var modifyUnit = _data.ownedAgents.Find(x => x.id == id);

            return modifyUnit != null
                && modifyUnit.level < _agentUpgradeRequirements.Length - 1
                && modifyUnit.unitCount >= _agentUpgradeRequirements[modifyUnit.level];
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