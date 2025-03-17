using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(menuName = "Templates/Etc/Rarity", fileName = "Rarity", order = 0)]
    public class RarityTemplate : ScriptableObject
    {
        [SerializeField] private string _displayName;
        [SerializeField] private ERarity _rarity;
        [SerializeField] private Sprite _sprite;

        #region ������Ƽ
        public string displayName => _displayName;
        public ERarity rarity => _rarity;
        public Sprite sprite => _sprite;
        #endregion
    }
}
