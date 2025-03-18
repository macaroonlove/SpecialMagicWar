using FrameWork;
using FrameWork.UIBinding;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// Show 상태라면 실행되는 스킬 버튼
    /// </summary>
    public class UIActiveSkillExecuteButton : UIBase
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
        enum CanvasGroupControllers
        {
            HaveSpell,
        }
        #endregion

        private Image _cooldownTimeImage;
        private CanvasGroupController _haveSpell;

        private Unit _unit;
        private ActiveSkillAbility _activeSkillAbility;
        [SerializeField] private ActiveSkillTemplate _template;

        private float _inverseMaxCoolDownTime;
        private float _currentCoolDownTime;

        #region 프로퍼티
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
            BindImage(typeof(Images));
            BindButton(typeof(Buttons));
            BindCanvasGroupController(typeof(CanvasGroupControllers));

            _cooldownTimeImage = GetImage((int)Images.CooldownTimeImage);
            _haveSpell = GetCanvasGroupController((int)CanvasGroupControllers.HaveSpell);

            GetImage((int)Images.DisableIcon).sprite = _template.sprite;
            GetImage((int)Images.EnableIcon).sprite = _template.sprite;

            GetButton((int)Buttons.HaveSpell).onClick.AddListener(CompositeSkill);

            BattleManager.Instance.playerCreateSystem.onCreatePlayer += SetUnit;
        }

        private void OnDestroy()
        {
            BattleManager.Instance.playerCreateSystem.onCreatePlayer -= SetUnit;
        }

        private void SetUnit(AgentUnit unit)
        {
            _unit = unit;
            _activeSkillAbility = _unit.GetAbility<ActiveSkillAbility>();
            Show();
        }

        internal void Show()
        {
            _haveSpell.Show(true);

            CalcMaxCoolDownTime();
            _currentCoolDownTime = 0;
        }

        internal void Hide()
        {
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

        }
    }
}