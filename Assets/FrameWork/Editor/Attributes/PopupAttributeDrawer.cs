using UnityEditor;
using UnityEngine;

namespace FrameWork.Editor
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(PopupAttribute))]
    public class PopupAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            PopupAttribute popupAttribute = (PopupAttribute)attribute;

            // Ÿ�Կ� ���� �ٸ��� ó��
            if (property.propertyType == SerializedPropertyType.String
                || property.propertyType == SerializedPropertyType.Integer
                || property.propertyType == SerializedPropertyType.Float)
            {
                // ���� ���� �ε��� ã��
                int selectedIndex = Mathf.Max(0, System.Array.IndexOf(popupAttribute.options, GetPropertyValue(property)));

                // �˾� �׸���
                selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, GetOptionsAsStrings(popupAttribute.options));

                // ���õ� �ε����� �ش��ϴ� ������ ����
                SetPropertyValue(property, popupAttribute.options[selectedIndex]);
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "�������� ���� �˾� ����");
            }
        }

        // SerializedProperty�� ���� �������� �޼���
        private object GetPropertyValue(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                    return property.stringValue;
                case SerializedPropertyType.Integer:
                    return property.intValue;
                case SerializedPropertyType.Float:
                    return property.floatValue;
                default:
                    return null;
            }
        }

        // SerializedProperty�� ���� �����ϴ� �޼���
        private void SetPropertyValue(SerializedProperty property, object value)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                    property.stringValue = (string)value;
                    break;
                case SerializedPropertyType.Integer:
                    property.intValue = (int)value;
                    break;
                case SerializedPropertyType.Float:
                    property.floatValue = (float)value;
                    break;
            }
        }

        // options �迭�� string �迭�� ��ȯ�ϴ� �޼���
        private string[] GetOptionsAsStrings(object[] options)
        {
            string[] stringOptions = new string[options.Length];
            for (int i = 0; i < options.Length; i++)
            {
                stringOptions[i] = options[i].ToString();
            }
            return stringOptions;
        }
    }
#endif
}