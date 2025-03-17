using DG.Tweening;
using FrameWork.Sound;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FrameWork.VisualNovel
{
    public enum ChapterState
    {
        None,
        Run,
    }

    public class CommandExecutor : Singleton<CommandExecutor>
    {
        private int _currentIndex = 0;
        private Command _currentCommand;
        private Coroutine _currentCommandCorutine;

        internal ChapterState chapterState = ChapterState.None;

        private List<Command> _commandList = new List<Command>();

        private UnityAction _onEndChapter;

        /// <summary>
        /// é�� ����
        /// </summary>
        internal void StartChapter(List<Command> commandList, UnityAction onEndChapter = null)
        {
            // é�Ͱ� ���� ���̶��
            if (chapterState == ChapterState.Run) return;

            _commandList = commandList;
            _currentIndex = 0;
            RegistCommand();

            _onEndChapter = onEndChapter;
        }

        /// <summary>
        /// ���� Ŀ�ǵ� ����
        /// </summary>
        internal void Next()
        {
            if (_currentIndex < 0) return;

            // ���� Ŀ�ǵ尡 �����ٸ�
            if (_currentCommand.isComplete)
            {
                RegistCommand();
            }
            // ���� Ŀ�ǵ尡 ������ �ʾҴٸ�
            else
            {
                if (_currentCommandCorutine != null)
                {
                    StopCoroutine(_currentCommandCorutine);
                    _currentCommandCorutine = null;
                }

                _currentCommand?.ForceExecute();
            }
        }

        private void RegistCommand()
        {
            // ���� é�Ͱ� �����ٸ�
            if (_commandList.Count <= _currentIndex)
            {
                _currentIndex = -1;
                ChapterEnd();
                return;
            }

            _currentCommand = _commandList[_currentIndex];
            _currentCommandCorutine = StartCoroutine(_currentCommand.Execute());
            _currentIndex++;
        }

        /// <summary>
        /// ���� é�� ��ŵ
        /// </summary>
        internal void Skip()
        {
            CommandEnd();
            ChapterEnd();
        }

        /// <summary>
        /// ���� é�͸� �����ϰ�, ���ϴ� é�ͷ� �̵�
        /// </summary>
        internal void JumpChapter(ChapterTemplate chapterTemplate)
        {
            CommandEnd();
            chapterState = ChapterState.None;
            _onEndChapter = null;

            VisualNovelManager.instance.Load(chapterTemplate);
        }

        private void CommandEnd()
        {
            if (_currentCommand.isComplete == false)
            {
                if (_currentCommandCorutine != null)
                {
                    StopCoroutine(_currentCommandCorutine);
                    _currentCommandCorutine = null;
                }

                _currentCommand.isComplete = true;
            }

            SpeakEnd();
            SCGHide(-1);
            BGMStop();
            SFXStop();
        }

        private void ChapterEnd()
        {
            chapterState = ChapterState.None;
            _onEndChapter?.Invoke();
            _onEndChapter = null;
        }

        #region Speak
        internal event UnityAction<string, string, UnityAction, bool> speakStart;
        internal event UnityAction speakEnd;

        internal void Speak(string speaker, string content, UnityAction onComplete, bool isForce = false)
        {
            speakStart?.Invoke(speaker, content, onComplete, isForce);
        }

        internal void SpeakEnd()
        {
            speakEnd?.Invoke();
        }
        #endregion

        #region SCG
        internal event UnityAction<int, string, Rect, Vector2> scgShow;
        internal event UnityAction<int> scgHide;
        internal event Action<int, Rect, Vector2, float, int, Ease, bool, Ease> scgMove;

        internal void SCGShow(int id, string theme, Rect position, Vector2 anchor)
        {
            scgShow?.Invoke(id, theme, position, anchor);
        }

        internal void SCGHide(int id)
        {
            scgHide?.Invoke(id);
        }

        internal void SCGMove(int id, Rect position, Vector2 anchor, float duration, int loopCount, Ease ease, bool isReturn, Ease returnEase)
        {
            scgMove?.Invoke(id, position, anchor, duration, loopCount, ease, isReturn, returnEase);
        }
        #endregion

        #region ECG
        internal event UnityAction<string> ecgShow;
        internal event UnityAction ecgHide;

        internal void ECGShow(string theme)
        {
            ecgShow?.Invoke(theme);
        }

        internal void ECGHide()
        {
            ecgHide?.Invoke();
        }
        #endregion

        #region BGM
        internal void BGMPlay(string audio)
        {
            AddressableAssetManager.Instance.GetAudioClip(audio, (audioClip) =>
            {
                SoundManager.PlayBGM(audioClip);
            });
        }

        internal void BGMStop()
        {
            SoundManager.StopAllBGM();
        }
        #endregion

        #region SFX
        internal void SFXPlay(string audio)
        {
            AddressableAssetManager.Instance.GetAudioClip(audio, (audioClip) =>
            {
                SoundManager.PlaySFX(audioClip);
            });
        }

        internal void SFXStop()
        {
            SoundManager.StopAllSFX();
        }
        #endregion

        #region Choice
        internal event UnityAction<List<KeyValuePair<string, ChapterTemplate>>, UnityAction> choice;

        internal void Choice(List<KeyValuePair<string, ChapterTemplate>> choiceList, UnityAction onComplete)
        {
            choice?.Invoke(choiceList, onComplete);
        }
        #endregion

        #region Get
        public event UnityAction<ItemType, int, string> getItem;

        internal void Get(ItemType itemType, int amount, string name)
        {
            getItem?.Invoke(itemType, amount, name);
        }
        #endregion

        #region Utility
        internal event UnityAction<bool> onNextButtonInteractableChanged;

        internal void SetNextButtonInteractable(bool isOn)
        {
            onNextButtonInteractableChanged?.Invoke(isOn);
        }
        #endregion
    }
}