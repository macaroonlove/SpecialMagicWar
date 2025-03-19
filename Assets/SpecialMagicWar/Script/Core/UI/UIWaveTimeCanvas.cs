using FrameWork.UIBinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class UIWaveTimeCanvas : UIBase
    {
        #region 바인딩
        enum Texts
        {
            WaveInfoText,
            RemainingText,
        }
        #endregion

        private TextMeshProUGUI _waveInfoText;
        private TextMeshProUGUI _remainingText;

        private WaveSystem _waveSystem;

        protected override void Initialize()
        {
            BindText(typeof(Texts));

            _waveInfoText = GetText((int)Texts.WaveInfoText);
            _remainingText = GetText((int)Texts.RemainingText);

            _waveSystem = BattleManager.Instance.GetSubSystem<WaveSystem>();
            _waveSystem.onWaveChanged += OnWaveChanged;
        }

        private void OnDestroy()
        {
            _waveSystem.onWaveChanged -= OnWaveChanged;
        }

        private void OnWaveChanged(int waveInfo, float remaining)
        {
            _waveInfoText.text = $"웨이브{waveInfo}";

            StartCoroutine(UpdateRemainingTime(remaining));
        }

        private IEnumerator UpdateRemainingTime(float remaining)
        {
            while (remaining > 1)
            {
                remaining -= 1f;

                int minutes = Mathf.FloorToInt(remaining / 60);
                int seconds = Mathf.FloorToInt(remaining % 60);
                _remainingText.text = $"{minutes:00}:{seconds:00}";
                yield return new WaitForSeconds(1f);
            }
        }
    }
}