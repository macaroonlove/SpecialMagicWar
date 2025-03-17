using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace FrameWork.GameSettings
{
    [AddComponentMenu("GameSettings/UI/Screen Mode Dropdown")]
    public class ScreenModeDropdown : MonoBehaviour
    {
		[SerializeField] private TMP_Dropdown targetElement;

		private void Reset()
		{
			targetElement = GetComponentInChildren<TMP_Dropdown>();
		}

		private void Awake()
		{
			if (targetElement == null)
			{
				targetElement = GetComponentInChildren<TMP_Dropdown>();
				if (targetElement == null)
				{
#if UNITY_EDITOR
					Debug.LogError("[ScreenModeDropdown] TextMeshPro Dropdown ������Ʈ�� ã�� �� �����ϴ�.", gameObject);
#endif
					return;
				}
			}
		}

		private void Start()
		{
			GameSettingsManager.ResolutionChanged += RefreshControlDelayed;
		}

		private void OnDestroy()
		{
			GameSettingsManager.ResolutionChanged -= RefreshControlDelayed;
		}

		private void OnEnable()
		{
			RefreshControl();
		}

		private void RefreshControlDelayed()
		{
			StartCoroutine(RefreshControlSequence());
		}

		private IEnumerator RefreshControlSequence()
		{
			// �ػ� ��ȯ�� ������ ������ �߻��ϹǷ� ���� ���� ��ġ�� ���� ��ٷ��� �Ѵ�.
			yield return new WaitForFixedUpdate();
			RefreshControl();
		}

		private void RefreshControl()
		{
			if (targetElement != null)
			{
				targetElement.ClearOptions();
				targetElement.onValueChanged.RemoveAllListeners();

				List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
				foreach (string mode in GameSettingsManager.ScreenModes)
				{
					var currentLanguage = LocalizationSettings.SelectedLocale;
					string option = LocalizationSettings.StringDatabase.GetLocalizedString("SettingTable", mode, currentLanguage);

					options.Add(new TMP_Dropdown.OptionData(option));
				}

				targetElement.AddOptions(options);
				targetElement.value = GameSettingsManager.ScreenModeIndex;
				targetElement.onValueChanged.AddListener(OnValueChange);
			}
		}

		private void OnValueChange(int idx)
		{
			GameSettingsManager.ScreenModeIndex = idx;
		}
	}
}
