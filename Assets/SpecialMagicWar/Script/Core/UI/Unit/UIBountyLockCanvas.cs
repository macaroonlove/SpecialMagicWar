using FrameWork.UIBinding;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpecialMagicWar.Core
{
    public class UIBountyLockCanvas : UIBase
    {
        #region ¹ÙÀÎµù
        enum Texts
        {
            RemainingText,
        }
        #endregion

        private TextMeshProUGUI _remainingText;

        private WaitForSeconds _wfs = new WaitForSeconds(1);

        protected override void Initialize()
        {
            BindText(typeof(Texts));

            _remainingText = GetText((int)Texts.RemainingText);
        }

        internal void Show(Toggle bountyToggle, int disableTime)
        {
            base.Show(true);
            
            bountyToggle.isOn = false;
            bountyToggle.interactable = false;

            StartCoroutine(UpdateLockTime(bountyToggle, disableTime));
        }

        private IEnumerator UpdateLockTime(Toggle bountyToggle, int disableTime)
        {
            int time = disableTime;

            while (time > 0)
            {
                _remainingText.text = time.ToString();
                yield return _wfs;
                time--;
            }
            
            bountyToggle.interactable = true;
            Hide(true);
        }
    }
}