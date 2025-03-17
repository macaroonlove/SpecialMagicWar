using UnityEngine;
using UnityEngine.EventSystems;

namespace FrameWork.Tooltip
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TooltipStyle _tooltipStyle;
        [SerializeField] private TipPosition _tooltipPosition;
        [SerializeField] private Vector2 _tooltipOffset;

        [HideInInspector] public TooltipData tooltipData;

        internal TooltipStyle tooltipStyle => _tooltipStyle;
        internal TipPosition tooltipPosition => _tooltipPosition;
        internal Vector2 tooltipOffset => _tooltipOffset;

        private void Awake()
        {
            tooltipData.InitializeData();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            StartHover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopHover();
        }

        private void StartHover()
        {
            TooltipManager.Instance.Show(this);
        }

        private void StopHover()
        {
            TooltipManager.Instance.Hide(this);
        }

        public void SetText(string parameterName, string text)
        {
            if (tooltipData == null)
            {
#if UNITY_EDITOR
                Debug.LogError("TooltipData�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
#endif
                return;
            }

            tooltipData.SetString(parameterName, text);
            TooltipManager.Instance.ReShow(this);
        }
    }
}

#if UNITY_EDITOR
namespace FrameWork.Tooltip.Editor
{
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(TooltipTrigger))]
    public class TooltipTriggerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            TooltipTrigger trigger = (TooltipTrigger)target;

            base.OnInspectorGUI();

            EditorGUILayout.Space(10);

            if (trigger.tooltipStyle != null)
            {
                if (trigger.tooltipData.IsInitialize() == false)
                {
                    trigger.tooltipData = trigger.tooltipStyle.CreateField();
                }

                if (trigger.tooltipData.IsInitializeData() == false)
                {
                    trigger.tooltipData.InitializeData();
                }

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("���� ������ ����", EditorStyles.boldLabel);

                // ���ڿ� ������ ó��
                var stringData = trigger.tooltipData.getAllString;
                foreach (var key in stringData.Keys.ToList())
                {
                    EditorGUILayout.LabelField(key);
                    string newValue = EditorGUILayout.TextArea(
                        stringData[key],
                        GUILayout.Height(50),
                        GUILayout.ExpandHeight(true)
                    );
                    if (newValue != stringData[key])
                    {
                        trigger.tooltipData.SetString(key, newValue);
                        GUI.changed = true;
                    }
                }

                // ��������Ʈ ������ ó��
                var spriteData = trigger.tooltipData.getAllSprite;
                foreach (var key in spriteData.Keys.ToList())
                {
                    Sprite newValue = (Sprite)EditorGUILayout.ObjectField(key, spriteData[key], typeof(Sprite), false);
                    if (newValue != spriteData[key])
                    {
                        trigger.tooltipData.SetSprite(key, newValue);
                        GUI.changed = true;
                    }
                }

                EditorGUILayout.EndVertical();

                if (GUI.changed)
                {
                    trigger.tooltipData.InitializeData();
                    EditorUtility.SetDirty(trigger);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("���� ��Ÿ���� ������ּ���.", MessageType.Info);
            }
        }
    }
}
#endif