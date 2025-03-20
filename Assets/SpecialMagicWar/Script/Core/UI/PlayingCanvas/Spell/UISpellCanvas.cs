using FrameWork.UIBinding;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    public class UISpellCanvas : UIBase
    {
        #region ¹ÙÀÎµù
        enum Texts
        {
            LandEnforceCount,
            FireEnforceCount,
            WaterEnforceCount,
        }
        #endregion

        private List<UISpellButton> _spells;

        private List<UISpellButton> _commonRarity;
        private List<UISpellButton> _rareRarity;
        private List<UISpellButton> _epicRarity;
        private List<UISpellButton> _legendRarity;
        private List<UISpellButton> _beginningRarity;
        private List<UISpellButton> _godRarity;

        private InGameTemplate _inGameTemplate;

        private TextMeshProUGUI _landEnforceCount;
        private TextMeshProUGUI _fireEnforceCount;
        private TextMeshProUGUI _waterEnforceCount;

        internal List<UISpellButton> spells => _spells;

        internal event UnityAction<List<UISpellButton>> onChangeSpell;

        protected override void Initialize()
        {
            _spells = GetComponentsInChildren<UISpellButton>().ToList();

            _commonRarity = _spells.Where(spell => spell.template.rarity.rarity == ERarity.Common).ToList();
            _rareRarity = _spells.Where(spell => spell.template.rarity.rarity == ERarity.Rare).ToList();
            _epicRarity = _spells.Where(spell => spell.template.rarity.rarity == ERarity.Epic).ToList();
            _legendRarity = _spells.Where(spell => spell.template.rarity.rarity == ERarity.Legend).ToList();
            _beginningRarity = _spells.Where(spell => spell.template.rarity.rarity == ERarity.Beginning).ToList();
            _godRarity = _spells.Where(spell => spell.template.rarity.rarity == ERarity.God).ToList();

            BindText(typeof(Texts));

            _landEnforceCount = GetText((int)Texts.LandEnforceCount);
            _fireEnforceCount = GetText((int)Texts.FireEnforceCount);
            _waterEnforceCount = GetText((int)Texts.WaterEnforceCount);
        }

        internal void UpdateLandEnforceCount(string value)
        {
            _landEnforceCount.text = value;
        }

        internal void UpdateFireEnforceCount(string value)
        {
            _fireEnforceCount.text = value;
        }

        internal void UpdateWaterEnforceCount(string value)
        {
            _waterEnforceCount.text = value;
        }

        internal void Initialize(AgentUnit unit, InGameTemplate inGameTemplate)
        {
            _inGameTemplate = inGameTemplate;

            foreach (var spell in _commonRarity)
            {
                spell.SetUnit(unit, this);
            }
            foreach (var spell in _rareRarity)
            {
                spell.SetUnit(unit, this);
            }
            foreach (var spell in _epicRarity)
            {
                spell.SetUnit(unit, this);
            }
            foreach (var spell in _legendRarity)
            {
                spell.SetUnit(unit, this);
            }
            foreach (var spell in _beginningRarity)
            {
                spell.SetUnit(unit, this);
            }
            foreach (var spell in _godRarity)
            {
                spell.SetUnit(unit, this);
            }
        }

        internal void GenerateRandomSpell()
        {
            var spellProbabilities = _inGameTemplate.GetSpellProbability();

            float[] probabilities = new float[]
            {
                spellProbabilities.god,
                spellProbabilities.beginning,
                spellProbabilities.legend,
                spellProbabilities.epic,
                spellProbabilities.rare,
                spellProbabilities.common,
            };

            float rand = Random.value;
            float cumulative = 0f;
            int index = 0;

            for (int i = 0; i < probabilities.Length; i++)
            {
                cumulative += probabilities[i];
                if (rand < cumulative)
                {
                    index = i;
                    break;
                }
            }
            
            List<UISpellButton> selectedList;
            switch (index)
            {
                case 0:
                    selectedList = _godRarity;
                    break;
                case 1:
                    selectedList = _beginningRarity;
                    break;
                case 2:
                    selectedList = _legendRarity;
                    break;
                case 3:
                    selectedList = _epicRarity;
                    break;
                case 4:
                    selectedList = _rareRarity;
                    break;
                default:
                    selectedList = _commonRarity;
                    break;
            }

            var spell = selectedList[Random.Range(0, 3)];
            spell.Show();

            onChangeSpell?.Invoke(_spells);
        }

        internal void GenerateRandomNextSpell(ERarity rarity)
        {
            List<UISpellButton> selectedList;

            switch (rarity)
            {
                case ERarity.Common:
                    selectedList = _rareRarity;
                    break;
                case ERarity.Rare:
                    selectedList = _epicRarity;
                    break;
                case ERarity.Epic:
                    selectedList = _legendRarity;
                    break;
                case ERarity.Legend:
                    selectedList = _beginningRarity;
                    break;
                case ERarity.Beginning:
                    selectedList = _godRarity;
                    break;
                default:
                    return;
            }

            var spell = selectedList[Random.Range(0, 3)];
            spell.Show();

            onChangeSpell?.Invoke(_spells);
        }

        internal void GenerateRarityUpperRandomSpell(ERarity rarity)
        {
            List<UISpellButton> selectedList;

            switch (rarity)
            {
                case ERarity.Epic:
                    selectedList = _epicRarity;
                    selectedList.AddRange(_legendRarity);
                    break;
                case ERarity.Legend:
                    selectedList = _legendRarity;
                    selectedList.AddRange(_beginningRarity);
                    break;
                case ERarity.Beginning:
                    selectedList = _beginningRarity;
                    selectedList.AddRange(_godRarity);
                    break;
                default:
                    return;
            }

            var spell = selectedList[Random.Range(0, 6)];
            spell.Show();

            onChangeSpell?.Invoke(_spells);
        }
    }
}