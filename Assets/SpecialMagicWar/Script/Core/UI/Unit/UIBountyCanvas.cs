using FrameWork.Editor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpecialMagicWar.Core
{
    public class UIBountyCanvas : MonoBehaviour
    {
        [Label("적 소환 버튼 프리팹"), SerializeField] private GameObject _prefab;
        [Label("적 소환 버튼 부모 위치"), SerializeField] private Transform _parent;
        [Label("비활성 시간"), SerializeField] private int _disableTime = 60;

        private Toggle _bountyToggle;
        private UIBountyLockCanvas _bountyLockCanvas;

        private List<UIGenerateBountyButton> _buttonList = new List<UIGenerateBountyButton>();

        internal void Initialize(Toggle bountyToggle, UIBountyLockCanvas bountyLockCanvas)
        {
            _bountyToggle = bountyToggle;
            _bountyLockCanvas = bountyLockCanvas;

            var bountys = GameDataManager.Instance.bountyLibrary.templates;

            foreach (var template in bountys)
            {
                var instance = Instantiate(_prefab, _parent);
                var btn = instance.GetComponent<UIGenerateBountyButton>();
                btn.Initialize(template, this);

                _buttonList.Add(btn);
            }
        }

        private void OnDestroy()
        {
            foreach (var btn in _buttonList)
            {
                Destroy(btn);
            }

            _buttonList.Clear();
        }

        internal void DisableGenerate()
        {
            _bountyLockCanvas.Show(_bountyToggle, _disableTime);
        }
    }
}