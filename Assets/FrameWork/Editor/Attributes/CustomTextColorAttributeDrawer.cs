using UnityEditor;
using UnityEngine;

namespace FrameWork.Editor
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TextColorUsageAttribute))]
    public class TextColorUsageAttributeDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            TextColorUsageAttribute textColorUsageAttribute = (TextColorUsageAttribute)attribute;

            EditorGUI.BeginProperty(position, label, property);

            // ���ϴ� ��ġ�� ���� �̵�
            Rect labelPosition = new Rect(position.x, position.y, 168f, position.height);

            // ���� ���� �����Ͽ� ������
            Color originalContentColor = GUI.contentColor;
            GUI.contentColor = textColorUsageAttribute.color;
            EditorGUI.LabelField(labelPosition, label);
            GUI.contentColor = originalContentColor;

            // ������Ƽ ������ �� ���ķ� �̵�
            Rect propertyPosition = new Rect(position.x + 168f, position.y, position.width - 168f, position.height);

            // ������Ƽ�� �׸�
            EditorGUI.PropertyField(propertyPosition, property, GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
#endif
}