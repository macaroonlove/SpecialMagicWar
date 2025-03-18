using FrameWork.UIBinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class UISpellCanvas : UIBase
    {
        private List<UISpellButton> _commonRarity;
        private List<UISpellButton> _rareRarity;
        private List<UISpellButton> _epicRarity;
        private List<UISpellButton> _legendRarity;
        private List<UISpellButton> _beginningRarity;
        private List<UISpellButton> _godRarity;

        private readonly float[,] spellProbabilityTable = new float[,]
        {
            { 0.50f, 0.30f, 0.15f, 0.05f, 0.00f, 0.00f },
            { 0.40f, 0.30f, 0.20f, 0.07f, 0.03f, 0.00f },
            { 0.30f, 0.30f, 0.25f, 0.10f, 0.04f, 0.01f },
            { 0.25f, 0.25f, 0.25f, 0.15f, 0.08f, 0.02f },
            { 0.20f, 0.20f, 0.25f, 0.20f, 0.10f, 0.05f },
        };

        protected override void Initialize()
        {
            var _spells = GetComponentsInChildren<UISpellButton>().ToList();

            _commonRarity = _spells.Where(spell => spell.template.rarity.rarity == ERarity.Common).ToList();
            _rareRarity = _spells.Where(spell => spell.template.rarity.rarity == ERarity.Rare).ToList();
            _epicRarity = _spells.Where(spell => spell.template.rarity.rarity == ERarity.Epic).ToList();
            _legendRarity = _spells.Where(spell => spell.template.rarity.rarity == ERarity.Legend).ToList();
            _beginningRarity = _spells.Where(spell => spell.template.rarity.rarity == ERarity.Beginning).ToList();
            _godRarity = _spells.Where(spell => spell.template.rarity.rarity == ERarity.God).ToList();
        }

        internal void GenerateRandomSpell()
        {
            var level = 0;

            float[] probabilities = new float[]
            {
                spellProbabilityTable[level, 0], // Common
                spellProbabilityTable[level, 1], // Rare
                spellProbabilityTable[level, 2], // Epic
                spellProbabilityTable[level, 3], // Legend
                spellProbabilityTable[level, 4], // Beginning
                spellProbabilityTable[level, 5]  // God
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
                case 1:
                    selectedList = _commonRarity;
                    break;
                case 2:
                    selectedList = _rareRarity;
                    break;
                case 3:
                    selectedList = _epicRarity;
                    break;
                case 4:
                    selectedList = _legendRarity;
                    break;
                case 5:
                    selectedList = _beginningRarity;
                    break;
                case 6:
                    selectedList = _godRarity;
                    break;
                default:
                    selectedList = _commonRarity;
                    break;
            }

            var spell = selectedList[Random.Range(0, 3)];
            spell.Show();
        }
    }
}
