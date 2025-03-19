using FrameWork;
using FrameWork.UIBinding;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpecialMagicWar.Core
{
    public class UIGenerateHolyAnimalButton : UIBase
    {
        #region ¹ÙÀÎµù
        enum Buttons
        {
            Icon,
            GenerateButton,
        }
        enum Images
        {
            Icon,
        }
        enum Texts
        {
            ConditionText,
        }
        enum CanvasGroupControllers
        {
            Impossible,
        }
        #endregion

        private HolyAnimalCreateSystem _holyAnimalCreateSystem;
        private HolyAnimalTemplate _template;
        private UIHolyAnimalCanvas _uiHolyAnimalCanvas;

        private Button _generateButton;
        private CanvasGroupController _impossible;
        private TextMeshProUGUI _conditionText;

        private bool _isCondition;
        private int _conditionCount;
        private bool _isBind = false;

        internal void Initialize(HolyAnimalTemplate template, UIHolyAnimalCanvas uiHolyAnimalCanvas)
        {
            Binding();

            _holyAnimalCreateSystem = BattleManager.Instance.GetSubSystem<HolyAnimalCreateSystem>();
            _template = template;
            _uiHolyAnimalCanvas = uiHolyAnimalCanvas;

            _conditionCount = CalcConditionCount(template.conditions);
            _conditionText.text = $"0/{_conditionCount}";

            GetImage((int)Images.Icon).sprite = template.sprite;

            _impossible.Hide(true);
        }

        private void Binding()
        {
            if (_isBind) return;

            BindButton(typeof(Buttons));
            BindImage(typeof(Images));
            BindText(typeof(Texts));
            BindCanvasGroupController(typeof(CanvasGroupControllers));

            _conditionText = GetText((int)Texts.ConditionText);
            _generateButton = GetButton((int)Buttons.GenerateButton);
            _generateButton.gameObject.SetActive(false);
            _impossible = GetCanvasGroupController((int)CanvasGroupControllers.Impossible);
            GetButton((int)Buttons.Icon).onClick.AddListener(EnableGenerateButton);
            _generateButton.onClick.AddListener(Generate);

            _isBind = true;
        }

        private int CalcConditionCount(List<SpawnCondition> conditions)
        {
            int result = 0;
            foreach (var condition in conditions)
            {
                result += condition.count;
            }
            return result;
        }

        private void SetGenerateButtonActive(bool isActive)
        {
            _generateButton.gameObject.SetActive(isActive);
        }

        private void EnableGenerateButton()
        {
            if (_isCondition)
            {
                _uiHolyAnimalCanvas?.SelectAnyButton();
                SetGenerateButtonActive(true);
            }
        }

        internal void DisableGenerateButton()
        {
            SetGenerateButtonActive(false);
        }

        internal void Show(UISpellCanvas uiSpellCanvas)
        {
            Condition(uiSpellCanvas.spells);

            uiSpellCanvas.onChangeSpell += Condition;
        }

        internal void Hide(UISpellCanvas uiSpellCanvas)
        {
            DisableGenerateButton();

            uiSpellCanvas.onChangeSpell -= Condition;
        }

        private void Condition(List<UISpellButton> spells)
        {
            int selectedCount = 0;

            var conditions = _template.conditions;
            List<(UISpellButton, int)> selectedSpells = new List<(UISpellButton, int)>();

            foreach (var condition in conditions)
            {
                foreach (var spell in spells)
                {
                    if (spell.template == condition.spellTemplate && spell.spellCount >= condition.count)
                    {
                        selectedSpells.Add((spell, condition.count));
                        selectedCount += condition.count;
                        break;
                    }
                }
            }

            _isCondition = selectedSpells.Count == conditions.Count;

            if (_isCondition)
            {
                _conditionText.color = Color.green;
            }
            else
            {
                _conditionText.color = Color.white;
                DisableGenerateButton();
            }
            _conditionText.text = $"{selectedCount}/{_conditionCount}";
        }

        private void Generate()
        {
            if (_isCondition == false) return;

            if (_holyAnimalCreateSystem.CreateUnit(_template) == false) return;

            _impossible.Show(true);
            _uiHolyAnimalCanvas.Close();
        }
    }
}