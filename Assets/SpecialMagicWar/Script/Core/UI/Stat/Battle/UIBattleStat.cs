using FrameWork.Tooltip;
using FrameWork.UIBinding;
using TMPro;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [RequireComponent(typeof(TooltipTrigger))]
    public abstract class UIBattleStat<T> : UIBase where T : struct
    {
        #region ���ε�
        enum Texts
        {
            Value
        }
        #endregion

        protected enum EDataType
        {
            Add,
            Increase,
            Multiplier,
        }

        protected TooltipTrigger _tooltip;
        protected Unit _unit;
        protected T _prevValue;

        private TextMeshProUGUI _valueText;

        protected override void Awake()
        {
            base.Awake();

            _tooltip = GetComponent<TooltipTrigger>();

            BindText(typeof(Texts));
            _valueText = GetText((int)Texts.Value);
        }

        private void OnDestroy()
        {
            _tooltip = null;
        }

        public virtual void Initialize(Unit unit)
        {
            _unit = unit;
        }

        public virtual void Deinitialize()
        {
            _unit = null;
        }

        protected void Update()
        {
            if (_unit == null) return;

            T newValue = GetValue();
            if (newValue.Equals(_prevValue) == false)
            {
                _valueText.text = GetValueText();
                ApplyTooltip();

                _prevValue = newValue;
            }
        }

        private void ApplyTooltip()
        {
            if (_unit == null) return;

            _tooltip.SetText("BaseStat", GetBaseValue());
            _tooltip.SetText("DetailStat", GetTooltip());
        }

        protected abstract T GetValue();
        protected abstract string GetBaseValue();
        protected abstract string GetValueText();
        protected abstract string GetTooltip();

        protected string ValueFormat(float value, EDataType type, int decimalPoint = 0)
        {
            string str = "";
            switch (type)
            {
                case EDataType.Add:
                    str = value.ToString("+#;-#;0");
                    break;
                case EDataType.Increase:
                    {
                        var finalValue = value * 100;
                        if (decimalPoint != 0)
                        {
                            if (finalValue == Mathf.Floor(finalValue) && finalValue == Mathf.Ceil(finalValue))
                            {
                                decimalPoint = 0;
                            }
                        }
                        if (value > 0)
                        {
                            str = finalValue.ToString($"F{decimalPoint}") + "% ����";
                        }
                        else if (value < 0)
                        {
                            str = finalValue.ToString($"F{decimalPoint}") + "% ����";
                        }
                        break;
                    }
                case EDataType.Multiplier:
                    {
                        var finalValue = value * 100;
                        if (decimalPoint != 0)
                        {
                            if (finalValue == Mathf.Floor(finalValue) && finalValue == Mathf.Ceil(finalValue))
                            {
                                decimalPoint = 0;
                            }
                        }
                        if (value > 0)
                        {
                            str = finalValue.ToString($"F{decimalPoint}") + "% ���";
                        }
                        else if (value < 0)
                        {
                            str = finalValue.ToString($"F{decimalPoint}") + "% �϶�";
                        }
                        break;
                    }
            }
            return str;
        }
    }
}