using System;
using UnityEditor;
using UnityEngine;

namespace FrameWork.VisualNovel
{
    public enum CommandType
    {
        CommandGroup_Start, // Ŀ�ǵ� ���� ����
        CommandGroup_End,   // Ŀ�ǵ� ���� ����
        Speak_Start, // ��ȭ ����
        Speak_End, // ��ȭ ����
        SCG_Show, // ĳ���ͳ� ��ü �Ϸ���Ʈ�� ���̰� �ϱ�
        SCG_Hide, // ĳ���ͳ� ��ü �Ϸ���Ʈ�� �����
        SCG_Move, // ĳ���ͳ� ��ü �Ϸ���Ʈ�� �ִϸ��̼� �����ϱ�
        ECG_Show, // ��� �Ϸ���Ʈ�� ���̰� �ϱ�
        ECG_Hide, // ��� �Ϸ���Ʈ�� �����
        BGM_Play, // ����� ���
        BGM_Stop, // ����� ����
        SFX_Play, // ȿ���� ���
        SFX_Stop, // ȿ���� ����
        Choice,   // ������
        Get,      // ������ ȹ��
    }

    [Serializable]
    public abstract class Episode : ScriptableObject
    {
        public abstract CommandType command { get; }

        public virtual void Draw(Rect rect)
        {
            var commandRect = new Rect(rect.x, rect.y + 4, 125, 20);
            EditorGUI.LabelField(commandRect, command.ToString());
        }

        public virtual int GetHeight()
        {
            return 1;
        }
    }

    [Serializable]
    public abstract class ThemeEpisode : Episode
    {
        public string theme;

        public void Initialize(string theme)
        {
            this.theme = theme;
        }

#if UNITY_EDITOR
        public override void Draw(Rect rect)
        {
            base.Draw(rect);

            var themeRect = new Rect(rect.x + 130, rect.y + 4, 190, 18);
            theme = EditorGUI.TextField(themeRect, theme);
        }
#endif
    }
}