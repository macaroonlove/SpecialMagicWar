using FrameWork.UIBinding;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpecialMagicWar.Core
{
    public class UIGenerateSpellButton : UIBase
    {
        #region ¹ÙÀÎµù
        enum Buttons
        {
            Button,
        }

        enum Texts
        {
            NeedCostText,
        }
        #endregion

        private UISpellCanvas _uiSpellCanvas;

        private TextMeshProUGUI _needCostText;
        private Button _button;

        private CostSystem _costSystem;
        private int _needCost;

        internal void Initialize(UISpellCanvas uiSpellCanvas)
        {
            _uiSpellCanvas = uiSpellCanvas;

            _costSystem = BattleManager.Instance.GetSubSystem<CostSystem>();
            _costSystem.onChangedCost += OnChangeCost;

            BindButton(typeof(Buttons));
            BindText(typeof(Texts));

            _needCost = 20;
            _needCostText = GetText((int)Texts.NeedCostText);
            _needCostText.text = _needCost.ToString();

            _button = GetButton((int)Buttons.Button);
            _button.onClick.AddListener(Generate);
        }

        private void OnDestroy()
        {
            _costSystem.onChangedCost -= OnChangeCost;
        }

        private void OnChangeCost(int currentCost)
        {
            if (_needCost > currentCost)
            {
                _button.enabled = false;
                _needCostText.color = Color.red;
            }
            else
            {
                _button.enabled = true;
                _needCostText.color = Color.white;
            }
        }

        private void Generate()
        {
            _uiSpellCanvas.GenerateRandomSpell();
            _costSystem.PayCost(_needCost);
            _needCost++;
            _needCostText.text = _needCost.ToString();
        }
    }
}