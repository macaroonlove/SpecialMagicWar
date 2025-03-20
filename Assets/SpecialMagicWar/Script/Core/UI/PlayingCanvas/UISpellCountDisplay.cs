using FrameWork.UIBinding;
using TMPro;

namespace SpecialMagicWar.Core
{
    public class UISpellCountDisplay : UIBase
    {
        #region ¹ÙÀÎµù
        enum Texts
        {
            Value,
        }
        #endregion

        private TextMeshProUGUI _valueText;

        private UISpellCanvas _uiSpellCanvas;

        internal void Initialize(UISpellCanvas uiSpellCanvas)
        {
            BindText(typeof(Texts));

            _valueText = GetText((int)Texts.Value);

            _uiSpellCanvas = uiSpellCanvas;
            _uiSpellCanvas.onChangeSpellCount += OnChangeSpellCount;
        }

        private void OnDestroy()
        {
            _uiSpellCanvas.onChangeSpellCount -= OnChangeSpellCount;
        }

        private void OnChangeSpellCount(int count)
        {
            _valueText.text = $"{count} / 25";
        }
    }
}