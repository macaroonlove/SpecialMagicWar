using DG.Tweening;
using FrameWork.UIBinding;
using SpecialMagicWar.Core;
using TMPro;
using UnityEngine;

namespace FrameWork.UI
{
    public class UIIntVariableLogEffect : UIBase
    {
        #region ¹ÙÀÎµù
        enum Objects
        {
            Log,
        }
        #endregion

        [SerializeField] private int _maxTextLength = 3;
        [SerializeField] private float _holdingTime = 1;
        [SerializeField] private float _moveTime = 1;

        private PoolSystem _poolSystem;
        private GameObject _logPrefab;
        private int prevValue = int.MinValue;
        private Color _originColor;

        private Sequence _cachedSequence;

        protected override void Initialize()
        {
            BindObject(typeof(Objects));

            _poolSystem = CoreManager.Instance.GetSubSystem<PoolSystem>();
            _logPrefab = GetObject((int)Objects.Log);
            _logPrefab.SetActive(false);

            _originColor = _logPrefab.GetComponent<TextMeshProUGUI>().color;
        }

        internal void Initialize(int value)
        {
            prevValue = value;
        }

        internal void OnChangeValue(int value)
        {
            var diff = value - prevValue;
            prevValue = value;
            
            if (diff > 0)
            {
                var instance = _poolSystem.Spawn(_logPrefab, _holdingTime + _moveTime, transform);
                var text = instance.GetComponent<TextMeshProUGUI>();

                instance.SetActive(true);
                _cachedSequence?.Kill();

                text.text = $"+{diff.Format(_maxTextLength)}";
                text.color = _originColor;

                _cachedSequence = DOTween.Sequence();
                _cachedSequence.AppendInterval(_holdingTime);
                _cachedSequence.Append(text.DOColor(new(_originColor.r, _originColor.g, _originColor.b, 0), _moveTime));
            }
        }
    }
}