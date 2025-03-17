using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FrameWork.GameSettings
{
    [AddComponentMenu("GameSettings/UI/Resolution Dropdown")]
    public class ResolutionDropdown : MonoBehaviour
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
					Debug.LogError("[ResolutionDropdown] TextMeshPro Dropdown ������Ʈ�� ã�� �� �����ϴ�.", gameObject);
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
				foreach (string resolution in GameSettingsManager.ScreenResolutions)
				{
					options.Add(new TMP_Dropdown.OptionData(resolution));
				}

				targetElement.AddOptions(options);
				targetElement.value = GameSettingsManager.ScreenResolutionIndex;
				targetElement.onValueChanged.AddListener(OnValueChange);
			}
		}

		private void OnValueChange(int idx)
		{
			GameSettingsManager.ScreenResolutionIndex = idx;
		}
	}
}
