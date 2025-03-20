using FrameWork.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(menuName = "Templates/Etc/InGame", fileName = "InGame", order = 1)]
    public class InGameTemplate : ScriptableObject
    {
        [SerializeField] private string _displayName;
        [SerializeField] private List<SpellProbabilityList> _spellProbabilityList = new List<SpellProbabilityList>();
        [SerializeField] private List<NeedSoulList> _landSoulList = new List<NeedSoulList>();
        [SerializeField] private List<NeedSoulList> _fireSoulList = new List<NeedSoulList>();
        [SerializeField] private List<NeedSoulList> _waterSoulList = new List<NeedSoulList>();
        [SerializeField] private List<NeedSoulList> _holyAnimalSoulList = new List<NeedSoulList>();

        [SerializeField, ReadOnly] private int _spellProbabilityLevel;
        [SerializeField, ReadOnly] private int _landSoulLevel;
        [SerializeField, ReadOnly] private int _fireSoulLevel;
        [SerializeField, ReadOnly] private int _waterSoulLevel;
        [SerializeField, ReadOnly] private int _holyAnimalSoulLevel;

        //{     ½ºÆç »Ì±â È®·ü
        //    { 0.8498f, 0.1132f, 0.03f, 0.005f, 0.002f, 0.00f },
        //    { 0.7854f, 0.1551f, 0.046f, 0.01f, 0.0035f, 0.00005f },
        //    { 0.7295f, 0.1875f, 0.0612f, 0.016f, 0.0055f, 0.0003f },
        //    { 0.6324f, 0.258f, 0.076f, 0.024f, 0.009f, 0.0006f },
        //    { 0.5773f, 0.29f, 0.086f, 0.032f, 0.012f, 0.0007f },
        //    { 0.5107f, 0.3265f, 0.105f, 0.041f, 0.016f, 0.0008f },
        //    { 0.4861f, 0.319f, 0.12f, 0.052f, 0.022f, 0.0009f },
        //    { 0.4410f, 0.337f, 0.135f, 0.058f, 0.028f, 0.001f },
        //    { 0.4202f, 0.3446f, 0.14f, 0.062f, 0.032f, 0.0012f },
        //};

        #region ÇÁ·ÎÆÛÆ¼
        public string displayName => _displayName;

        public int spellProbabilityLevel => _spellProbabilityLevel + 1;
        public int landSoulLevel => _landSoulLevel + 1;
        public int fireSoulLevel => _fireSoulLevel + 1;
        public int waterSoulLevel => _waterSoulLevel + 1;
        public int holyAnimalSoulLevel => _holyAnimalSoulLevel + 1;

        public bool isUpgradeSpellProbabilityLevel => _spellProbabilityLevel < _spellProbabilityList.Count;
        public bool isUpgradeLandSoulLevel => _landSoulLevel < _landSoulList.Count;
        public bool isUpgradeFireSoulLevel => _fireSoulLevel < _fireSoulList.Count;
        public bool isUpgradeWaterSoulLevel => _waterSoulLevel < _waterSoulList.Count;
        public bool isUpgradeHolyAnimalSoulLevel => _holyAnimalSoulLevel < _holyAnimalSoulList.Count;
        #endregion

        public void Initialize()
        {
            _spellProbabilityLevel = 0;
            _landSoulLevel = 0;
            _fireSoulLevel = 0;
            _waterSoulLevel = 0;
            _holyAnimalSoulLevel = 0;
        }

        #region Probability
        public int GetNeedCost()
        {
            return _spellProbabilityList[_spellProbabilityLevel].needCost;
        }

        public SpellProbabilityList GetSpellProbability()
        {
            return _spellProbabilityList[_spellProbabilityLevel];
        }

        public bool UpgradeSpellProbabilityLevel()
        {
            _spellProbabilityLevel++;

            return _spellProbabilityLevel + 1 == _spellProbabilityList.Count;
        }
        #endregion

        #region Land Soul
        public NeedSoulList GetNeedLandSoul()
        {
            return _landSoulList[_landSoulLevel];
        }

        public bool UpgradeLandSoulLevel()
        {
            _landSoulLevel++;

            return _landSoulLevel + 1 == _landSoulList.Count;
        }
        #endregion

        #region Fire Soul
        public NeedSoulList GetNeedFireSoul()
        {
            return _fireSoulList[_fireSoulLevel];
        }

        public bool UpgradeFireSoulLevel()
        {
            _fireSoulLevel++;

            return _fireSoulLevel + 1 == _fireSoulList.Count;
        }
        #endregion

        #region Water Soul
        public NeedSoulList GetNeedWaterSoul()
        {
            return _waterSoulList[_waterSoulLevel];
        }

        public bool UpgradeWaterSoulLevel()
        {
            _waterSoulLevel++;

            return _waterSoulLevel + 1 == _waterSoulList.Count;
        }
        #endregion

        #region Holy Animal Soul
        public int GetNeedHolyAnimalSoul()
        {
            return _holyAnimalSoulList[_holyAnimalSoulLevel].needSoul;
        }

        public bool UpgradeHolyAnimalSoulLevel()
        {
            _holyAnimalSoulLevel++;

            return _holyAnimalSoulLevel + 1 == _holyAnimalSoulList.Count;
        }
        #endregion
    }

    [System.Serializable]
    public class SpellProbabilityList
    {
        public int needCost;
        public float common;
        public float rare;
        public float epic;
        public float legend;
        public float beginning;
        public float god;
    }

    [System.Serializable]
    public class NeedSoulList
    {
        public int needSoul;
        public BuffTemplate template;
    }
}