using UnityEditor;
using UnityEngine;

namespace FrameWork.Editor
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(HeaderColorAttribute))]
    public class HeaderColorAttributeDrawer : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {
            HeaderColorAttribute headerColorAttribute = (HeaderColorAttribute)attribute;

            // ���� �� ĳ��
            Color originalColor = GUI.contentColor;

            // ���ϴ� ������ �ٲٱ�
            GUI.contentColor = headerColorAttribute.color;

            // y�� ��ġ�� +5
            position.y += 5;

            // ���� �۾�ü�� �� �ʵ� �׷��ֱ�
            EditorGUI.LabelField(position, headerColorAttribute.header, EditorStyles.boldLabel);

            // ���� �ʵ���� �ٽ� ���� ���� �ֱ�
            GUI.contentColor = originalColor;
        }

        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight + 10f;
        }
    }
#endif
}