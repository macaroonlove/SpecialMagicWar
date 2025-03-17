using FrameWork.Sound;
//using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.GameSettings
{
	/// <summary>
	/// PlayerFrefs�� ������Ʈ���� ����� �������� �����ϴ� �Ŵ���
	/// </summary>
	public static class GameSettingsManager
	{
		private static readonly Vector2 MinScreenSize = new Vector2(1024, 768);

		// ------------------------------------------------------------------------------------------------------------
		#region ���� ����

		public static event System.Action OnRestoreComplete;

		/// <summary> ����� ������ �����մϴ�. </summary>
		public static void RestoreSettings()
		{
			// �г��� ����
			string displayName = PlayerPrefs.GetString($"Settings.DisplayName", "�̸��� �����ּ���.");
			DisplayNameText = displayName;

			// ���� ��� ���� ����
			bool isSprintToggle = GetControlMode(ControlTarget.Sprint);
			SetControlMode(ControlTarget.Sprint, isSprintToggle);
			bool isCrouchToggle = GetControlMode(ControlTarget.Crouch);
			SetControlMode(ControlTarget.Crouch, isCrouchToggle);

			// �ִ� FPS ���� ����
			int maxFPSIndex = PlayerPrefs.GetInt("Settings.MaxFPS", FPSOptions.Length - 1);
			Application.targetFrameRate = FPSOptions[maxFPSIndex];
			
			// ���콺 ���� ���� ����
			float rotateSensitivity = PlayerPrefs.GetFloat("Settings.RotateSensitivity", 1f);
			RotateSensitivity = rotateSensitivity;

			// �þ� �Ÿ� ���� ����
			float viewDistance = PlayerPrefs.GetFloat("Settings.ViewDistance", 1000f);
			ViewDistance = viewDistance;

			// ����Ƽ ���� ����
			int q = PlayerPrefs.GetInt("Settings.Quality", QualitySettings.GetQualityLevel());
			QualitySettings.SetQualityLevel(q);

			// ȭ�� ��� ���� ����
			float screenBrightness = ScreenBrightness;
			ScreenBrightness = screenBrightness;

			// �Ҹ� ���� ����
			var masterVolume = PlayerPrefs.GetFloat($"Settings.Volume.0", 0.5f);
			SetSoundVolume(Audio.AudioType.Master, masterVolume);
			var musicVolume = PlayerPrefs.GetFloat($"Settings.Volume.1", 0.5f);
			SetSoundVolume(Audio.AudioType.BGM, musicVolume);
			var sfxVolume = PlayerPrefs.GetFloat($"Settings.Volume.2", 0.5f);
			SetSoundVolume(Audio.AudioType.SFX, sfxVolume);
			var uiVolume = PlayerPrefs.GetFloat($"Settings.Volume.3", 1f);
			SetSoundVolume(Audio.AudioType.UI, uiVolume);
			var voiceVolume = PlayerPrefs.GetFloat($"Settings.Volume.4", 1f);
			SetSoundVolume(Audio.AudioType.Voice, voiceVolume);

			// ����ũ ���� ����
			float voiceTreshold = GetMicrophoneThreshold(MicrophoneType.Voice);
			SetMicrophoneThreshold(MicrophoneType.Voice, voiceTreshold);
			float ambientSountTreshold = GetMicrophoneThreshold(MicrophoneType.AmbientSound);
			SetMicrophoneThreshold(MicrophoneType.AmbientSound, ambientSountTreshold);

			OnRestoreComplete?.Invoke();
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------
		#region �⺻

		#region �г���
		// �̱��÷����� ���
		public static event System.Action<string> DisplayNameChanged;
		// ��Ƽ�÷����� ���
		// public static event System.Action DisplayNameChanged;

		/// <summary>
		/// �г����� �����մϴ�.
		/// </summary>
		public static string DisplayNameText
		{
			get => PlayerPrefs.GetString($"Settings.DisplayName", "�̸��� �����ּ���.");
			set
			{
				PlayerPrefs.SetString("Settings.DisplayName", value);
				PlayerPrefs.Save();

				// �̱� �÷����� ���
				DisplayNameChanged?.Invoke(value);

				// PhotonNetwork.LocalPlayer.NickName = value;
				// DisplayNameChanged?.Invoke();
			}
		}

		public static event System.Action<bool> DisplayNameVisibleChanged;

		/// <summary>
		/// �г����� ȭ�鿡 ������ ���θ� �����մϴ�.
		/// </summary>
		public static bool DisplayNameVisible
		{
			get => System.Convert.ToBoolean(PlayerPrefs.GetInt($"Settings.DisplayNameVisible", 0));
			set
			{
				PlayerPrefs.SetInt("Settings.DisplayNameVisible", System.Convert.ToInt16(value));
				PlayerPrefs.Save();

				DisplayNameVisibleChanged?.Invoke(value);
			}
		}
		#endregion

		#region ���� ���
		public enum ControlTarget
		{
			Sprint,
			Crouch,
		}

		// ȸ�� ���� ������ �˸��� ���� �ڵ鷯
		public static event System.Action<ControlTarget, bool> ControlModeChanged;

		/// <summary> 
		/// ���� ����� �����մϴ�.
		/// </summary>
		public static void SetControlMode(ControlTarget type, bool value)
		{
			int idx = (int)type;

			PlayerPrefs.SetInt($"Settings.ControlMode.{idx}", System.Convert.ToInt16(value));
			PlayerPrefs.Save();

			ControlModeChanged?.Invoke(type, value);
		}

		/// <summary> 
		/// ������ִ� ���� ����� �޾ƿɴϴ�.
		/// </summary>
		public static bool GetControlMode(ControlTarget type)
		{
			int idx = (int)type;

			bool value = System.Convert.ToBoolean(PlayerPrefs.GetInt($"Settings.ControlMode.{idx}", 0));

			return value;
		}
		#endregion

		#region �ִ� FPS
		private static int[] FPSOptions = { 25, 30, 60, 80, 120, 144, 200, 240, -1 };
		public static string[] FPSOptionNames = { "25 FPS", "30 FPS", "60 FPS", "80 FPS", "120 FPS", "144 FPS", "200 FPS", "240 FPS", "NoLlimit" };

		/// <summary>
		/// FPS �ɼ��� �ε����� �������ų� �����մϴ�.
		/// </summary>
		public static int MaxFPSIndex
		{
			get => System.Array.IndexOf(FPSOptions, Application.targetFrameRate);
			set
			{
				if (value >= 0 && value < FPSOptions.Length)
				{
					Application.targetFrameRate = FPSOptions[value];
					PlayerPrefs.SetInt("Settings.MaxFPS", value);
					PlayerPrefs.Save();
				}
			}
		}
		#endregion

		#region ȸ�� ����
		// ȸ�� ���� ������ �˸��� ���� �ڵ鷯
		public static event System.Action<float> RotateSensitivityChanged;

		/// <summary> 
		/// ���콺 ������ �������ų� �����մϴ�. 
		/// </summary>
		public static float RotateSensitivity
		{
			get => PlayerPrefs.GetFloat("Settings.RotateSensitivity", 1f);
			set
			{
				PlayerPrefs.SetFloat("Settings.RotateSensitivity", value);
				PlayerPrefs.Save();
				RotateSensitivityChanged?.Invoke(value);
			}
		}
		#endregion

		#region �þ߰Ÿ�
		// ȸ�� ���� ������ �˸��� ���� �ڵ鷯
		public static event System.Action<float> ViewDistanceChanged;

		/// <summary> 
		/// �þ� �Ÿ��� �������ų� �����մϴ�. 
		/// </summary>
		public static float ViewDistance
		{
			get => PlayerPrefs.GetFloat("Settings.ViewDistance", 1f);
			set
			{
				PlayerPrefs.SetFloat("Settings.ViewDistance", value);
				PlayerPrefs.Save();
				ViewDistanceChanged?.Invoke(value);
			}
		}
        #endregion

        #endregion
        // ------------------------------------------------------------------------------------------------------------
        #region �׷���

        #region �ػ�
        private static List<Resolution> Resolutions = new List<Resolution>();

		// �ػ� ������ �˸��� ���� �ڵ鷯
		public static event System.Action ResolutionChanged;

		/// <summary> 
		/// ����� �� �ִ� ȭ�� �ػ󵵸� ��Ÿ���� ���ڿ� ���. 
		/// </summary>
		public static List<string> ScreenResolutions
		{
			get
			{
				List<string> l = new List<string>(Screen.resolutions.Length);
				Resolutions.Clear();

				if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
				{
					for (int i = 0; i < Screen.resolutions.Length; i++)
					{
						if (Screen.resolutions[i].width >= MinScreenSize.x && Screen.resolutions[i].height >= MinScreenSize.y)
						{
							Resolutions.Add(Screen.resolutions[i]);
							l.Add(Screen.resolutions[i].ToString());
						}
					}
				}
				else
				{
					for (int i = 0; i < Screen.resolutions.Length; i++)
					{
						if (Screen.resolutions[i].width >= MinScreenSize.x && Screen.resolutions[i].height >= MinScreenSize.y)
						{
							var res = $"{Screen.resolutions[i].width} x {Screen.resolutions[i].height}";
							if (!l.Contains(res))
							{
								Resolutions.Add(Screen.resolutions[i]);
								l.Add(res);
							}
						}
					}
				}

				return l;
			}
		}

		/// <summary> 
		/// ���� ȭ�� �ػ� �ε����� �������ų� �����մϴ�. 
		/// �̴� �����Ǵ� �ػ� ��Ͽ��� �ػ󵵸� ��Ÿ���� ���� ���Դϴ�.
		/// �����Ǵ� �ػ� ����� ScreenResolutions �Ӽ��� ���� �����ɴϴ�. �ػ� �ε����� ������ �� ������ -1�� ��ȯ�մϴ�. 
		/// </summary>
		public static int ScreenResolutionIndex
		{
			get
			{
				int _screenResolutionIdx = -1;

				int w = Screen.width;
				int h = Screen.height;
				if (Screen.fullScreenMode != FullScreenMode.Windowed)
				{
					w = Screen.currentResolution.width;
					h = Screen.currentResolution.height;
				}

				for (int i = Resolutions.Count - 1; i >= 0; i--)
				{
					if (w == Resolutions[i].width && h == Resolutions[i].height)
					{
						_screenResolutionIdx = i;
						if (Screen.fullScreenMode != FullScreenMode.ExclusiveFullScreen)
						{
							break;
						}
					}
				}

				return _screenResolutionIdx;
			}
			set
			{
				if (value >= 0 && value < Resolutions.Count)
				{
					Screen.SetResolution(Resolutions[value].width, Resolutions[value].height, Screen.fullScreenMode);
					PlayerPrefs.Save();

					ResolutionChanged?.Invoke();
				}
			}
		}
		#endregion

		#region ȭ�� ���
		public static string[] ScreenModes = { "FullScreen", "Windowed", "NoBorder" };

		public static int ScreenModeIndex
		{
			get
			{
				// ��üȭ��, â���, �׵θ� ���� ��带 ������ �ε����� ��ȯ
				return Screen.fullScreenMode switch
				{
					FullScreenMode.ExclusiveFullScreen => 0,
					FullScreenMode.Windowed => 1,
					FullScreenMode.FullScreenWindow => 2,
					_ => 2,
				};
			}
			set
			{
				// �ε����� ���� ȭ�� ��� ����
				FullScreenMode mode = value switch
				{
					0 => FullScreenMode.ExclusiveFullScreen,
					1 => FullScreenMode.Windowed,
					2 => FullScreenMode.FullScreenWindow,
					_ => FullScreenMode.FullScreenWindow,
				};
				SetScreenMode(mode);
			}
		}

		private static void SetScreenMode(FullScreenMode mode)
		{
			int w = Screen.width;
			int h = Screen.height;
			if (Screen.fullScreenMode != FullScreenMode.Windowed)
			{
				w = Screen.currentResolution.width;
				h = Screen.currentResolution.height;
			}

			Screen.SetResolution(w, h, mode);
			PlayerPrefs.Save();

			ResolutionChanged?.Invoke();
		}
		#endregion

		#region �׷��� ǰ��
		/// <summary> 
		/// ���ǵ� ǰ�� ���� ����� �ε����� ���� ����� ǰ�� ������ �������ų� �����մϴ�. 
		/// �̷��� ǰ�� ������ ǰ�� ���� �����⿡�� �����˴ϴ�.
		/// �޴�: Edit > Project Settings > Quality
		/// </summary>
		public static int GFXQualityLevelIndex
		{
			get => QualitySettings.GetQualityLevel();
			set
			{
#if !UNITY_EDITOR
				QualitySettings.SetQualityLevel(value);
#endif
				PlayerPrefs.SetInt("Settings.Quality", value);
				PlayerPrefs.Save();
			}
		}
        #endregion

        #region ȭ�� ���
        /// <summary> 
		/// ȭ�� ��⸦ �������ų� �����մϴ�. 
		/// </summary>
        public static float ScreenBrightness
		{
			get => PlayerPrefs.GetFloat("Settings.ScreenBrightness", 0f);
			set
			{
				VolumeController.Instance?.SetScreenBrightness(value);
				PlayerPrefs.SetFloat("Settings.ScreenBrightness", value);
				PlayerPrefs.Save();
			}
		}
        #endregion

        #endregion
        // ------------------------------------------------------------------------------------------------------------
        #region �Ҹ� ����

        // �Ҹ� ������ ����� �� �˸��� ���� �ڵ鷯
        public static event System.Action<Audio.AudioType, float> SoundVolumeChanged;

		/// <summary>
		/// ������ �Ҹ� ������ ������ �����մϴ�.
		/// ������ 0(�Ҹ� ����)�� 1(�ִ�) ������ �ε� �Ҽ��� ���Դϴ�.
		/// </summary>
		public static void SetSoundVolume(Audio.AudioType type, float value)
		{
			int idx = (int)type;
			value = Mathf.Clamp01(value);
			PlayerPrefs.SetFloat($"Settings.Volume.{idx}", value);
			PlayerPrefs.Save();

			switch (type)
			{
				case Audio.AudioType.Master:
					SoundManager.GlobalVolume = value;
					break;
				case Audio.AudioType.BGM:
					SoundManager.GlobalBGMVolume = value;
					break;
				case Audio.AudioType.SFX:
					SoundManager.GlobalSFXVolume = value;
					break;
				case Audio.AudioType.UI:
					SoundManager.GlobalUIVolume = value;
					break;
				case Audio.AudioType.Voice:
					SoundManager.GlobalVoiceVolume = value;
					break;
			}

			SoundVolumeChanged?.Invoke(type, value);
		}

		/// <summary>
		/// ������ �Ҹ� ������ ������ �����ɴϴ�.
		/// ���� 0(�Ҹ� ����)�� 1(�ִ�) ������ �ε� �Ҽ��� ���Դϴ�.
		/// </summary>
		public static float GetSoundVolume(Audio.AudioType type)
		{
			switch (type)
			{
				case Audio.AudioType.Master:
					return SoundManager.GlobalVolume;
				case Audio.AudioType.BGM:
					return SoundManager.GlobalBGMVolume;
				case Audio.AudioType.SFX:
					return SoundManager.GlobalSFXVolume;
				case Audio.AudioType.UI:
					return SoundManager.GlobalUIVolume;
				case Audio.AudioType.Voice:
					return SoundManager.GlobalVoiceVolume;
			}
			return 0f;
		}

		#endregion

		#region ����ũ �Ӱ谪 ����
		public enum MicrophoneType
		{
			Voice,
			AmbientSound,
		}

		public static event System.Action<MicrophoneType, float> MicrophoneThresholdChanged;

		/// <summary>
		/// ������ ������ �Ӱ谪�� �����մϴ�.
		/// ������ 0(�Ҹ� ����)�� 1(�ִ�) ������ �ε� �Ҽ��� ���Դϴ�.
		/// </summary>
		public static void SetMicrophoneThreshold(MicrophoneType type, float value)
		{
			int idx = (int)type;
			value = Mathf.Clamp01(value);
			PlayerPrefs.SetFloat($"Settings.MicrophoneThreshold.{idx}", value);
			PlayerPrefs.Save();
			
			MicrophoneThresholdChanged?.Invoke(type, value);
		}

		/// <summary>
		/// ������ ������ �Ӱ谪�� �����ɴϴ�.
		/// ������ 0(�Ҹ� ����)�� 1(�ִ�) ������ �ε� �Ҽ��� ���Դϴ�.
		/// </summary>
		public static float GetMicrophoneThreshold(MicrophoneType type)
		{
			int idx = (int)type;

			float value = PlayerPrefs.GetFloat($"Settings.MicrophoneThreshold.{idx}", 0f);
			
			return value;
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------

	}
}