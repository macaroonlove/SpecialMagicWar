using FrameWork.UIBinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpecialMagicWar.Core
{
    public class UIActiveItemExecuteButton : UIBase, IPointerDownHandler, IPointerMoveHandler
    {
        #region ���ε�
        enum Images
        {
            Icon,
            CooldownTimeImage,
        }
        enum Texts
        {
            NeedCost,
            CooldownTimeText,
        }
        #endregion

        private enum EActionType
        {
            Item_1,
            Item_2,
            Item_3,
        }

        [SerializeField] private EActionType _actionType;

        private Image _cooldownTimeImage;

        private TextMeshProUGUI _needCostText;
        private TextMeshProUGUI _coolDownTimeText;

        private Color _lackCostColor = new Color(1, 0.27f, 0);

        private InputSystem _inputSystem;
        private CostSystem _costSystem;
        private ActiveItemRangeRenderer _rangeRenderer;
        private ActiveItemTemplate _template;

        private int _currentCost;
        private float _inverseMaxCoolDownTime;
        private float _currentCoolDownTime;

        private bool _isActiveRangeRenderer;
        private bool _isInteractable;
        private float _threshold = 0.1f;
        private bool _isPointerDown;
        private Vector2 _cachedPointerDownPosition;
        private Vector3 _cachedLastRendererPosition;

        #region ������Ƽ
        private bool IsInteractable
        {
            get
            {
                if (_isInteractable == false) return false;

                return true;
            }
        }

        private int finalNeedCost
        {
            get
            {
                int result = _template.needCost;

                return result;
            }
        }

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
            BindText(typeof(Texts));

            _cooldownTimeImage = GetImage((int)Images.CooldownTimeImage);
            _needCostText = GetText((int)Texts.NeedCost);
            _coolDownTimeText = GetText((int)Texts.CooldownTimeText);

            _cooldownTimeImage.gameObject.SetActive(false);

            _threshold *= _threshold;

            BattleManager.Instance.onBattleInitialize += GetActiveItem;
            BattleManager.Instance.onBattleDeinitialize += Hide;
            BattleManager.Instance.onBattleManagerDestroy += Unsubscribe;
        }

        private void Unsubscribe()
        {
            if (BattleManager.Instance == null) return;

            BattleManager.Instance.onBattleInitialize -= GetActiveItem;
            BattleManager.Instance.onBattleDeinitialize -= Hide;
            BattleManager.Instance.onBattleManagerDestroy -= Unsubscribe;
        }

        private void GetActiveItem()
        {
            var template = CoreManager.Instance.GetSubSystem<ActiveItemSystem>().GetSelectedItem((int)_actionType);
            if (template != null)
            {
                Show(template);
            }
            else
            {
                base.Hide(true);
            }
        }

        private void Show(ActiveItemTemplate template)
        {
            GetImage((int)Images.Icon).sprite = template.sprite;

            _template = template;

            _needCostText.text = $"Cost: {finalNeedCost}";

            _rangeRenderer = BattleManager.Instance.GetSubSystem<ActiveItemRangeRenderer>();
            _inputSystem = BattleManager.Instance.GetSubSystem<InputSystem>();
            _costSystem = BattleManager.Instance.GetSubSystem<CostSystem>();
            _costSystem.onChangedCost += OnChangeCost;

            CalcMaxCoolDownTime();
            _currentCoolDownTime = 0;

            CheckInteractable();

            InputBinding();

            base.Show(true);
        }

        internal void Hide()
        {
            base.Hide(true);

            _template = null;
            _rangeRenderer = null;

            _costSystem.onChangedCost -= OnChangeCost;
            _costSystem = null;

            InputCancelBinding();
            _inputSystem = null;
        }

        #region Input Binding
        private void InputBinding()
        {
            if (_template.unitType == EUnitType.None)
            {
                switch (_actionType)
                {
                    case EActionType.Item_1:
                        _inputSystem.onItem_1 += ShortcutNoneRenderer;
                        break;
                    case EActionType.Item_2:
                        _inputSystem.onItem_2 += ShortcutNoneRenderer;
                        break;
                    case EActionType.Item_3:
                        _inputSystem.onItem_3 += ShortcutNoneRenderer;
                        break;
                }

                return;
            }

            switch (_template.rangeType)
            {
                case ERangeType.All:
                    switch (_actionType)
                    {
                        case EActionType.Item_1:
                            _inputSystem.onItem_1 += ShortcutAllRenderer;
                            break;
                        case EActionType.Item_2:
                            _inputSystem.onItem_2 += ShortcutAllRenderer;
                            break;
                        case EActionType.Item_3:
                            _inputSystem.onItem_3 += ShortcutAllRenderer;
                            break;
                    }
                    break;
                case ERangeType.Circle:
                    switch (_actionType)
                    {
                        case EActionType.Item_1:
                            _inputSystem.onItem_1 += ShortcutCircleRenderer;
                            break;
                        case EActionType.Item_2:
                            _inputSystem.onItem_2 += ShortcutCircleRenderer;
                            break;
                        case EActionType.Item_3:
                            _inputSystem.onItem_3 += ShortcutCircleRenderer;
                            break;
                    }
                    break;
            }
        }

        private void InputCancelBinding()
        {
            if (_template.unitType == EUnitType.None)
            {
                switch (_actionType)
                {
                    case EActionType.Item_1:
                        _inputSystem.onItem_1 -= ShortcutNoneRenderer;
                        break;
                    case EActionType.Item_2:
                        _inputSystem.onItem_2 -= ShortcutNoneRenderer;
                        break;
                    case EActionType.Item_3:
                        _inputSystem.onItem_3 -= ShortcutNoneRenderer;
                        break;
                }

                return;
            }

            switch (_template.rangeType)
            {
                case ERangeType.All:
                    switch (_actionType)
                    {
                        case EActionType.Item_1:
                            _inputSystem.onItem_1 -= ShortcutAllRenderer;
                            break;
                        case EActionType.Item_2:
                            _inputSystem.onItem_2 -= ShortcutAllRenderer;
                            break;
                        case EActionType.Item_3:
                            _inputSystem.onItem_3 -= ShortcutAllRenderer;
                            break;
                    }
                    break;
                case ERangeType.Circle:
                    switch (_actionType)
                    {
                        case EActionType.Item_1:
                            _inputSystem.onItem_1 -= ShortcutCircleRenderer;
                            break;
                        case EActionType.Item_2:
                            _inputSystem.onItem_2 -= ShortcutCircleRenderer;
                            break;
                        case EActionType.Item_3:
                            _inputSystem.onItem_3 -= ShortcutCircleRenderer;
                            break;
                    }
                    break;
            }
        }

        private void ShortcutNoneRenderer()
        {
            InitializeNoneRenderer();
        }

        private void ShortcutAllRenderer()
        {
            _isPointerDown = true;
            InitializeAllRenderer();
        }

        private void ShortcutCircleRenderer()
        {
            _isPointerDown = true;
            _isActiveRangeRenderer = true;
            InitializeCircleRenderer();
        }
        #endregion

        #region ������ ��� ���� ����
        private void OnChangeCost(int cost)
        {
            _currentCost = cost;
            CheckInteractable();
        }

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
                _cooldownTimeImage.gameObject.SetActive(false);
                CheckInteractable();
            }
            else
            {
                _coolDownTimeText.text = _currentCoolDownTime.ToString("F1");
                _cooldownTimeImage.fillAmount = _currentCoolDownTime * _inverseMaxCoolDownTime;
            }
        }

        /// <summary>
        /// �ش� �������� ��� �������� ���� üũ
        /// </summary>
        private void CheckInteractable()
        {
            bool isInteractable = true;
            if (_currentCoolDownTime > 0)
            {
                isInteractable = false;
                _isInteractable = false;
                _cooldownTimeImage.gameObject.SetActive(true);
                ResetPointer();
            }
            if (_currentCost < finalNeedCost)
            {
                isInteractable = false;
                _isInteractable = false;
                _needCostText.color = _lackCostColor;
                ResetPointer();
            }

            if (isInteractable == true)
            {
                _isInteractable = true;
                _cooldownTimeImage.gameObject.SetActive(false);
                _needCostText.color = Color.white;
            }
        }
        #endregion

        private void Update()
        {
            UpdateCoolDownTime();

            CheckPointerUp();
        }

        #region ������(���콺 �� ��ġ) ����
        public void OnPointerDown(PointerEventData eventData)
        {
            if (IsInteractable == false) return;
            if (_isPointerDown == true) return;

            _isPointerDown = true;
            _cachedPointerDownPosition = eventData.position;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (IsInteractable == false) return;
            if (_isPointerDown == false) return;
            if (_isActiveRangeRenderer == true) return;
            if (_template.rangeType == ERangeType.All) return;

            float distSquare = (_cachedPointerDownPosition - eventData.position).sqrMagnitude;

            if (distSquare > _threshold)
            {
                switch(_template.rangeType)
                {
                    case ERangeType.Circle:
                        InitializeCircleRenderer();
                        break;
                }
            }
        }

        private void CheckPointerUp()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButtonUp(0))
            {
                OnPointerUp();
            }
#elif UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                OnPointerUp();
            }
#endif
        }

        public void OnPointerUp()
        {
            if (IsInteractable == false) return;
            if (_isPointerDown == false) return;

            if (_template.unitType == EUnitType.None)
            {
                InitializeNoneRenderer();
            }
            else if (_isActiveRangeRenderer)
            {
                _rangeRenderer.Confirm(_template.delay, ExecuteItem);
            }
            else
            {
                switch (_template.rangeType)
                {
                    case ERangeType.All:
                        InitializeAllRenderer();
                        break;
                    case ERangeType.Circle:
                        InitializeCircleRenderer();
                        return;
                }
            }

            ResetPointer();
        }

        private void ResetPointer()
        {
            _isPointerDown = false;
            _cachedPointerDownPosition = Vector2.zero;
            _isActiveRangeRenderer = false;
        }
        #endregion

        #region ������ �ʱ�ȭ
        #region None Renderer
        private void InitializeNoneRenderer()
        {
            // TODO: ���� �߾� or ȭ���� �߾ӿ� �ִ� ���� ��ȯ�ϵ��� ����
            ExecuteItem(Vector3.zero);

            if (_template.delay > 0)
            {
                StartCoroutine(CoConfirmNoneRenderer(_template.delay));
            }
            else
            {
                ConfirmNoneRenderer();
            }
        }

        private IEnumerator CoConfirmNoneRenderer(float delay)
        {
            yield return new WaitForSeconds(delay);
            ConfirmNoneRenderer();
        }

        private void ConfirmNoneRenderer()
        {
            ExecuteEffect();

            ExecuteAfterDelayFX();
        }
        #endregion

        #region All Renderer
        private void InitializeAllRenderer()
        {
            _rangeRenderer.Show_AllRange(_template.unitType, _template.delay, (units) => {
                if (units.Count > 0)
                {
                    ExecuteEffect(units);

                    ExecuteAfterDelayFX();
                }
                else
                {
                    // TODO: ��� ���⿡ �����ߴٴ� UI�� �����ְų� ���� ���� ����ֱ�
                }
            }, ExecuteItem);
        }
        #endregion

        #region Circle Renderer
        private void InitializeCircleRenderer()
        {
            _rangeRenderer.Show_CircleRange(_template.unitType, _template.range, (units) => {
                if (units.Count > 0)
                {
                    ExecuteEffect(units);

                    ExecuteAfterDelayFX();
                }
                else
                {
                    // TODO: ��� ���⿡ �����ߴٴ� UI�� �����ְų� ���� ���� ����ֱ�
                }
            });
            _isActiveRangeRenderer = true;
        }
        #endregion
        #endregion

        private void ExecuteItem(Vector3 pos)
        {
            // FX ����
            ExecuteItemFX(pos);

            // ��Ÿ�� ����
            _currentCoolDownTime = finalCoolDownTime;
            CalcMaxCoolDownTime();

            // �ڽ�Ʈ ����
            _costSystem.PayCost(finalNeedCost);

            CheckInteractable();
        }

        #region ȿ�� ����
        private void ExecuteEffect()
        {
            foreach (var effect in _template.effects)
            {
                if (effect is GlobalEffect globalEffect)
                {
                    globalEffect.Execute();
                }
            }
        }

        private void ExecuteEffect(List<Unit> units)
        {
            foreach (var effect in _template.effects)
            {
                if (effect is ActiveItemEffect activeItemEffect)
                {
                    activeItemEffect.Execute(units);
                }
            }
        }
        #endregion

        #region FX
        private void ExecuteItemFX(Vector3 pos)
        {
            if (_template.itemFX != null)
            {
                _template.itemFX.Play(pos);
            }

            _cachedLastRendererPosition = pos;
        }

        private void ExecuteAfterDelayFX()
        {
            if (_template.delay <= 0) return;

            if (_template.afterDelayFX != null)
            {
                _template.afterDelayFX.Play(_cachedLastRendererPosition);
            }
        }
        #endregion
    }
}