using UnityEngine;

namespace FrameWork.Sound
{
    public class Audio
    {
        public enum AudioType
        {
            Master,
            BGM,
            SFX,
            UI,
            Voice
        }

        /// <summary>
        /// ����� ID
        /// <para>����� ��ü�� ������ �� ���� +1�� �����Ѵ�.</para>
        /// </summary>
        public int AudioID { get; private set; }

        /// <summary>
        /// ������� Ÿ��
        /// </summary>
        public AudioType Type { get; private set; }

        /// <summary>
        /// ������� ��� ������ ����
        /// </summary>
        public bool IsPlaying { get; private set; }

        /// <summary>
        /// ������� �Ͻ� �����Ǿ����� ����
        /// </summary>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// ������� ������ �����Ǿ����� ����
        /// </summary>
        public bool IsStopped { get; private set; }

        /// <summary>
        /// ������� �����ǰ� �ּ��� �� �� ������Ʈ�Ǿ����� ����
        /// </summary>
        public bool IsUpdated { get; private set; }

        /// <summary>
        /// ������� ���� Ǯ���Ǿ� �ִ��� ����
        /// </summary>
        public bool IsPooled { get; set; }

        /// <summary>
        /// �� ��ȯ ��, ������� ���ӵ��� ����
        /// </summary>
        public bool Persist { get; set; }

        /// <summary>
        /// ������� ���̵� �� �Ǵµ� �ɸ��� �ð� (������ ��)
        /// <para>������� ��ǥ ������ ���� �������� ���� ���, ��ǥ ������ �����ϴ� �� �ҿ�Ǵ� �ð�</para>
        /// </summary>
        public float FadeInSeconds { get; set; }

        /// <summary>
        /// ������� ���̵� �ƿ� �Ǵµ� �ɸ��� �ð� (������ ��)
        /// <para>������� ��ǥ ������ ���� �������� ���� ���, ��ǥ ������ �����ϴ� �� �ҿ�Ǵ� �ð�</para>
        /// </summary>
        public float FadeOutSeconds { get; set; }

        /// <summary>
        /// ����� ����
        /// <para>SetVolume �޼��带 ����Ͽ� ���� ������ �� �ֽ��ϴ�.</para>
        /// </summary>
        public float Volume { get; private set; }

        /// <summary>
        /// ����� �ҽ�
        /// </summary>
        public AudioSource AudioSource { get; private set; }

        /// <summary>
        /// AudioSource�� � ��ü�� �߰������� ������ �� �ֽ��ϴ�.
        /// <para>�� ���� ������ ������ ���, SoundManager��ü�� �����˴ϴ�.</para>
        /// </summary>
        public Transform SourceTransform
        {
            get { return sourceTransform; }
            set
            {
                if (value == null)
                {
                    sourceTransform = SoundManager.Transform;
                }
                else
                {
                    sourceTransform = value;
                }
            }
        }

        #region AudioSource �ʵ� ����
        private AudioClip clip;
        private bool loop;
        private bool mute;
        private int priority;
        private float pitch;
        private float stereoPan;
        private float spatialBlend;
        private float reverbZoneMix;
        private float dopplerLevel;
        private float spread;
        private AudioRolloffMode rolloffMode;
        private float max3DDistance;
        private float min3DDistance;
        #endregion

        #region AudioSource ������Ƽ ����
        /// <summary>
        /// ����� Ŭ��
        /// <para>�� Ŭ���� �����ϸ�, �ڵ����� AudioSource�� Clip�� ����˴ϴ�.</para>
        /// </summary>
        public AudioClip Clip
        {
            get { return clip; }
            set
            {
                clip = value;

                if (AudioSource != null)
                {
                    AudioSource.clip = clip;
                }
            }
        }

        /// <summary>
        /// ������� �ݺ����� ����
        /// <para>�� �����ϸ�, �ڵ����� AudioSource�� loop�� ����˴ϴ�.</para>
        /// </summary>
        public bool Loop
        {
            get { return loop; }
            set
            {
                loop = value;

                if (AudioSource != null)
                {
                    AudioSource.loop = loop;
                }
            }
        }

        /// <summary>
        /// ������� ���ҰŵǾ� �ִ��� ����
        /// </summary>
        public bool Mute
        {
            get { return mute; }
            set
            {
                mute = value;

                if (AudioSource != null)
                {
                    AudioSource.mute = mute;
                }
            }
        }

        /// <summary>
        /// ������� �켱������ �����մϴ�.
        /// </summary>
        /// <value>0 ~ 256</value>
        public int Priority
        {
            get { return priority; }
            set
            {
                priority = Mathf.Clamp(value, 0, 256);

                if (AudioSource != null)
                {
                    AudioSource.priority = priority;
                }
            }
        }

        /// <summary>
        /// ������� ��ġ(�Ҹ��� ������)�� �����մϴ�.
        /// <para>��ġ�� �� �����ϸ�, �⺻���� 1.0�Դϴ�. ���� 0.5�̸� ������ ��������, 2.0�̸� �� �� �������ϴ�.</para>
        /// </summary>
        /// <value>-3.0(������) ~ 3.0(������)</value>
        public float Pitch
        {
            get { return pitch; }
            set
            {
                pitch = Mathf.Clamp(value, -3.0f, 3.0f);

                if (AudioSource != null)
                {
                    AudioSource.pitch = pitch;
                }
            }
        }

        /// <summary>
        /// ��� ���� ������� �¿� ä��(L/R) �������� �����մϴ�.
        /// <para>Stereo ä�ο��� ����˴ϴ�. (SpatialBlend�� 1�� �����Ҽ���)</para>
        /// </summary>
        /// <value>-1.0(����) ~ 1.0(������)</value>
        public float StereoPan
        {
            get { return stereoPan; }
            set
            {
                stereoPan = Mathf.Clamp(value, -1.0f, 1.0f);

                if (AudioSource != null)
                {
                    AudioSource.panStereo = stereoPan;
                }
            }
        }

        /// <summary>
        /// ��� ���� ������� Mono�� Stereo ���� ������ �����մϴ�.
        /// </summary>
        /// <value>0.0(2D, Mono) ~ 1.0(3D, Stereo)</value>
        public float SpatialBlend
        {
            get { return spatialBlend; }
            set
            {
                spatialBlend = Mathf.Clamp01(value);

                if (AudioSource != null)
                {
                    AudioSource.spatialBlend = spatialBlend;
                }
            }
        }

        /// <summary>
        /// ������� �󸶳� ������ �ݻ�Ǹ鼭 �︱��(������ ȿ��)�� ������ �� �ֽ��ϴ�.
        /// </summary>
        /// <value>
        /// 0.0(������ ����) ~ 2.0(���� ������)
        /// <para>�⺻: 1.0</para>
        /// </value>
        public float ReverbZoneMix
        {
            get { return reverbZoneMix; }
            set
            {
                reverbZoneMix = Mathf.Clamp(value, 0, 2.0f);

                if (AudioSource != null)
                {
                    AudioSource.reverbZoneMix = reverbZoneMix;
                }
            }
        }

        #region 3D ����
        /// <summary>
        /// ������� ���÷� ȿ���� ������ �� �ֽ��ϴ�.
        /// <para>���÷� ȿ��: �Ҹ��� �̵� �ӵ��� ���� ��ġ�� ���ϴ� ȿ��</para>
        /// </summary>
        /// <value>0.0(�ӵ��� ���� ��ġ ��ȭ ����) ~ 5.0(�ӵ��� ���� ��ġ ��ȭ �ش�ȭ)</value>
        public float DopplerLevel
        {
            get { return dopplerLevel; }
            set
            {
                dopplerLevel = Mathf.Clamp(value, 0, 5.0f);

                if (AudioSource != null)
                {
                    AudioSource.dopplerLevel = dopplerLevel;
                }
            }
        }

        /// <summary>
        /// ������� �󸶳� �а� ������ ������ �� �ֽ��ϴ�.
        /// <para>�� Stereo ä�ο����� �����Ͽ� �Ҹ��� ����� ��ü���� �����մϴ�.</para>
        /// </summary>
        /// <value>
        /// <para>0: �Ҹ��� ����(�� or ��)������ �鸲</para>
        /// <para>180: �Ҹ��� ������ ���� ��ο��� �鸲</para>
        /// <para>360: �Ҹ��� ������������ ������ �������� �Ҹ��� ������ �� ó�� �鸲</para>
        /// </value>
        public float Spread
        {
            get { return spread; }
            set
            {
                spread = Mathf.Clamp(value, 0, 360.0f);

                if (AudioSource != null)
                {
                    AudioSource.spread = spread;
                }
            }
        }

        /// <summary>
        /// ����� �ҽ��� �������� �Ÿ��� ���� �Ҹ��� ���ҵǴ� ����� ������ �� �ֽ��ϴ�.
        /// </summary>
        public AudioRolloffMode RolloffMode
        {
            get { return rolloffMode; }
            set
            {
                rolloffMode = value;

                if (AudioSource != null)
                {
                    AudioSource.rolloffMode = rolloffMode;
                }
            }
        }

        /// <summary>
        /// �Ҹ��� �ִ� �Ÿ�, �� �Ÿ��� �ʰ��ϸ� �Ҹ��� �鸮�� �ʽ��ϴ�.
        /// </summary>
        public float Max3DDistance
        {
            get { return max3DDistance; }
            set
            {
                max3DDistance = Mathf.Max(value, 0.01f);

                if (AudioSource != null)
                {
                    AudioSource.maxDistance = max3DDistance;
                }
            }
        }

        /// <summary>
        /// �Ҹ��� ��Ȯ�ϰ� �鸮�� �ִ� �Ÿ�
        /// <para>�� �� �Ÿ��� �ʰ��ϸ� �Ҹ��� ������ �Ÿ��� �Բ� ����</para>
        /// </summary>
        public float Min3DDistance
        {
            get { return min3DDistance; }
            set
            {
                min3DDistance = Mathf.Max(value, 0);

                if (AudioSource != null)
                {
                    AudioSource.minDistance = min3DDistance;
                }
            }
        }
        #endregion
        #endregion

        private static int audioCounter = 0;

        private float targetVolume;
        private float initTargetVolume;
        private float fadeInterpolater;
        private float onFadeStartVolume;
        private Transform sourceTransform;

        public Audio(AudioType audioType, AudioClip clip, bool loop, bool persist, float volume, float fadeInValue, float fadeOutValue, Transform sourceTransform)
        {
            AudioID = audioCounter;
            audioCounter++;

            this.Type = audioType;
            this.Clip = clip;
            this.SourceTransform = sourceTransform;
            this.Loop = loop;
            this.Persist = persist;
            this.targetVolume = volume;
            this.initTargetVolume = volume;
            this.FadeInSeconds = fadeInValue;
            this.FadeOutSeconds = fadeOutValue;

            Mute = false;
            Volume = 0f;
            Priority = 128;
            Pitch = 1;
            StereoPan = 0;
            if (sourceTransform != null && sourceTransform != SoundManager.Transform)
            {
                SpatialBlend = 1;
            }
            ReverbZoneMix = 1;

            DopplerLevel = 1;
            Spread = 0;
            RolloffMode = AudioRolloffMode.Logarithmic;
            Min3DDistance = 1;
            Max3DDistance = 500;

            IsPlaying = false;
            IsPaused = false;
            IsUpdated = false;
            IsPooled = false;
        }

        #region ����� ���
        /// <summary>
        /// ����� Ŭ���� ó������ ����մϴ�.
        /// </summary>
        public void Play()
        {
            Play(initTargetVolume);
        }

        /// <summary>
        /// ����� Ŭ���� ó������ ����մϴ�.
        /// </summary>
        /// <param name="volume">�ش� ������� ������ ���� ������ �� �ֽ��ϴ�.</param>
        public void Play(float volume)
        {
            if (IsPooled)
            {
                bool restoredFromPool = SoundManager.RestoreAudioFromPool(Type, AudioID);

                if (!restoredFromPool) return;

                IsPooled = false;
            }

            if (AudioSource == null)
            {
                CreateAudiosource();
            }

            AudioSource.Play();
            IsPlaying = true;

            fadeInterpolater = 0f;
            onFadeStartVolume = this.Volume;
            targetVolume = volume;
        }

        private void CreateAudiosource()
        {
            AudioSource = SourceTransform.gameObject.AddComponent<AudioSource>();
            AudioSource.clip = Clip;
            AudioSource.loop = Loop;
            AudioSource.mute = Mute;
            AudioSource.volume = Volume;
            AudioSource.priority = Priority;
            AudioSource.pitch = Pitch;
            AudioSource.panStereo = StereoPan;
            AudioSource.spatialBlend = SpatialBlend;
            AudioSource.reverbZoneMix = ReverbZoneMix;
            AudioSource.dopplerLevel = DopplerLevel;
            AudioSource.spread = Spread;
            AudioSource.rolloffMode = RolloffMode;
            AudioSource.maxDistance = Max3DDistance;
            AudioSource.minDistance = Min3DDistance;
        }
        #endregion

        #region ����� ����/�Ͻ� ����/�ٽ� ����
        public void Stop()
        {
            fadeInterpolater = 0f;
            onFadeStartVolume = Volume;
            targetVolume = 0f;

            IsStopped = true;
        }

        public void Pause()
        {
            AudioSource.Pause();
            IsPaused = true;
        }

        public void Resume()
        {
            AudioSource.UnPause();
            IsPaused = false;
        }
        #endregion

        public void Update()
        {
            if (AudioSource == null) return;

            IsUpdated = true;

            // ���� ������ ��ǥ ������ �ٸ��ٸ�, ���� ������ ��ǥ �������� ���̰ų� ����ϴ�.
            if (Volume != targetVolume)
            {
                float fadeValue;
                fadeInterpolater += Time.unscaledDeltaTime;

                if (Volume > targetVolume) fadeValue = FadeOutSeconds;
                else fadeValue = FadeInSeconds;

                Volume = Mathf.Lerp(onFadeStartVolume, targetVolume, fadeInterpolater / fadeValue);
            }

            // ������ �����մϴ�.
            switch (Type)
            {
                case AudioType.BGM:
                    {
                        AudioSource.volume = Volume * SoundManager.GlobalBGMVolume * SoundManager.GlobalVolume;
                        break;
                    }
                case AudioType.SFX:
                    {
                        AudioSource.volume = Volume * SoundManager.GlobalSFXVolume * SoundManager.GlobalVolume;
                        break;
                    }
                case AudioType.UI:
                    {
                        AudioSource.volume = Volume * SoundManager.GlobalUIVolume * SoundManager.GlobalVolume;
                        break;
                    }
                case AudioType.Voice:
                    {
                        AudioSource.volume = Volume * SoundManager.GlobalVoiceVolume * SoundManager.GlobalVolume;
                        break;
                    }
            }

            // ������� ������ ������ŵ�ϴ�.
            if (Volume == 0f && IsStopped)
            {
                AudioSource.Stop();
                IsPlaying = false;
                IsStopped = false;
                IsPaused = false;
            }

            // ���ø����̼��� Ȱ��ȭ �Ǿ��� ��, ������� ����ǰ� �ִ��� Ȯ���մϴ�.
            if (AudioSource.isPlaying != IsPlaying && Application.isFocused)
            {
                IsPlaying = AudioSource.isPlaying;
            }
        }
    }
}