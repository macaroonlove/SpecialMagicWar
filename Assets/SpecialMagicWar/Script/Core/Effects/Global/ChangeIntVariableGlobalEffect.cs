using ScriptableObjectArchitecture;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class ChangeIntVariableGlobalEffect : GlobalEffect
    {
        [SerializeField] private ObscuredIntVariable _target;
        [SerializeField] private EOperator _operator = EOperator.Add;
        [SerializeField] private int _value;

        public override string GetDescription()
        {
            if (_target == null)
            {
                return "������ �־��ּ���.";
            }

            switch (_operator)
            {
                case EOperator.Add:
                    return $"{_target.name}�� ���� {_value}��ŭ ���ϱ�";
                case EOperator.Multiply:
                    return $"{_target.name}�� ���� {_value}��ŭ ���ϱ�";
                case EOperator.Set:
                    return $"{_target.name}�� ���� {_value}�� �����ϱ�";
            }
            return "����! Ȯ�� �ʿ�";
        }

        public override void Execute()
        {
            if (_target == null) return;

            switch (_operator)
            {
                case EOperator.Add:
                    _target.Value += _value;
                    break;
                case EOperator.Multiply:
                    _target.Value *= _value;
                    break;
                case EOperator.Set:
                    _target.Value = _value;
                    break;
            }
        }

#if UNITY_EDITOR
        public override void Draw(Rect rect)
        {
            var labelRect = new Rect(rect.x, rect.y, 140, rect.height);
            var valueRect = new Rect(rect.x + 140, rect.y, rect.width - 140, rect.height);

            GUI.Label(labelRect, "����");
            _target = EditorGUI.ObjectField(valueRect, _target, typeof(ObscuredIntVariable), false) as ObscuredIntVariable;
            labelRect.y += 20;
            valueRect.y += 20;
            GUI.Label(labelRect, "������");
            _operator = (EOperator)EditorGUI.EnumPopup(valueRect, _operator);
            labelRect.y += 20;
            valueRect.y += 20;
            GUI.Label(labelRect, "��");
            _value = EditorGUI.IntField(valueRect, _value);
        }

        public override int GetNumRows()
        {
            return 3;
        }
#endif
    }
}
