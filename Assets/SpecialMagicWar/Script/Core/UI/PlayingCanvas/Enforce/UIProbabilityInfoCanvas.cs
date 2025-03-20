using FrameWork.UIBinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class UIProbabilityInfoCanvas : UIBase
    {
        #region ¹ÙÀÎµù
        enum Texts
        {
            CommonText,
            RareText,
            EpicText,
            LegendText,
            BeginningText,
            GodText,
        }
        #endregion

        private TextMeshProUGUI _commonText;
        private TextMeshProUGUI _rareText;
        private TextMeshProUGUI _epicText;
        private TextMeshProUGUI _legendText;
        private TextMeshProUGUI _beginningText;
        private TextMeshProUGUI _godText;

        internal void Initialize()
        {
            BindText(typeof(Texts));

            _commonText = GetText((int)Texts.CommonText);
            _rareText = GetText((int)Texts.RareText);
            _epicText = GetText((int)Texts.EpicText);
            _legendText = GetText((int)Texts.LegendText);
            _beginningText = GetText((int)Texts.BeginningText);
            _godText = GetText((int)Texts.GodText);
        }

        internal void Show(SpellProbabilityList spellProbability)
        {
            _commonText.text = (spellProbability.common * 100).ToString("0.###") + "%";
            _rareText.text = (spellProbability.rare * 100).ToString("0.###") + "%";
            _epicText.text = (spellProbability.epic * 100).ToString("0.###") + "%";
            _legendText.text = (spellProbability.legend * 100).ToString("0.###") + "%";
            _beginningText.text = (spellProbability.beginning * 100).ToString("0.###") + "%";
            _godText.text = (spellProbability.god * 100).ToString("0.###") + "%";
        }
    }
}