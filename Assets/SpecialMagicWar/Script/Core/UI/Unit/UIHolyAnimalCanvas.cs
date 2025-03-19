using FrameWork.Editor;
using FrameWork.UIBinding;
using SpecialMagicWar.Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class UIHolyAnimalCanvas : MonoBehaviour
    {
        [Label("신수 소환 버튼 프리팹"), SerializeField] private GameObject _prefab;
        [Label("신수 소환 버튼 부모 위치"), SerializeField] private Transform _parent;

        private UISpellCanvas _uiSpellCanvas;

        private List<UIGenerateHolyAnimalButton> _buttonList = new List<UIGenerateHolyAnimalButton>();

        internal void Initialize(UISpellCanvas uiSpellCanvas)
        {
            _uiSpellCanvas = uiSpellCanvas;

            var holyAnimals = SaveManager.Instance.profileData.ownedHolyAnimals;

            foreach (var animal in holyAnimals)
            {
                var template = GameDataManager.Instance.GetHolyAnimalTemplateById(animal.id);

                var instance = Instantiate(_prefab, _parent);
                var btn = instance.GetComponent<UIGenerateHolyAnimalButton>();
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

        private void OnEnable()
        {
            foreach (var btn in _buttonList)
            {
                btn.Show(_uiSpellCanvas);
            }
        }

        private void OnDisable()
        {
            foreach (var btn in _buttonList)
            {
                btn.Hide(_uiSpellCanvas);
            }
        }

        internal void SelectAnyButton()
        {
            foreach (var btn in _buttonList)
            {
                btn.DisableGenerateButton();
            }
        }
    }
}