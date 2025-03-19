using FrameWork;
using FrameWork.UIBinding;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// Show 상태라면 실행되는 스펠 버튼
    /// </summary>
    public class UISpellButton : UIBase
    {
        #region 바인딩
        enum Images
        {
            DisableIcon,
            EnableIcon,
            CooldownTimeImage,
        }
        enum Buttons
        {
            HaveSpell,
        }
        enum Texts
        {
            CountText,
        }
        enum CanvasGroupControllers
        {
            HaveSpell,
        }
        #endregion

        [SerializeField] private ActiveSkillTemplate _template;

        private Image _cooldownTimeImage;
        private TextMeshProUGUI _countText;
        private CanvasGroupController _haveSpell;        

        private Unit _unit;
        private ActiveSkillAbility _activeSkillAbility;
        private UISpellCanvas _uiSpellCanvas;

        private float _inverseMaxCoolDownTime;
        private float _currentCoolDownTime;
        private int _spellCount;

        #region 프로퍼티
        internal ActiveSkillTemplate template => _template;
        internal int spellCount => _spellCount;

        private float finalCoolDownTime
        {
            get
            {
                float result = _template.cooldownTime;

                return result;
            }
        }
        #endregion

        protected override void Initialize()
        {
            if (_template == null) return;

            BindImage(typeof(Images));
            BindButton(typeof(Buttons));
            BindText(typeof(Texts));
            BindCanvasGroupController(typeof(CanvasGroupControllers));

            _cooldownTimeImage = GetImage((int)Images.CooldownTimeImage);
            _countText = GetText((int)Texts.CountText);
            _haveSpell = GetCanvasGroupController((int)CanvasGroupControllers.HaveSpell);

            var sprite = _template.sprite;
            GetImage((int)Images.DisableIcon).sprite = sprite;
            GetImage((int)Images.EnableIcon).sprite = sprite;

            GetButton((int)Buttons.HaveSpell).onClick.AddListener(CompositeSkill);
        }

        internal void SetUnit(AgentUnit unit, UISpellCanvas uiSpellCanvas)
        {
            _unit = unit;
            _activeSkillAbility = _unit.GetAbility<ActiveSkillAbility>();
            _uiSpellCanvas = uiSpellCanvas;

            Hide();
        }

        internal void Show()
        {
            _haveSpell.Show(true);

            CalcMaxCoolDownTime();
            _currentCoolDownTime = 0;
            
            _spellCount++;
            UpdateSpellText();
        }

        internal void Hide()
        {
            _spellCount = 0;
            _countText.text = "";
            _haveSpell.Hide(true);
        }

        #region 쿨타임
        private void CalcMaxCoolDownTime()
        {
            if (finalCoolDownTime == 0)
            {
                _inverseMaxCoolDownTime = 0;
            }
            else
            {
                _inverseMaxCoolDownTime = 1 / finalCoolDownTime;
            }
        }

        private void UpdateCoolDownTime()
        {
            if (_currentCoolDownTime == 0) return;

            _currentCoolDownTime -= Time.deltaTime;

            if (_currentCoolDownTime < 0)
            {
                _currentCoolDownTime = 0;
            }
            else
            {
                _cooldownTimeImage.fillAmount = _currentCoolDownTime * _inverseMaxCoolDownTime;
            }
        }
        #endregion

        private void Update()
        {
            if (_spellCount == 0) return;

            UpdateCoolDownTime();

            if (_currentCoolDownTime <= 0)
            {
                ExecuteSkill();
            }
        }

        internal void ExecuteSkill()
        {
            if (_activeSkillAbility == null) return;

            if (_activeSkillAbility.TryExecuteSkill(_template))
            {
                // 쿨타임 적용
                _currentCoolDownTime = finalCoolDownTime;
                CalcMaxCoolDownTime();
            }
        }

        private void CompositeSkill()
        {
            if (_spellCount < 3) return;
            if (_template.rarity.rarity == ERarity.God) return;

            _uiSpellCanvas.GenerateRandomNextSpell(_template.rarity.rarity);
            _spellCount -= 3;
            UpdateSpellText();

            if (_spellCount == 0)
            {
                Hide();
            }
        }

        private void UpdateSpellText()
        {
            _countText.text = _spellCount.ToString();

            if (_spellCount >= 3 && _template.rarity.rarity != ERarity.God) _countText.color = Color.green;
            else _countText.color = Color.white;
        }
    }
}