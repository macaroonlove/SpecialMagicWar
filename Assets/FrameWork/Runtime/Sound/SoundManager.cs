using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrameWork.Sound
{
    /// <summary>
    /// ���ӿ� ���Ǵ� ��� �Ҹ��� �����ϴ� �Ŵ���
    /// </summary>
    public class SoundManager : PersistentSingleton<SoundManager>
    {
        /// <summary>
        /// SoundManager�� ����� GameObject�� Transform
        /// </summary>
        public static Transform Transform { get { return Instance.transform; } }

        #region �ߺ��� ����� Ŭ�� ���� ����
        public static bool IgnoreDuplicateBGM { get; set; }
        public static bool IgnoreDuplicateSFX { get; set; }
        public static bool IgnoreDuplicateUISound { get; set; }
        public static bool IgnoreDuplicateVoice { get; set; }
        #endregion

        #region ���� ����
        public static float GlobalVolume { get; set; }
        public static float GlobalBGMVolume { get; set; }
        public static float GlobalSFXVolume { get; set; }
        public static float GlobalUIVolume { get; set; }
        public static float GlobalVoiceVolume { get; set; }
        #endregion

        #region ����� Ŭ���� ������ ���� Dictionary
        private static Dictionary<int, Audio> bgmAudio;
        private static Dictionary<int, Audio> sfxAudio;
        private static Dictionary<int, Audio> uiSoundAudio;
        private static Dictionary<int, Audio> voiceAudio;
        private static Dictionary<int, Audio> audioPool;
        #endregion

        private static bool initialized = false;

        protected override void Initialize()
        {
            if (!initialized)
            {
                bgmAudio = new Dictionary<int, Audio>();
                sfxAudio = new Dictionary<int, Audio>();
                uiSoundAudio = new Dictionary<int, Audio>();
                voiceAudio = new Dictionary<int, Audio>();
                audioPool = new Dictionary<int, Audio>();

                GlobalVolume = 0.5f;
                GlobalBGMVolume = 0.5f;
                GlobalSFXVolume = 0.5f;
                GlobalUIVolume = 0.5f;
                GlobalVoiceVolume = 0.5f;

                IgnoreDuplicateBGM = false;
                IgnoreDuplicateSFX = false;
                IgnoreDuplicateUISound = false;
                IgnoreDuplicateVoice = false;

                initialized = true;
            }
        }

        #region �� �ε� �� �ʱ�ȭ ����
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        /// ��� ������� �����ϰ� �����մϴ�.
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            RemoveNonPersistAudio(bgmAudio);
            RemoveNonPersistAudio(sfxAudio);
            RemoveNonPersistAudio(uiSoundAudio);
            RemoveNonPersistAudio(voiceAudio);
        }

        /// <summary>
        /// ���� ��ȯ�� ��, ���ӵ��� ���� ������� Audio Dictionary���� ����
        /// </summary>
        private static void RemoveNonPersistAudio(Dictionary<int, Audio> audioDict)
        {
            List<int> keys = new List<int>(audioDict.Keys);

            foreach (int key in keys)
            {
                Audio audio = audioDict[key];

                if (!audio.Persist && audio.IsUpdated)
                {
                    Destroy(audio.AudioSource);
                    audioDict.Remove(key);
                }
            }

            keys = new List<int>(audioPool.Keys);

            foreach (int key in keys)
            {
                Audio audio = audioPool[key];

                if (!audio.Persist && audio.IsUpdated)
                {
                    audioPool.Remove(key);
                }
            }
        }
        #endregion

        #region ����� ������Ʈ
        private void Update()
        {
            UpdateAllAudio(bgmAudio);
            UpdateAllAudio(sfxAudio);
            UpdateAllAudio(uiSoundAudio);
            UpdateAllAudio(voiceAudio);
        }

        /// <summary>
        /// Audio Dictionary�� ��� ����� ���¸� ������Ʈ�մϴ�.
        /// </summary>
        private static void UpdateAllAudio(Dictionary<int, Audio> audioDict)
        {
            List<int> keys = new List<int>(audioDict.Keys);

            foreach (int key in keys)
            {
                Audio audio = audioDict[key];
                audio.Update();

                // ������� ������ �����Ǿ��ٸ�
                if (!audio.IsPlaying && !audio.IsPaused)
                {
                    Destroy(audio.AudioSource);

                    audioPool.Add(key, audio);
                    audio.IsPooled = true;
                    audioDict.Remove(key);
                }
            }
        }
        #endregion

        #region ��ƿ��Ƽ
        /// <summary>
        /// AudioType�� ���� ����� ������ ��ȯ�մϴ�.
        /// </summary>
        private static Dictionary<int, Audio> GetAudioTypeDictionary(Audio.AudioType audioType)
        {
            Dictionary<int, Audio> audioDict = new Dictionary<int, Audio>();

            switch (audioType)
            {
                case Audio.AudioType.BGM:
                    audioDict = bgmAudio;
                    break;
                case Audio.AudioType.SFX:
                    audioDict = sfxAudio;
                    break;
                case Audio.AudioType.UI:
                    audioDict = uiSoundAudio;
                    break;
                case Audio.AudioType.Voice:
                    audioDict = voiceAudio;
                    break;
            }

            return audioDict;
        }

        /// <summary>
        /// Ǯ���� ������� �����Ͽ� �ش� Audio Dictionary�� �ٽ� �߰��մϴ�.
        /// </summary>
        public static bool RestoreAudioFromPool(Audio.AudioType audioType, int audioID)
        {
            if (audioPool.ContainsKey(audioID))
            {
                Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);
                audioDict.Add(audioID, audioPool[audioID]);
                audioPool.Remove(audioID);

                return true;
            }

            return false;
        }
        #endregion

        #region GetAudio (����� ��ü�� �޾ƿ��� �޼���)
        public static Audio GetBGMAudio(int audioID)
        {
            return GetAudio(Audio.AudioType.BGM, true, audioID);
        }

        public static Audio GetBGMAudio(AudioClip audioClip)
        {
            return GetAudio(Audio.AudioType.BGM, true, audioClip);
        }

        public static Audio GetSFXAudio(int audioID)
        {
            return GetAudio(Audio.AudioType.SFX, true, audioID);
        }

        public static Audio GetSFXAudio(AudioClip audioClip)
        {
            return GetAudio(Audio.AudioType.SFX, true, audioClip);
        }

        public static Audio GetUISoundAudio(int audioID)
        {
            return GetAudio(Audio.AudioType.UI, true, audioID);
        }

        public static Audio GetUISoundAudio(AudioClip audioClip)
        {
            return GetAudio(Audio.AudioType.UI, true, audioClip);
        }

        public static Audio GetVoiceAudio(int audioID)
        {
            return GetAudio(Audio.AudioType.Voice, true, audioID);
        }

        public static Audio GetVoiceAudio(AudioClip audioClip)
        {
            return GetAudio(Audio.AudioType.Voice, true, audioClip);
        }

        private static Audio GetAudio(Audio.AudioType audioType, bool usePool, int audioID)
        {
            Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);

            if (audioDict.ContainsKey(audioID))
            {
                return audioDict[audioID];
            }

            if (usePool && audioPool.ContainsKey(audioID) && audioPool[audioID].Type == audioType)
            {
                return audioPool[audioID];
            }

            return null;
        }

        private static Audio GetAudio(Audio.AudioType audioType, bool usePool, AudioClip audioClip)
        {
            Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);

            List<Audio> audioList = new List<Audio>(audioDict.Values);
            foreach (var audio in audioList)
            {
                if (audio.Clip == audioClip && audio.Type == audioType)
                {
                    if (audioDict.ContainsKey(audio.AudioID))
                    {
                        return audio;
                    }
                }
            }

            if (usePool)
            {
                List<Audio> poolList = new List<Audio>(audioPool.Values);
                foreach (var audio in poolList)
                {
                    if (audio.Clip == audioClip && audio.Type == audioType)
                    {
                        if (audioPool.ContainsKey(audio.AudioID))
                        {
                            return audio;
                        }
                    }
                }
            }

            return null;
        }
        #endregion

        #region CreateAudio (����� ��ü�� �����ϴ� �޼���)
        private static int CreateAudio(Audio.AudioType audioType, AudioClip clip, float volume, bool loop, bool persist, float fadeInSeconds, float fadeOutSeconds, Transform sourceTransform)
        {
            if (clip == null)
            {
                Debug.Log("<color=#ff0000>[SoundManager] Audio clip�� �����ϴ�.</color>", clip);
            }

            Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);
            bool ignoreDuplicateAudio = GetAudioTypeIgnoreDuplicateSetting(audioType);

            if (ignoreDuplicateAudio)
            {
                // �ߺ��� ������� �ִ��� Ž��
                Audio duplicateAudio = GetAudio(audioType, true, clip);

                if (duplicateAudio != null)
                {
                    return duplicateAudio.AudioID;
                }
            }

            Audio audio = new Audio(audioType, clip, loop, persist, volume, fadeInSeconds, fadeOutSeconds, sourceTransform);

            audioDict.Add(audio.AudioID, audio);

            return audio.AudioID;
        }

        /// <summary>
        /// ������ ����� ������ ������� ���� �ߺ� ���� ������ �˻��մϴ�.
        /// </summary>
        private static bool GetAudioTypeIgnoreDuplicateSetting(Audio.AudioType audioType)
        {
            switch (audioType)
            {
                case Audio.AudioType.BGM:
                    return IgnoreDuplicateBGM;
                case Audio.AudioType.SFX:
                    return IgnoreDuplicateSFX;
                case Audio.AudioType.UI:
                    return IgnoreDuplicateUISound;
                case Audio.AudioType.Voice:
                    return IgnoreDuplicateVoice;
                default:
                    return false;
            }
        }
        #endregion

        #region PlayAudio (������� �����Ű�� �޼���)
        /// <param name="clip">����� ����� Ŭ��</param>
        /// <param name="volume">������ ����</param>
        /// <param name="loop">������ �ݺ� ����Ǵ��� ����</param>
        /// <param name="persist">������� �� ��ȯ ���� �����Ǵ��� ����</param>
        /// <param name="fadeInSeconds">������� ���̵� ���Ͽ� ��ǥ ������ �����ϴ� �� �ʿ��� �ð�(���� �������� ���� ���)</param>
        /// <param name="fadeOutSeconds">������� ���̵� �ƿ��Ͽ� ��ǥ ������ �����ϴ� �� �ʿ��� �ð�(���� �������� ���� ���)</param>
        /// <param name="currentMusicfadeOutSeconds">���� ���� ������� ���̵� �ƿ��ϴ� �� �ʿ��� �ð��Դϴ�. �� ���� �����Ǹ� ��ü ���̵� �ƿ� �ð��� ����ϴ�. -1�� ���޵Ǹ� ���� ������ �ڽ��� ���̵� �ƿ� �ð��� �����մϴ�.</param>
        /// <param name="sourceTransform">������ �ҽ��� �Ǵ� ��ȯ(3D ������� �˴ϴ�). 3D ������� �ʿ� ������ null�� ����ϼ���.</param>
        public static int PlayBGM(AudioClip clip, float volume = 1f, bool loop = false, bool persist = false, float fadeInSeconds = 1f, float fadeOutSeconds = 1f, float currentMusicfadeOutSeconds = -1f, Transform sourceTransform = null)
        {
            return PlayAudio(Audio.AudioType.BGM, clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds, currentMusicfadeOutSeconds, sourceTransform);
        }

        /// <param name="clip">����� ����� Ŭ��</param>
        /// <param name="volume">������ ����</param>
        /// <param name="loop">������ �ݺ� ����Ǵ��� ����</param>
        /// <param name="persist">������� �� ��ȯ ���� �����Ǵ��� ����</param>
        /// <param name="fadeInSeconds">������� ���̵� ���Ͽ� ��ǥ ������ �����ϴ� �� �ʿ��� �ð�(���� �������� ���� ���)</param>
        /// <param name="fadeOutSeconds">������� ���̵� �ƿ��Ͽ� ��ǥ ������ �����ϴ� �� �ʿ��� �ð�(���� �������� ���� ���)</param>
        /// <param name="currentMusicfadeOutSeconds">���� ���� ������� ���̵� �ƿ��ϴ� �� �ʿ��� �ð��Դϴ�. �� ���� �����Ǹ� ��ü ���̵� �ƿ� �ð��� ����ϴ�. -1�� ���޵Ǹ� ���� ������ �ڽ��� ���̵� �ƿ� �ð��� �����մϴ�.</param>
        /// <param name="sourceTransform">������ �ҽ��� �Ǵ� ��ȯ(3D ������� �˴ϴ�). 3D ������� �ʿ� ������ null�� ����ϼ���.</param>
        public static int PlaySFX(AudioClip clip, float volume = 1f, bool loop = false, bool persist = false, float fadeInSeconds = 1f, float fadeOutSeconds = 1f, float currentMusicfadeOutSeconds = -1f, Transform sourceTransform = null)
        {
            return PlayAudio(Audio.AudioType.SFX, clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds, currentMusicfadeOutSeconds, sourceTransform);
        }

        /// <param name="clip">����� ����� Ŭ��</param>
        /// <param name="volume">������ ����</param>
        /// <param name="loop">������ �ݺ� ����Ǵ��� ����</param>
        /// <param name="persist">������� �� ��ȯ ���� �����Ǵ��� ����</param>
        /// <param name="fadeInSeconds">������� ���̵� ���Ͽ� ��ǥ ������ �����ϴ� �� �ʿ��� �ð�(���� �������� ���� ���)</param>
        /// <param name="fadeOutSeconds">������� ���̵� �ƿ��Ͽ� ��ǥ ������ �����ϴ� �� �ʿ��� �ð�(���� �������� ���� ���)</param>
        /// <param name="currentMusicfadeOutSeconds">���� ���� ������� ���̵� �ƿ��ϴ� �� �ʿ��� �ð��Դϴ�. �� ���� �����Ǹ� ��ü ���̵� �ƿ� �ð��� ����ϴ�. -1�� ���޵Ǹ� ���� ������ �ڽ��� ���̵� �ƿ� �ð��� �����մϴ�.</param>
        /// <param name="sourceTransform">������ �ҽ��� �Ǵ� ��ȯ(3D ������� �˴ϴ�). 3D ������� �ʿ� ������ null�� ����ϼ���.</param>
        public static int PlayUISound(AudioClip clip, float volume = 1f, bool loop = false, bool persist = false, float fadeInSeconds = 1f, float fadeOutSeconds = 1f, float currentMusicfadeOutSeconds = -1f, Transform sourceTransform = null)
        {
            return PlayAudio(Audio.AudioType.UI, clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds, currentMusicfadeOutSeconds, sourceTransform);
        }

        /// <param name="clip">����� ����� Ŭ��</param>
        /// <param name="volume">������ ����</param>
        /// <param name="loop">������ �ݺ� ����Ǵ��� ����</param>
        /// <param name="persist">������� �� ��ȯ ���� �����Ǵ��� ����</param>
        /// <param name="fadeInSeconds">������� ���̵� ���Ͽ� ��ǥ ������ �����ϴ� �� �ʿ��� �ð�(���� �������� ���� ���)</param>
        /// <param name="fadeOutSeconds">������� ���̵� �ƿ��Ͽ� ��ǥ ������ �����ϴ� �� �ʿ��� �ð�(���� �������� ���� ���)</param>
        /// <param name="currentMusicfadeOutSeconds">���� ���� ������� ���̵� �ƿ��ϴ� �� �ʿ��� �ð��Դϴ�. �� ���� �����Ǹ� ��ü ���̵� �ƿ� �ð��� ����ϴ�. -1�� ���޵Ǹ� ���� ������ �ڽ��� ���̵� �ƿ� �ð��� �����մϴ�.</param>
        /// <param name="sourceTransform">������ �ҽ��� �Ǵ� ��ȯ(3D ������� �˴ϴ�). 3D ������� �ʿ� ������ null�� ����ϼ���.</param>
        public static int PlayVoice(AudioClip clip, float volume = 1f, bool loop = false, bool persist = false, float fadeInSeconds = 1f, float fadeOutSeconds = 1f, float currentMusicfadeOutSeconds = -1f, Transform sourceTransform = null)
        {
            return PlayAudio(Audio.AudioType.Voice, clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds, currentMusicfadeOutSeconds, sourceTransform);
        }

        private static int PlayAudio(Audio.AudioType audioType, AudioClip clip, float volume, bool loop, bool persist, float fadeInSeconds, float fadeOutSeconds, float currentMusicfadeOutSeconds, Transform sourceTransform)
        {
            int audioID = CreateAudio(audioType, clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds, sourceTransform);

            if (audioType == Audio.AudioType.BGM)
            {
                StopAllBGM(currentMusicfadeOutSeconds);
            }

            GetAudio(audioType, false, audioID).Play();

            return audioID;
        }
        #endregion

        #region StopAudio (Ư�� ������� ������ ������Ű�� �޼���)
        public static void StopSFX(int audioID, float fadeOutSeconds = -1)
        {
            StopAudio(Audio.AudioType.SFX, audioID, fadeOutSeconds);
        }

        public static void StopSFX(AudioClip clip, float fadeOutSeconds = -1)
        {
            StopAudio(Audio.AudioType.SFX, clip, fadeOutSeconds);
        }

        public static void StopUISound(AudioClip clip, float fadeOutSeconds = -1)
        {
            StopAudio(Audio.AudioType.UI, clip, fadeOutSeconds);
        }

        public static void StopUISound(int audioID, float fadeOutSeconds = -1)
        {
            StopAudio(Audio.AudioType.UI, audioID, fadeOutSeconds);
        }

        public static void StopVoice(AudioClip clip, float fadeOutSeconds = -1)
        {
            StopAudio(Audio.AudioType.Voice, clip, fadeOutSeconds);
        }

        public static void StopVoice(int audioID, float fadeOutSeconds = -1)
        {
            StopAudio(Audio.AudioType.Voice, audioID, fadeOutSeconds);
        }

        private static void StopAudio(Audio.AudioType audioType, int audioID, float fadeOutSeconds)
        {
            Audio audio = GetAudio(audioType, true, audioID);

            if (audio == null) return;

            if (fadeOutSeconds > 0)
            {
                audio.FadeOutSeconds = fadeOutSeconds;
            }
            audio.Stop();
        }

        private static void StopAudio(Audio.AudioType audioType, AudioClip clip, float fadeOutSeconds)
        {
            Audio audio = GetAudio(audioType, true, clip);

            if (audio == null) return;

            if (fadeOutSeconds > 0)
            {
                audio.FadeOutSeconds = fadeOutSeconds;
            }
            audio.Stop();
        }
        #endregion

        #region StopAllAudio (��� ������� ������ ������Ű�� �޼���)
        public static void StopAll(float musicFadeOutSeconds = -1)
        {
            StopAllBGM(musicFadeOutSeconds);
            StopAllSFX(musicFadeOutSeconds);
            StopAllUISound(musicFadeOutSeconds);
            StopAllVoice(musicFadeOutSeconds);
        }

        public static void StopAllBGM(float fadeOutSeconds = -1)
        {
            StopAllAudio(Audio.AudioType.BGM, fadeOutSeconds);
        }

        public static void StopAllSFX(float fadeOutSeconds = -1)
        {
            StopAllAudio(Audio.AudioType.SFX, fadeOutSeconds);
        }

        public static void StopAllUISound(float fadeOutSeconds = -1)
        {
            StopAllAudio(Audio.AudioType.UI, fadeOutSeconds);
        }

        public static void StopAllVoice(float fadeOutSeconds = -1)
        {
            StopAllAudio(Audio.AudioType.Voice, fadeOutSeconds);
        }

        private static void StopAllAudio(Audio.AudioType audioType, float fadeOutSeconds)
        {
            Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);

            List<int> keys = new List<int>(audioDict.Keys);
            foreach (int key in keys)
            {
                Audio audio = audioDict[key];
                if (fadeOutSeconds > 0)
                {
                    audio.FadeOutSeconds = fadeOutSeconds;
                }
                audio.Stop();
            }
        }
        #endregion

        #region PauseAudio (Ư�� ������� �Ͻ� ������Ű�� �޼���)
        public static void PauseSFX(int audioID)
        {
            PauseAudio(Audio.AudioType.SFX, audioID);
        }

        public static void PauseSFX(AudioClip clip)
        {
            PauseAudio(Audio.AudioType.SFX, clip);
        }

        public static void PauseUISound(AudioClip clip)
        {
            PauseAudio(Audio.AudioType.UI, clip);
        }

        public static void PauseUISound(int audioID)
        {
            PauseAudio(Audio.AudioType.UI, audioID);
        }

        public static void PauseVoice(AudioClip clip)
        {
            PauseAudio(Audio.AudioType.Voice, clip);
        }

        public static void PauseVoice(int audioID)
        {
            PauseAudio(Audio.AudioType.Voice, audioID);
        }

        private static void PauseAudio(Audio.AudioType audioType, int audioID)
        {
            Audio audio = GetAudio(audioType, true, audioID);

            if (audio == null) return;

            audio.Pause();
        }

        private static void PauseAudio(Audio.AudioType audioType, AudioClip clip)
        {
            Audio audio = GetAudio(audioType, true, clip);

            if (audio == null) return;

            audio.Pause();
        }
        #endregion

        #region PauseAllAudio (��� ������� �Ͻ� ������Ű�� �޼���)
        public static void PauseAll()
        {
            PauseAllBGM();
            PauseAllSFX();
            PauseAllUISound();
            PauseAllVoice();
        }

        public static void PauseAllBGM()
        {
            PauseAllAudio(Audio.AudioType.BGM);
        }

        public static void PauseAllSFX()
        {
            PauseAllAudio(Audio.AudioType.SFX);
        }

        public static void PauseAllUISound()
        {
            PauseAllAudio(Audio.AudioType.UI);
        }

        public static void PauseAllVoice()
        {
            PauseAllAudio(Audio.AudioType.Voice);
        }

        private static void PauseAllAudio(Audio.AudioType audioType)
        {
            Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);

            List<int> keys = new List<int>(audioDict.Keys);
            foreach (int key in keys)
            {
                Audio audio = audioDict[key];
                audio.Pause();
            }
        }
        #endregion

        #region ResumeAudio (Ư�� ������� �Ͻ� ������Ű�� �޼���)
        public static void ResumeSFX(int audioID)
        {
            ResumeAudio(Audio.AudioType.SFX, audioID);
        }

        public static void ResumeSFX(AudioClip clip)
        {
            ResumeAudio(Audio.AudioType.SFX, clip);
        }

        public static void ResumeUISound(AudioClip clip)
        {
            ResumeAudio(Audio.AudioType.UI, clip);
        }

        public static void ResumeUISound(int audioID)
        {
            ResumeAudio(Audio.AudioType.UI, audioID);
        }

        public static void ResumeVoice(AudioClip clip)
        {
            ResumeAudio(Audio.AudioType.Voice, clip);
        }

        public static void ResumeVoice(int audioID)
        {
            ResumeAudio(Audio.AudioType.Voice, audioID);
        }

        private static void ResumeAudio(Audio.AudioType audioType, int audioID)
        {
            Audio audio = GetAudio(audioType, true, audioID);

            if (audio == null) return;

            audio.Resume();
        }

        private static void ResumeAudio(Audio.AudioType audioType, AudioClip clip)
        {
            Audio audio = GetAudio(audioType, true, clip);

            if (audio == null) return;

            audio.Resume();
        }
        #endregion

        #region ResumeAllAudio (�Ͻ������� ��� ������� �ٽ� ���۽�Ű�� �޼���)
        public static void ResumeAll()
        {
            ResumeAllBGM();
            ResumeAllSFX();
            ResumeAllUISound();
            ResumeAllVoice();
        }

        public static void ResumeAllBGM()
        {
            ResumeAllAudio(Audio.AudioType.BGM);
        }

        public static void ResumeAllSFX()
        {
            ResumeAllAudio(Audio.AudioType.SFX);
        }

        public static void ResumeAllUISound()
        {
            ResumeAllAudio(Audio.AudioType.UI);
        }

        public static void ResumeAllVoice()
        {
            ResumeAllAudio(Audio.AudioType.Voice);
        }

        private static void ResumeAllAudio(Audio.AudioType audioType)
        {
            Dictionary<int, Audio> audioDict = GetAudioTypeDictionary(audioType);

            List<int> keys = new List<int>(audioDict.Keys);
            foreach (int key in keys)
            {
                Audio audio = audioDict[key];
                audio.Resume();
            }
        }
        #endregion
    }
}