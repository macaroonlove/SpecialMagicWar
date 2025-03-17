using ScriptableObjectArchitecture;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class UnitEventGameTrigger : GameTrigger
    {
        [SerializeField] private UnitEvent _unitEvent;

        internal UnitEvent unitEvent => _unitEvent;

        public override string GetLabel()
        {
            if (_unitEvent != null)
            {
                return $"{_unitEvent.name} ȣ�� ��";
            }
            else
            {
                return "Ư�� ���� �̺�Ʈ �߻� ��";
            }
        }

#if UNITY_EDITOR
        public override void Draw()
        {
            GUILayout.Space(5);

            GUILayout.BeginVertical("GroupBox");
            {
                _unitEvent = (UnitEvent)EditorGUILayout.ObjectField(_unitEvent, typeof(UnitEvent), false);
            }
            GUILayout.EndVertical();

            GUILayout.Space(5);

            GUILayout.BeginVertical();
            {
                base.Draw();
            }
            GUILayout.EndVertical();
        }
#endif
    }
}