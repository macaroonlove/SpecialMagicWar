using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.VisualNovel
{
    public class ChapterTemplate : ScriptableObject
    {
        [HideInInspector, SerializeField] private string _title;
        [HideInInspector, SerializeField] private string _sheetID;
        [HideInInspector, SerializeField] private string _gid;
        [HideInInspector, SerializeField] private string _range = "A2:C";
        [HideInInspector, SerializeField] internal ChapterTemplate nextChapter;

        [HideInInspector] public List<Episode> episodes = new List<Episode>();

        public string title => _title;

        public void SetTitle(string title)
        {
            _title = title;
        }
    }
}

#if UNITY_EDITOR
namespace FrameWork.VisualNovel.Editor
{
    using System;
    using System.Threading.Tasks;
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine.Networking;

    [CustomEditor(typeof(ChapterTemplate)), CanEditMultipleObjects]
    public class ChapterTemplateEditor : Editor
    {
        private ChapterTemplate _target;

        private SerializedProperty _title;
        private SerializedProperty _sheetID;
        private SerializedProperty _gid;
        private SerializedProperty _range;
        private SerializedProperty nextChapter;

        private ReorderableList _reorderableList;

        private void OnEnable()
        {
            _target = target as ChapterTemplate;

            _title = serializedObject.FindProperty("_title");
            _sheetID = serializedObject.FindProperty("_sheetID");
            _gid = serializedObject.FindProperty("_gid");
            _range = serializedObject.FindProperty("_range");
            nextChapter = serializedObject.FindProperty("nextChapter");

            SetEpisodeList();
        }

        #region ReorderableList
        private void SetEpisodeList()
        {
            _reorderableList = new ReorderableList(_target.episodes, typeof(Episode), true, true, true, true)
            {
                drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(rect, "���Ǽҵ� ���");
                },
                onAddDropdownCallback = (buttonRect, list) =>
                {
                    EpisodeMenu();
                },
                onRemoveCallback = list =>
                {
                    if (!EditorUtility.DisplayDialog("���!", "�� �׸��� �����Ͻðڽ��ϱ�?", "��", "�ƴϿ�")) return;

                    _target.episodes.RemoveAt(list.index);

                    EditorUtility.SetDirty(_target);
                },
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    var episode = _target.episodes[index];
                    episode.Draw(rect);

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(_target);
                    }
                },
                elementHeightCallback = (index) =>
                {
                    var episode = _target.episodes[index];
                    return episode.GetHeight() * 20 + 8;
                }
            };
        }

        private void EpisodeMenu()
        {
            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("��ɾ� �׷����� ���� ����"), false, CreateEpisodeCallback, typeof(CommandGroupStartEpisode));
            menu.AddItem(new GUIContent("��ɾ� �׷����� ���� ����"), false, CreateEpisodeCallback, typeof(CommandGroupEndEpisode));
            menu.AddItem(new GUIContent("��ȭ ���� ��ɾ�"), false, CreateEpisodeCallback, typeof(SpeakStartEpisode));
            menu.AddItem(new GUIContent("��ȭ ���� ��ɾ�"), false, CreateEpisodeCallback, typeof(SpeakEndEpisode));
            menu.AddItem(new GUIContent("ĳ���� �� ��ü ǥ�� ��ɾ�"), false, CreateEpisodeCallback, typeof(SCGShowEpisode));
            menu.AddItem(new GUIContent("ĳ���� �� ��ü ���� ��ɾ�"), false, CreateEpisodeCallback, typeof(SCGHideEpisode));
            menu.AddItem(new GUIContent("ĳ���� �� ��ü �̵� ��ɾ�"), false, CreateEpisodeCallback, typeof(SCGMoveEpisode));
            menu.AddItem(new GUIContent("��� ǥ�� ��ɾ�"), false, CreateEpisodeCallback, typeof(ECGShowEpisode));
            menu.AddItem(new GUIContent("��� ���� ��ɾ�"), false, CreateEpisodeCallback, typeof(ECGHideEpisode));
            menu.AddItem(new GUIContent("����� ���� ��ɾ�"), false, CreateEpisodeCallback, typeof(BGMPlayEpisode));
            menu.AddItem(new GUIContent("����� ���� ��ɾ�"), false, CreateEpisodeCallback, typeof(BGMStopEpisode));
            menu.AddItem(new GUIContent("ȿ���� ���� ��ɾ�"), false, CreateEpisodeCallback, typeof(SFXPlayEpisode));
            menu.AddItem(new GUIContent("ȿ���� ���� ��ɾ�"), false, CreateEpisodeCallback, typeof(SFXStopEpisode));
            menu.AddItem(new GUIContent("������ ��ɾ�"), false, CreateEpisodeCallback, typeof(ChoiceEpisode));
            menu.AddItem(new GUIContent("������ ȹ�� ��ɾ�"), false, CreateEpisodeCallback, typeof(GetEpisode));

            menu.ShowAsContext();
        }

        private void CreateEpisodeCallback(object obj)
        {
            var episode = CreateInstance((Type)obj) as Episode;
            
            if (episode != null)
            {
                episode.hideFlags = HideFlags.HideInHierarchy;
                _target.episodes.Add(episode);

                var template = target as ChapterTemplate;
                var path = AssetDatabase.GetAssetPath(template);
                AssetDatabase.AddObjectToAsset(episode, path);
                EditorUtility.SetDirty(template);
            }
        }
        #endregion

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.BeginHorizontal();
            GUILayout.Label("����", GUILayout.Width(120));
            EditorGUILayout.PropertyField(_title, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("���� é��", GUILayout.Width(120));
            EditorGUILayout.PropertyField(nextChapter, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Sheet ID", GUILayout.Width(120));
            EditorGUILayout.PropertyField(_sheetID, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("GID", GUILayout.Width(120));
            EditorGUILayout.PropertyField(_gid, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("����", GUILayout.Width(120));
            EditorGUILayout.PropertyField(_range, GUIContent.none);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("�ҷ�����"))
            {
                LoadCSVData(_sheetID.stringValue, _gid.stringValue, _range.stringValue);
            }

            GUILayout.Space(20);

            _reorderableList?.DoLayoutList();

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(this);
            }
        }

        #region CSV ������ �ҷ�����
        private async void LoadCSVData(string sheetID, string gid, string range)
        {
            string csvData = await LoadCSVFromGoogleSheets(sheetID, gid, range);
            if (!string.IsNullOrEmpty(csvData))
            {
                ConvertCSVToEpisode(csvData);
            }
        }

        private async Task<string> LoadCSVFromGoogleSheets(string sheetID, string gid, string range)
        {
            using (UnityWebRequest request = UnityWebRequest.Get($"https://docs.google.com/spreadsheets/d/{sheetID}/export?format=csv&gid={gid}&range={range}"))
            {
                var operation = request.SendWebRequest();

                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    return request.downloadHandler.text;
                }
                else
                {
                    Debug.LogError("���������Ʈ ������ �ε� ����: " + request.error);
                    return null;
                }
            }
        }

        private void ConvertCSVToEpisode(string data)
        {
            var template = target as ChapterTemplate;
            var path = AssetDatabase.GetAssetPath(template);

            List<Episode> episodes = _target.episodes;
            episodes.Clear();

            var lines = data.Split("\r\n");

            foreach (var line in lines)
            {
                var cell = line.Split(',');

                if (Enum.TryParse(cell[0], out CommandType command))
                {
                    Episode episode = null;

                    switch (command)
                    {
                        case CommandType.CommandGroup_Start:
                            episode = CreateInstance<CommandGroupStartEpisode>();
                            break;
                        case CommandType.CommandGroup_End:
                            episode = CreateInstance<CommandGroupEndEpisode>();
                            break;
                        case CommandType.Speak_Start:
                            episode = CreateInstance<SpeakStartEpisode>();
                            ((SpeakStartEpisode)episode).Initialize(cell[1], cell[2]);
                            break;
                        case CommandType.Speak_End:
                            episode = CreateInstance<SpeakEndEpisode>();
                            break;
                        case CommandType.SCG_Show:
                            episode = CreateInstance<SCGShowEpisode>();
                            ((SCGShowEpisode)episode).Initialize(cell[1], cell[2]);
                            break;
                        case CommandType.SCG_Hide:
                            episode = CreateInstance<SCGHideEpisode>();
                            ((SCGHideEpisode)episode).Initialize(cell[1]);
                            break;
                        case CommandType.SCG_Move:
                            episode = CreateInstance<SCGMoveEpisode>();
                            ((SCGMoveEpisode)episode).Initialize(cell[1], cell[2]);
                            break;
                        case CommandType.ECG_Show:
                            episode = CreateInstance<ECGShowEpisode>();
                            ((ECGShowEpisode)episode).Initialize(cell[1]);
                            break;
                        case CommandType.ECG_Hide:
                            episode = CreateInstance<ECGHideEpisode>();
                            break;
                        case CommandType.BGM_Play:
                            episode = CreateInstance<BGMPlayEpisode>();
                            ((BGMPlayEpisode)episode).Initialize(cell[1]);
                            break;
                        case CommandType.BGM_Stop:
                            episode = CreateInstance<BGMStopEpisode>();
                            break;
                        case CommandType.SFX_Play:
                            episode = CreateInstance<SFXPlayEpisode>();
                            ((SFXPlayEpisode)episode).Initialize(cell[1]);
                            break;
                        case CommandType.SFX_Stop:
                            episode = CreateInstance<SFXStopEpisode>();
                            break;
                        case CommandType.Choice:
                            episode = CreateInstance<ChoiceEpisode>();
                            ((ChoiceEpisode)episode).Initialize(cell[2]);
                            break;
                        case CommandType.Get:
                            episode = CreateInstance<GetEpisode>();
                            ((GetEpisode)episode).Initialize(cell[1], cell[2]);
                            break;
                    }

                    if (episode != null)
                    {
                        episode.hideFlags = HideFlags.HideInHierarchy;
                        AssetDatabase.AddObjectToAsset(episode, path);
                        episodes.Add(episode);
                    }
                }
            }
            
            EditorUtility.SetDirty(template);
        }
        #endregion
    }
}
#endif