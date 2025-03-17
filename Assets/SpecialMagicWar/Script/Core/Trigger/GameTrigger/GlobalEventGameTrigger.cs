using ScriptableObjectArchitecture;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class GlobalEventGameTrigger : GameTrigger
    {
        [SerializeField] private GlobalEvent _globalEvent;

        internal GlobalEvent globalEvent => _globalEvent;

        public override string GetLabel()
        {
            if (_globalEvent != null)
            {
                return $"{_globalEvent.name} ȣ�� ��";
            }
            else
            {
                return "Ư�� �۷ι� �̺�Ʈ �߻� ��";
            }
        }

#if UNITY_EDITOR
        public override void Draw()
        {
            GUILayout.BeginVertical("GroupBox");
            {
                _globalEvent = (GlobalEvent)EditorGUILayout.ObjectField(_globalEvent, typeof(GlobalEvent), false);
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