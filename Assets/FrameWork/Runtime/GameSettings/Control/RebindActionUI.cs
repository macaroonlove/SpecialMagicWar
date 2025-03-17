using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;

namespace FrameWork.GameSettings
{
    /// <summary>
    /// ���� �׼��� �����ε��ϱ� ���� �������� UI�� ���� ���� ������ ������Ʈ�Դϴ�.
    /// </summary>
    public class RebindActionUI : MonoBehaviour
    {
        #region ������Ƽ
        /// <summary>
        /// �����ε��� �׼ǿ� ���� ����
        /// </summary>
        public InputActionReference actionReference
        {
            get => m_Action;
            set
            {
                m_Action = value;
                UpdateActionLabel();
                UpdateBindingDisplay();
            }
        }

        /// <summary>
        /// �׼ǿ��� �����ε��� ���ε��� ID(���ڿ� ����)
        /// </summary>
        public string bindingId
        {
            get => m_BindingId;
            set
            {
                m_BindingId = value;
                UpdateBindingDisplay();
            }
        }

        public InputBinding.DisplayStringOptions displayStringOptions
        {
            get => m_DisplayStringOptions;
            set
            {
                m_DisplayStringOptions = value;
                UpdateBindingDisplay();
            }
        }

        /// <summary>
        /// �׼��� �̸��� ǥ���ϴ� �ؽ�Ʈ ������Ʈ
        /// </summary>
        public TextMeshProUGUI actionLabel
        {
            get => m_ActionLabel;
            set
            {
                m_ActionLabel = value;
                UpdateActionLabel();
            }
        }

        /// <summary>
        /// ������ path�� ǥ���ϴ� �ؽ�Ʈ ������Ʈ
        /// </summary>
        public TextMeshProUGUI pathLabel
        {
            get => m_PathLabel;
            set
            {
                m_PathLabel = value;
                UpdateBindingDisplay();
            }
        }

        /// <summary>
        /// �����ε� �гο� ������ ���õ� ������ �̸��� ǥ���ϴ� �ؽ�Ʈ ������Ʈ
        /// </summary>
        public TextMeshProUGUI rebindActionLabel
        {
            get => m_RebindActionLabel;
            set => m_RebindActionLabel = value;
        }

        /// <summary>
        /// �����ε� �гο� ������ ���õ� ������ path�� ǥ���ϴ� �ؽ�Ʈ ������Ʈ
        /// </summary>
        public TextMeshProUGUI rebindPathLabel
        {
            get => m_RebindPathLabel;
            set => m_RebindPathLabel = value;
        }

        /// <summary>
        /// �����ε��� �������� �� ǥ�õ� UI �г�
        /// </summary>
        public CanvasGroupController rebindOverlay
        {
            get => m_RebindOverlay;
            set => m_RebindOverlay = value;
        }

        /// <summary>
        /// UI�� ���� ���ε��� �ݿ��ϵ��� ������Ʈ�� ������ Ʈ���ŵǴ� �̺�Ʈ�Դϴ�. ����� ���� �ð�ȭ�� ���ε��� �����ϴ� �� ����� �� �ֽ��ϴ�.
        /// </summary>
        public UpdateBindingUIEvent updateBindingUIEvent
        {
            get
            {
                if (m_UpdateBindingUIEvent == null)
                    m_UpdateBindingUIEvent = new UpdateBindingUIEvent();
                return m_UpdateBindingUIEvent;
            }
        }

        /// <summary>
        /// �׼ǿ��� ���ͷ�Ƽ�� �����ε��� ���۵� �� Ʈ���ŵǴ� �̺�Ʈ�Դϴ�.
        /// </summary>
        public InteractiveRebindEvent startRebindEvent
        {
            get
            {
                if (m_RebindStartEvent == null)
                    m_RebindStartEvent = new InteractiveRebindEvent();
                return m_RebindStartEvent;
            }
        }

        /// <summary>
        /// ���ͷ�Ƽ�� �����ε��� �Ϸ�ǰų� ��ҵ� �� Ʈ���ŵǴ� �̺�Ʈ�Դϴ�.
        /// </summary>
        public InteractiveRebindEvent stopRebindEvent
        {
            get
            {
                if (m_RebindStopEvent == null)
                    m_RebindStopEvent = new InteractiveRebindEvent();
                return m_RebindStopEvent;
            }
        }

        /// <summary>
        /// ���ͷ�Ƽ�� �����ε��� ���� ���� ��, �̴� �����ε� �۾� ��Ʈ�ѷ��Դϴ�. �׷��� ������ <c>null</c>�Դϴ�.
        /// </summary>
        public InputActionRebindingExtensions.RebindingOperation ongoingRebind => m_RebindOperation;
        #endregion

        /// <summary>
        /// ������Ʈ�� ������� �ϴ� ���ε��� �׼ǰ� ���ε� �ε����� ��ȯ�մϴ�.
        /// </summary>
        public bool ResolveActionAndBinding(out InputAction action, out int bindingIndex)
        {
            bindingIndex = -1;

            action = m_Action?.action;
            if (action == null)
                return false;

            if (string.IsNullOrEmpty(m_BindingId))
                return false;

            // ���ε� �ε��� ��ȸ
            var bindingId = new Guid(m_BindingId);
            bindingIndex = action.bindings.IndexOf(x => x.id == bindingId);
            if (bindingIndex == -1)
            {
                Debug.LogError($"ID '{bindingId}'�� ���� ���ε��� '{action}'���� ã�� �� �����ϴ�.", this);
                return false;
            }

            return true;
        }

        /// <summary>
        /// ���� ǥ�õ� ���ε��� ���� ��Ĩ�ϴ�.
        /// </summary>
        public void UpdateBindingDisplay()
        {
            var displayString = string.Empty;
            var deviceLayoutName = default(string);
            var controlPath = default(string);

            // �׼ǿ��� ǥ�� ���ڿ� ��������
            var action = m_Action?.action;
            if (action != null)
            {
                var bindingIndex = action.bindings.IndexOf(x => x.id.ToString() == m_BindingId);
                if (bindingIndex != -1)
                    displayString = action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath, displayStringOptions);
            }

            // ���̺� ���� (�ִٸ�)
            if (m_PathLabel != null)
                m_PathLabel.text = displayString;

            // �����ʿ��� UI�� ������ ��ȸ ����
            m_UpdateBindingUIEvent?.Invoke(this, displayString, deviceLayoutName, controlPath);
        }

        /// <summary>
        /// ���� ����� ���ε� �������̵带 �����մϴ�.
        /// </summary>
        public void ResetToDefault()
        {
            if (!ResolveActionAndBinding(out var action, out var bindingIndex))
                return;

            if (action.bindings[bindingIndex].isComposite)
            {
                for (var i = bindingIndex + 1; i < action.bindings.Count && action.bindings[i].isPartOfComposite; ++i)
                    action.RemoveBindingOverride(i);
            }
            else
            {
                action.RemoveBindingOverride(bindingIndex);
            }
            UpdateBindingDisplay();
        }

        /// <summary>
        /// �÷��̾ ��� Ȱ��ȭ�Ͽ� �׼ǿ� ���� �� ���ε��� ������ �� �ֵ��� �ϴ� ���ͷ�Ƽ�� �����ε��� �����մϴ�.
        /// </summary>
        public void StartInteractiveRebind()
        {
            if (!ResolveActionAndBinding(out var action, out var bindingIndex))
                return;

            // ���ε��� ������Ʈ�� ��� �� �κ��� ���ʷ� ����ε��ؾ� �մϴ�.
            if (action.bindings[bindingIndex].isComposite)
            {
                var firstPartIndex = bindingIndex + 1;
                if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
                    PerformInteractiveRebind(action, firstPartIndex, allCompositeParts: true);
            }
            else
            {
                PerformInteractiveRebind(action, bindingIndex);
            }
        }

        private void PerformInteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts = false)
        {
            m_RebindOperation?.Cancel();

            void CleanUp()
            {
                m_RebindOperation?.Dispose();
                m_RebindOperation = null;
            }

            // ��� ���� �׼� ��Ȱ��ȭ
            action.Disable();
            // �����ε� ����
            // WithControlsExcluding: �ش� path�δ� ���� �Ұ�
            // WithCancelingThrough: �ش� path�� �ѱ��
            m_RebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                .WithControlsExcluding("<Mouse>/leftButton")
                .WithControlsExcluding("<Mouse>/rightButton")
                .WithControlsExcluding("<Mouse>/middleButton")
                .WithControlsExcluding("<Mouse>/press")
                .WithControlsExcluding("<Mouse>/scroll")
                .WithCancelingThrough("<Keyboard>/escape")
                .OnCancel(
                    operation =>
                    {
                        action.Enable();
                        m_RebindStopEvent?.Invoke(this, operation);
                        m_RebindOverlay?.Hide(true);
                        UpdateBindingDisplay();
                        CleanUp();
                    })
                .OnComplete(
                    operation =>
                    {
                        action.Enable();
                        m_RebindOverlay?.Hide(true);
                        m_RebindStopEvent?.Invoke(this, operation);

                        if (CheckDuplicateBindings(action, bindingIndex, allCompositeParts))
                        {
                            action.RemoveBindingOverride(bindingIndex);
                            CleanUp();
                            PerformInteractiveRebind(action, bindingIndex, allCompositeParts);
                            return;
                        }

                        UpdateBindingDisplay();
                        CleanUp();

                        // �� ���� ������Ʈ �κ��� �ִٸ�, ���� �κп� ���� �����ε��� �����մϴ�.
                        if (allCompositeParts)
                        {
                            var nextBindingIndex = bindingIndex + 1;
                            if (nextBindingIndex < action.bindings.Count && action.bindings[nextBindingIndex].isPartOfComposite)
                                PerformInteractiveRebind(action, nextBindingIndex, true);
                        }
                    });

            // ���ε��� �κ� ���ε��� ���, UI�� �κ� �̸��� ǥ���մϴ�.
            string act = action.bindings[bindingIndex].action;
            var currentLanguage = LocalizationSettings.SelectedLocale;
            string partName = LocalizationSettings.StringDatabase.GetLocalizedString("SettingTable", act, currentLanguage);

            string path = action.GetBindingDisplayString(bindingIndex, displayStringOptions);

            m_RebindOverlay?.Show(true);
            if (m_RebindActionLabel != null)
            {
                m_RebindActionLabel.text = partName;
            }
            if (m_RebindPathLabel != null)
            {
                m_RebindPathLabel.text = path;
            }

            // �����ε� �г��� ���� ���, path ���� "<��ٸ��� ��...>"���� �����մϴ�.
            if (m_RebindOverlay == null && m_PathLabel != null)
                m_PathLabel.text = "<��ٸ��� ��...>";

            // �����ʿ��� �����ε� ���ۿ� ���� ��ġ�� ���� ��ȸ�� �����մϴ�.
            m_RebindStartEvent?.Invoke(this, m_RebindOperation);

            m_RebindOperation.Start();
        }

        private bool CheckDuplicateBindings(InputAction action, int bindingIndex, bool allCompositeParts = false)
        {
            InputBinding newBinding = action.bindings[bindingIndex];
            for (int i = 0; i < action.actionMap.bindings.Count; i++)
            {
                if (action.actionMap.bindings[i].action.Equals(newBinding.action))
                {
                    if (newBinding.action != "Move")
                        continue;
                    else
                    {
                        if (action.actionMap.bindings[i].name.Equals(newBinding.name)) continue;
                    }
                }
                if (action.actionMap.bindings[i].effectivePath.Equals(newBinding.effectivePath))
                {
                    return true;
                }
            }
            if (allCompositeParts)
            {
                for (int i = 1; i < bindingIndex; i++)
                {
                    if (action.bindings[i].effectivePath.Equals(newBinding.effectivePath))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected void OnEnable()
        {
            if (s_RebindActionUIs == null)
                s_RebindActionUIs = new List<RebindActionUI>();
            s_RebindActionUIs.Add(this);
            if (s_RebindActionUIs.Count == 1)
                InputSystem.onActionChange += OnActionChange;
        }

        protected void OnDisable()
        {
            m_RebindOperation?.Dispose();
            m_RebindOperation = null;

            s_RebindActionUIs.Remove(this);
            if (s_RebindActionUIs.Count == 0)
            {
                s_RebindActionUIs = null;
                InputSystem.onActionChange -= OnActionChange;
            }
        }

        // �׼� �ý����� ���ε��� �ٽ� �ذ��� ��, UI�� ������Ʈ�մϴ�.
        // �츮�� �� ���� ���׿��� �߻��ϴ� Ʈ���ŻӸ� �ƴ϶� �ٸ� ������ �� ���� ���׿��� �����մϴ�.
        // ���� ���, ����ڰ� Ű���� ���̾ƿ��� �����ϸ� �ٿ�� ��Ʈ�� ���� �˸��� �ް� ���� Ű���� ���̾ƿ��� �ݿ��ϵ��� UI�� ������Ʈ�մϴ�.
        private static void OnActionChange(object obj, InputActionChange change)
        {
            if (change != InputActionChange.BoundControlsChanged)
                return;

            var action = obj as InputAction;
            var actionMap = action?.actionMap ?? obj as InputActionMap;
            var actionAsset = actionMap?.asset ?? obj as InputActionAsset;

            for (var i = 0; i < s_RebindActionUIs.Count; ++i)
            {
                var component = s_RebindActionUIs[i];
                var referencedAction = component.actionReference?.action;
                if (referencedAction == null)
                    continue;

                if (referencedAction == action ||
                    referencedAction.actionMap == actionMap ||
                    referencedAction.actionMap?.asset == actionAsset)
                    component.UpdateBindingDisplay();
            }
        }

        [Tooltip("UI���� ����ε��� �׼ǿ� ���� �����Դϴ�.")]
        [SerializeField]
        private InputActionReference m_Action;

        [SerializeField]
        private string m_BindingId;

        [SerializeField]
        private InputBinding.DisplayStringOptions m_DisplayStringOptions;

        [Tooltip("���� ���ε��� Action�� ǥ�� �̸�")]
        [SerializeField]
        private TextMeshProUGUI m_ActionLabel;

        [Tooltip("���� ���ε��� Action�� Path")]
        [SerializeField]
        private TextMeshProUGUI m_PathLabel;

        [Tooltip("�����ε��� ���� ���� �� ǥ�õ� UI �г�")]
        [SerializeField]
        private CanvasGroupController m_RebindOverlay;

        [Tooltip("�����ε� �гο� ������ ���õ� Action�� ǥ�� �̸�")]
        [SerializeField]
        private TextMeshProUGUI m_RebindActionLabel;
        
        [Tooltip("�����ε� �гο� ������ ���õ� Action�� path")]
        [SerializeField]
        private TextMeshProUGUI m_RebindPathLabel;

        [Tooltip("���ε� ǥ�� ����� ������Ʈ�� �� Ʈ���ŵǴ� �̺�Ʈ")]
        [SerializeField]
        private UpdateBindingUIEvent m_UpdateBindingUIEvent;

        [Tooltip("�����ε��� ���۵� �� Ʈ���ŵǴ� �̺�Ʈ")]
        [SerializeField]
        private InteractiveRebindEvent m_RebindStartEvent;

        [Tooltip("�����ε��� �Ϸ�ǰų� ��ҵ� �� Ʈ���ŵǴ� �̺�Ʈ")]
        [SerializeField]
        private InteractiveRebindEvent m_RebindStopEvent;

        private InputActionRebindingExtensions.RebindingOperation m_RebindOperation;

        private static List<RebindActionUI> s_RebindActionUIs;

        // �����Ϳ����� �׼� �̸� ���̺��� ������Ʈ
#if UNITY_EDITOR
        protected void OnValidate()
        {
            UpdateActionLabel();
            UpdateBindingDisplay();
        }

#endif

        private void UpdateActionLabel()
        {
            if (m_ActionLabel != null)
            {
                var action = m_Action?.action;

                if (action != null)
                {
                    switch (action.name)
                    {
                        case "Move":
                            m_ActionLabel.text = "�̵�(�����Ͽ� ������ �������ּ���)";
                            break;
                        case "Sprint":
                            m_ActionLabel.text = "�޸���";
                            break;
                        case "Jump":
                            m_ActionLabel.text = "����/�Ŵ޸���";
                            break;
                        case "Crouch":
                            m_ActionLabel.text = "�ɱ�";
                            break;
                        case "Interaction":
                            m_ActionLabel.text = "��ȣ�ۿ�";
                            break;
                        default:
                            m_ActionLabel.text = string.Empty;
                            break;
                    }
                }
            }
        }

        [Serializable]
        public class UpdateBindingUIEvent : UnityEvent<RebindActionUI, string, string, string>
        {
        }

        [Serializable]
        public class InteractiveRebindEvent : UnityEvent<RebindActionUI, InputActionRebindingExtensions.RebindingOperation>
        {
        }
    }
}
