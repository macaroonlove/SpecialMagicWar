using FrameWork;
using System;
using System.Collections.Generic;
using System.IO;
using SpecialMagicWar.Core;
using UnityEditor;
using UnityEngine;

namespace SpecialMagicWar.Editor
{
    public class DataSystemEditorWindow : EditorWindow
    {
        private int selectedTab = 0;
        private Texture2D emptyTexture2D;

        private Vector2 contentScrollPosition;

        #region ����
        private int selectedUnitTitle = 0;

        #region �Ʊ� ����
        private UnityEditor.Editor agentEditor;
        private int selectedAgentIndex = 0;
        private Vector2 agentScrollPosition;
        private List<Tuple<AgentTemplate, Texture2D>> agentTemplates = new List<Tuple<AgentTemplate, Texture2D>>();
        #endregion
        #region ���� ����
        private UnityEditor.Editor enemyEditor;
        private int selectedEnemyIndex = 0;
        private Vector2 enemyScrollPosition;
        private List<Tuple<EnemyTemplate, Texture2D>> enemyTemplates = new List<Tuple<EnemyTemplate, Texture2D>>();
        #endregion
        #endregion

        #region ����
        private int selectedStatusTitle = 0;
        #region ����
        private UnityEditor.Editor buffEditor;
        private int selectedBuffIndex = 0;
        private Vector2 buffScrollPosition;
        private List<BuffTemplate> buffTemplates = new List<BuffTemplate>();
        #endregion
        #region �����̻�
        private UnityEditor.Editor abnormalStatusEditor;
        private int selectedAbnormalStatusIndex = 0;
        private Vector2 abnormalStatusScrollPosition;
        private List<Tuple<AbnormalStatusTemplate, Texture2D>> abnormalStatusTemplates = new List<Tuple<AbnormalStatusTemplate, Texture2D>>();
        #endregion
        #region �����̻�
        private UnityEditor.Editor globalStatusEditor;
        private int selectedGlobalStatusIndex = 0;
        private Vector2 globalStatusScrollPosition;
        private List<Tuple<GlobalStatusTemplate, Texture2D>> globalStatusTemplates = new List<Tuple<GlobalStatusTemplate, Texture2D>>();
        #endregion
        #endregion

        #region ��ų
        private int selectedSkillTitle = 0;
        #region ��Ƽ�� ��ų
        private UnityEditor.Editor activeSkillEditor;
        private int selectedActiveSkillIndex = 0;
        private Vector2 activeSkillScrollPosition;
        private List<Tuple<ActiveSkillTemplate, Texture2D>> activeSkillTemplates = new List<Tuple<ActiveSkillTemplate, Texture2D>>();
        #endregion
        #region �нú� ��ų
        private UnityEditor.Editor passiveSkillEditor;
        private int selectedPassiveSkillIndex = 0;
        private Vector2 passiveSkillScrollPosition;
        private List<Tuple<PassiveSkillTemplate, Texture2D>> passiveSkillTemplates = new List<Tuple<PassiveSkillTemplate, Texture2D>>();
        #endregion
        #endregion

        #region ������
        private int selectedItemTitle = 0;
        #region ��Ƽ�� ������
        private UnityEditor.Editor activeItemEditor;
        private int selectedActiveItemIndex = 0;
        private Vector2 activeItemScrollPosition;
        private List<Tuple<ActiveItemTemplate, Texture2D>> activeItemTemplates = new List<Tuple<ActiveItemTemplate, Texture2D>>();
        #endregion
        #region �нú� ������
        private UnityEditor.Editor passiveItemEditor;
        private int selectedPassiveItemIndex = 0;
        private Vector2 passiveItemScrollPosition;
        private List<Tuple<PassiveItemTemplate, Texture2D>> passiveItemTemplates = new List<Tuple<PassiveItemTemplate, Texture2D>>();
        #endregion
        #endregion

        [MenuItem("Window/���� ������ ���� �ý���")]
        public static void Open()
        {
            var window = GetWindow<DataSystemEditorWindow>();
            window.titleContent = new GUIContent("���� ������ ����");
        }

        private void OnGUI()
        {
            DrawTab();
        }

        private void OnDisable()
        {
            if (agentEditor != null)
            {
                DestroyImmediate(agentEditor);
                agentEditor = null;
            }
            if (enemyEditor != null)
            {
                DestroyImmediate(enemyEditor);
                enemyEditor = null;
            }
            if (buffEditor != null)
            {
                DestroyImmediate(buffEditor);
                buffEditor = null;
            }
            if (abnormalStatusEditor != null)
            {
                DestroyImmediate(abnormalStatusEditor);
                abnormalStatusEditor = null;
            }
            if (activeSkillEditor != null)
            {
                DestroyImmediate(activeSkillEditor);
                activeSkillEditor = null;
            }
            if (passiveSkillEditor != null)
            {
                DestroyImmediate(passiveSkillEditor);
                passiveSkillEditor = null;
            }
        }

        private void DrawTab()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(selectedTab == 0, "����", "Button")) selectedTab = 0;
            if (GUILayout.Toggle(selectedTab == 1, "����", "Button")) selectedTab = 1;
            if (GUILayout.Toggle(selectedTab == 2, "��ų", "Button")) selectedTab = 2;
            if (GUILayout.Toggle(selectedTab == 3, "������", "Button")) selectedTab = 3;
            GUILayout.EndHorizontal();

            DrawLine();

            switch (selectedTab)
            {
                case 0:
                    DrawUnitTitle();
                    break;
                case 1:
                    DrawStatusTitle();
                    break;
                case 2:
                    DrawSkillTitle();
                    break;
                case 3:
                    DrawItemTitle();
                    break;
            }
        }

        private void DrawUnitTitle()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(selectedUnitTitle == 0, "�Ʊ� ����", "Button")) selectedUnitTitle = 0;
            if (GUILayout.Toggle(selectedUnitTitle == 1, "�� ����", "Button")) selectedUnitTitle = 1;
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            if (selectedUnitTitle == 0) DrawAgentTab();
            else DrawEnemyTab();
        }

        private void DrawStatusTitle()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(selectedStatusTitle == 0, "����", "Button")) selectedStatusTitle = 0;
            if (GUILayout.Toggle(selectedStatusTitle == 1, "�����̻�", "Button")) selectedStatusTitle = 1;
            if (GUILayout.Toggle(selectedStatusTitle == 2, "���� ����", "Button")) selectedStatusTitle = 2;
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            if (selectedStatusTitle == 0) DrawBuffTab();
            else if (selectedStatusTitle == 1) DrawAbnormalStatusTab();
            else DrawGlobalStatusTab();
        }

        private void DrawSkillTitle()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(selectedSkillTitle == 0, "��Ƽ�� ��ų", "Button")) selectedSkillTitle = 0;
            if (GUILayout.Toggle(selectedSkillTitle == 1, "�нú� ��ų", "Button")) selectedSkillTitle = 1;
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            if (selectedSkillTitle == 0) DrawActiveSkillTab();
            else DrawPassiveSkillTab();
        }

        private void DrawItemTitle()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(selectedItemTitle == 0, "��Ƽ�� ������", "Button")) selectedItemTitle = 0;
            if (GUILayout.Toggle(selectedItemTitle == 1, "�нú� ������", "Button")) selectedItemTitle = 1;
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            if (selectedItemTitle == 0) DrawActiveItemTab();
            else DrawPassiveItemTab();
        }

        #region �Ʊ� ����
        private void DrawAgentTab()
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(GUILayout.Width(200));
            if (GUILayout.Button("�Ʊ� ���� �߰�"))
            {
                AddAgentTemplate();
            }
            if (GUILayout.Button("�Ʊ� ���� ����"))
            {
                DeleteSelectedAgentTemplate();
            }
            if (GUILayout.Button("�Ʊ� ���� Ž��"))
            {
                LoadAgentTemplates();
            }

            DrawLine();

            agentScrollPosition = GUILayout.BeginScrollView(agentScrollPosition, false, true);

            var agentCatalog = new GUIStyle(GUI.skin.button);
            agentCatalog.alignment = TextAnchor.MiddleLeft;
            agentCatalog.padding = new RectOffset(5, 5, 5, 5);
            agentCatalog.margin = new RectOffset(5, 5, -2, -2);
            agentCatalog.border = new RectOffset(0, 0, 0, 0);
            agentCatalog.fixedWidth = GUI.skin.box.fixedWidth;
            agentCatalog.fixedHeight = GUI.skin.box.fixedHeight;

            for (int i = 0; i < agentTemplates.Count; i++)
            {
                bool isSelected = (selectedAgentIndex == i);

                var text = "  " + agentTemplates[i].Item1.displayName;
                text = text.Substring(0, Mathf.Min(text.Length, 13));
                GUIContent content = new GUIContent(text, agentTemplates[i].Item2);

                if (GUILayout.Toggle(isSelected, content, agentCatalog))
                {
                    if (selectedAgentIndex != i)
                    {
                        selectedAgentIndex = i;

                        GUI.FocusControl(null);
                    }
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));

            if (agentTemplates.Count > 0 && selectedAgentIndex < agentTemplates.Count)
            {
                AgentTemplate selectedAgent = agentTemplates[selectedAgentIndex].Item1;

                if (agentEditor == null || agentEditor.target != selectedAgent)
                {
                    agentEditor = UnityEditor.Editor.CreateEditor(selectedAgent);
                }

                contentScrollPosition = GUILayout.BeginScrollView(contentScrollPosition, false, false);
                agentEditor.OnInspectorGUI();
                GUILayout.EndScrollView();
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void AddAgentTemplate()
        {
            // �Ʊ� ���� ���ø� ����
            AgentTemplate newAgent = CreateInstance<AgentTemplate>();

            // ���� ����
            string defaultPath = "Assets/FrameWork/Core/GameData/Unit/Agent";
            string path = EditorUtility.SaveFilePanelInProject("FrameWork/Core/GameData/Unit/Agent", "Agent_", "asset", "�Ʊ� ���ø��� FrameWork/Core/GameData/Unit/Agent ��ġ�� ����˴ϴ�..", defaultPath);
            if (!string.IsNullOrEmpty(path))
            {
                newAgent.SetDisplayName(Path.GetFileNameWithoutExtension(path).Replace("Agent_", ""));

                AssetDatabase.CreateAsset(newAgent, path);
                AssetDatabase.SaveAssets();
                LoadAgentTemplates();
            }
        }

        private void DeleteSelectedAgentTemplate()
        {
            if (agentTemplates.Count > 0)
            {
                AgentTemplate selectedAgent = agentTemplates[selectedAgentIndex].Item1;
                string assetPath = AssetDatabase.GetAssetPath(selectedAgent);
                agentTemplates.RemoveAt(selectedAgentIndex);
                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.SaveAssets();
            }
        }

        private void LoadAgentTemplates()
        {
            if (emptyTexture2D == null) emptyTexture2D = CreateTexture(Color.gray);

            agentTemplates.Clear();
            string[] guids = AssetDatabase.FindAssets("t:AgentTemplate");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AgentTemplate agent = AssetDatabase.LoadAssetAtPath<AgentTemplate>(path);

                var texture = (agent.sprite == null) ? emptyTexture2D : agent.sprite.texture;
                texture = texture.ResizeTexture(30, 30);

                agentTemplates.Add(new Tuple<AgentTemplate, Texture2D>(agent, texture));
            }
        }
        #endregion

        #region �� ����
        private void DrawEnemyTab()
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(GUILayout.Width(200));
            if (GUILayout.Button("�� ���� �߰�"))
            {
                AddEnemyTemplate();
            }
            if (GUILayout.Button("�� ���� ����"))
            {
                DeleteSelectedEnemyTemplate();
            }
            if (GUILayout.Button("�� ���� Ž��"))
            {
                LoadEnemyTemplates();
            }

            DrawLine();

            agentScrollPosition = GUILayout.BeginScrollView(agentScrollPosition, false, true);

            var enemyCatalog = new GUIStyle(GUI.skin.button);
            enemyCatalog.alignment = TextAnchor.MiddleLeft;
            enemyCatalog.padding = new RectOffset(5, 5, 5, 5);
            enemyCatalog.margin = new RectOffset(5, 5, -2, -2);
            enemyCatalog.border = new RectOffset(0, 0, 0, 0);
            enemyCatalog.fixedWidth = GUI.skin.box.fixedWidth;
            enemyCatalog.fixedHeight = GUI.skin.box.fixedHeight;

            for (int i = 0; i < enemyTemplates.Count; i++)
            {
                bool isSelected = (selectedEnemyIndex == i);

                var text = "  " + enemyTemplates[i].Item1.displayName;
                text = text.Substring(0, Mathf.Min(text.Length, 13));
                GUIContent content = new GUIContent(text, enemyTemplates[i].Item2);

                if (GUILayout.Toggle(isSelected, content, enemyCatalog))
                {
                    if (selectedEnemyIndex != i)
                    {
                        selectedEnemyIndex = i;

                        GUI.FocusControl(null);
                    }
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));

            if (enemyTemplates.Count > 0 && selectedEnemyIndex < enemyTemplates.Count)
            {
                EnemyTemplate selectedEnemy = enemyTemplates[selectedEnemyIndex].Item1;

                if (enemyEditor == null || enemyEditor.target != selectedEnemy)
                {
                    enemyEditor = UnityEditor.Editor.CreateEditor(selectedEnemy);
                }

                contentScrollPosition = GUILayout.BeginScrollView(contentScrollPosition, false, false);
                enemyEditor.OnInspectorGUI();
                GUILayout.EndScrollView();
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void AddEnemyTemplate()
        {
            // �� ���� ���ø� ����
            EnemyTemplate newEnemy = CreateInstance<EnemyTemplate>();

            // ���� ����
            string defaultPath = "Assets/FrameWork/Core/GameData/Unit/Enemy";
            string path = EditorUtility.SaveFilePanelInProject("FrameWork/Core/GameData/Unit/Enemy", "Enemy_", "asset", "�� ���ø��� FrameWork/Core/GameData/Unit/Enemy ��ġ�� ����˴ϴ�..", defaultPath);
            if (!string.IsNullOrEmpty(path))
            {
                newEnemy.SetDisplayName(Path.GetFileNameWithoutExtension(path).Replace("Enemy_", ""));

                AssetDatabase.CreateAsset(newEnemy, path);
                AssetDatabase.SaveAssets();
                LoadEnemyTemplates();
            }
        }

        private void DeleteSelectedEnemyTemplate()
        {
            if (enemyTemplates.Count > 0)
            {
                EnemyTemplate selectedEnemy = enemyTemplates[selectedEnemyIndex].Item1;
                string assetPath = AssetDatabase.GetAssetPath(selectedEnemy);
                enemyTemplates.RemoveAt(selectedEnemyIndex);
                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.SaveAssets();
            }
        }

        private void LoadEnemyTemplates()
        {
            if (emptyTexture2D == null) emptyTexture2D = CreateTexture(Color.gray);

            enemyTemplates.Clear();
            string[] guids = AssetDatabase.FindAssets("t:EnemyTemplate");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                EnemyTemplate enemy = AssetDatabase.LoadAssetAtPath<EnemyTemplate>(path);

                var texture = (enemy.sprite == null) ? emptyTexture2D : enemy.sprite.texture;
                texture = texture.ResizeTexture(30, 30);

                enemyTemplates.Add(new Tuple<EnemyTemplate, Texture2D>(enemy, texture));
            }
        }
        #endregion

        #region ����
        private void DrawBuffTab()
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(GUILayout.Width(200));
            if (GUILayout.Button("���� �߰�"))
            {
                AddBuffTemplate();
            }
            if (GUILayout.Button("���� ����"))
            {
                DeleteSelectedBuffTemplate();
            }
            if (GUILayout.Button("���� Ž��"))
            {
                LoadBuffTemplates();
            }

            DrawLine();

            buffScrollPosition = GUILayout.BeginScrollView(buffScrollPosition, false, true);

            var buffCatalog = new GUIStyle(GUI.skin.button);
            buffCatalog.alignment = TextAnchor.MiddleLeft;
            buffCatalog.padding = new RectOffset(5, 5, 5, 5);
            buffCatalog.margin = new RectOffset(5, 5, -2, -2);
            buffCatalog.border = new RectOffset(0, 0, 0, 0);
            buffCatalog.fixedWidth = GUI.skin.box.fixedWidth;
            buffCatalog.fixedHeight = GUI.skin.box.fixedHeight;

            for (int i = 0; i < buffTemplates.Count; i++)
            {
                bool isSelected = (selectedBuffIndex == i);

                var text = "  " + buffTemplates[i].displayName;
                text = text.Substring(0, Mathf.Min(text.Length, 13));

                if (GUILayout.Toggle(isSelected, text, buffCatalog))
                {
                    if (selectedBuffIndex != i)
                    {
                        selectedBuffIndex = i;

                        GUI.FocusControl(null);
                    }
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));

            if (buffTemplates.Count > 0 && selectedBuffIndex < buffTemplates.Count)
            {
                BuffTemplate selectedbuff = buffTemplates[selectedBuffIndex];

                if (buffEditor == null || buffEditor.target != selectedbuff)
                {
                    buffEditor = UnityEditor.Editor.CreateEditor(selectedbuff);
                }
                contentScrollPosition = GUILayout.BeginScrollView(contentScrollPosition, false, false);
                buffEditor.OnInspectorGUI();
                GUILayout.EndScrollView();
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void AddBuffTemplate()
        {
            // ���� ���ø� ����
            BuffTemplate newBuff = CreateInstance<BuffTemplate>();

            // ���� ����
            string defaultPath = "Assets/FrameWork/Core/GameData/Status/BuffStatus";
            string path = EditorUtility.SaveFilePanelInProject("FrameWork/Core/GameData/Status/BuffStatus", "BuffStatus_", "asset", "�����̻� ���ø��� FrameWork/Core/GameData/Status/BuffStatus ��ġ�� ����˴ϴ�..", defaultPath);
            if (!string.IsNullOrEmpty(path))
            {
                newBuff.SetDisplayName(Path.GetFileNameWithoutExtension(path).Replace("BuffStatus_", ""));

                AssetDatabase.CreateAsset(newBuff, path);
                AssetDatabase.SaveAssets();
                LoadBuffTemplates();
            }
        }

        private void DeleteSelectedBuffTemplate()
        {
            if (buffTemplates.Count > 0)
            {
                BuffTemplate selectedBuff = buffTemplates[selectedBuffIndex];
                string assetPath = AssetDatabase.GetAssetPath(selectedBuff);
                buffTemplates.RemoveAt(selectedBuffIndex);
                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.SaveAssets();
            }
        }

        private void LoadBuffTemplates()
        {
            buffTemplates.Clear();
            string[] guids = AssetDatabase.FindAssets("t:BuffTemplate");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                BuffTemplate buff = AssetDatabase.LoadAssetAtPath<BuffTemplate>(path);

                buffTemplates.Add(buff);
            }
        }
        #endregion

        #region �����̻�
        private void DrawAbnormalStatusTab()
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(GUILayout.Width(200));
            if (GUILayout.Button("�����̻� �߰�"))
            {
                AddAbnormalStatusTemplate();
            }
            if (GUILayout.Button("�����̻� ����"))
            {
                DeleteSelectedAbnormalStatusTemplate();
            }
            if (GUILayout.Button("�����̻� Ž��"))
            {
                LoadAbnormalStatusTemplates();
            }

            DrawLine();

            abnormalStatusScrollPosition = GUILayout.BeginScrollView(abnormalStatusScrollPosition, false, true);

            var abnormalStatusCatalog = new GUIStyle(GUI.skin.button);
            abnormalStatusCatalog.alignment = TextAnchor.MiddleLeft;
            abnormalStatusCatalog.padding = new RectOffset(5, 5, 5, 5);
            abnormalStatusCatalog.margin = new RectOffset(5, 5, -2, -2);
            abnormalStatusCatalog.border = new RectOffset(0, 0, 0, 0);
            abnormalStatusCatalog.fixedWidth = GUI.skin.box.fixedWidth;
            abnormalStatusCatalog.fixedHeight = GUI.skin.box.fixedHeight;

            for (int i = 0; i < abnormalStatusTemplates.Count; i++)
            {
                bool isSelected = (selectedAbnormalStatusIndex == i);

                var text = "  " + abnormalStatusTemplates[i].Item1.displayName;
                text = text.Substring(0, Mathf.Min(text.Length, 13));
                GUIContent content = new GUIContent(text, abnormalStatusTemplates[i].Item2);

                if (GUILayout.Toggle(isSelected, content, abnormalStatusCatalog))
                {
                    if (selectedAbnormalStatusIndex != i)
                    {
                        selectedAbnormalStatusIndex = i;

                        GUI.FocusControl(null);
                    }
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));

            if (abnormalStatusTemplates.Count > 0 && selectedAbnormalStatusIndex < abnormalStatusTemplates.Count)
            {
                AbnormalStatusTemplate selectedabnormalStatus = abnormalStatusTemplates[selectedAbnormalStatusIndex].Item1;

                if (abnormalStatusEditor == null || abnormalStatusEditor.target != selectedabnormalStatus)
                {
                    abnormalStatusEditor = UnityEditor.Editor.CreateEditor(selectedabnormalStatus);
                }

                contentScrollPosition = GUILayout.BeginScrollView(contentScrollPosition, false, false);
                abnormalStatusEditor.OnInspectorGUI();
                GUILayout.EndScrollView();
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void AddAbnormalStatusTemplate()
        {
            // �����̻� ���ø� ����
            AbnormalStatusTemplate newAbnormalStatus = CreateInstance<AbnormalStatusTemplate>();

            // ���� ����
            string defaultPath = "Assets/FrameWork/Core/GameData/Status/AbnormalStatus";
            string path = EditorUtility.SaveFilePanelInProject("FrameWork/Core/GameData/Status/AbnormalStatus", "AbnormalStatus_", "asset", "�����̻� ���ø��� FrameWork/Core/GameData/Status/AbnormalStatus ��ġ�� ����˴ϴ�..", defaultPath);
            if (!string.IsNullOrEmpty(path))
            {
                newAbnormalStatus.SetDisplayName(Path.GetFileNameWithoutExtension(path).Replace("AbnormalStatus_", ""));

                AssetDatabase.CreateAsset(newAbnormalStatus, path);
                AssetDatabase.SaveAssets();
                LoadAbnormalStatusTemplates();
            }
        }

        private void DeleteSelectedAbnormalStatusTemplate()
        {
            if (abnormalStatusTemplates.Count > 0)
            {
                AbnormalStatusTemplate selectedAbnormalStatus = abnormalStatusTemplates[selectedAbnormalStatusIndex].Item1;
                string assetPath = AssetDatabase.GetAssetPath(selectedAbnormalStatus);
                abnormalStatusTemplates.RemoveAt(selectedAbnormalStatusIndex);
                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.SaveAssets();
            }
        }

        private void LoadAbnormalStatusTemplates()
        {
            if (emptyTexture2D == null) emptyTexture2D = CreateTexture(Color.gray);

            abnormalStatusTemplates.Clear();
            string[] guids = AssetDatabase.FindAssets("t:AbnormalStatusTemplate");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AbnormalStatusTemplate abnormalStatus = AssetDatabase.LoadAssetAtPath<AbnormalStatusTemplate>(path);

                var texture = (abnormalStatus.sprite == null) ? emptyTexture2D : abnormalStatus.sprite.texture;
                texture = texture.ResizeTexture(30, 30);

                abnormalStatusTemplates.Add(new Tuple<AbnormalStatusTemplate, Texture2D>(abnormalStatus, texture));
            }
        }
        #endregion

        #region ���� ����
        private void DrawGlobalStatusTab()
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(GUILayout.Width(200));
            if (GUILayout.Button("���� ���� �߰�"))
            {
                AddGlobalStatusTemplate();
            }
            if (GUILayout.Button("���� ���� ����"))
            {
                DeleteSelectedGlobalStatusTemplate();
            }
            if (GUILayout.Button("���� ���� Ž��"))
            {
                LoadGlobalStatusTemplates();
            }

            DrawLine();

            globalStatusScrollPosition = GUILayout.BeginScrollView(globalStatusScrollPosition, false, true);

            var abnormalStatusCatalog = new GUIStyle(GUI.skin.button);
            abnormalStatusCatalog.alignment = TextAnchor.MiddleLeft;
            abnormalStatusCatalog.padding = new RectOffset(5, 5, 5, 5);
            abnormalStatusCatalog.margin = new RectOffset(5, 5, -2, -2);
            abnormalStatusCatalog.border = new RectOffset(0, 0, 0, 0);
            abnormalStatusCatalog.fixedWidth = GUI.skin.box.fixedWidth;
            abnormalStatusCatalog.fixedHeight = GUI.skin.box.fixedHeight;

            for (int i = 0; i < globalStatusTemplates.Count; i++)
            {
                bool isSelected = (selectedGlobalStatusIndex == i);

                var text = "  " + globalStatusTemplates[i].Item1.displayName;
                text = text.Substring(0, Mathf.Min(text.Length, 13));
                GUIContent content = new GUIContent(text, globalStatusTemplates[i].Item2);

                if (GUILayout.Toggle(isSelected, content, abnormalStatusCatalog))
                {
                    if (selectedGlobalStatusIndex != i)
                    {
                        selectedGlobalStatusIndex = i;

                        GUI.FocusControl(null);
                    }
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));

            if (globalStatusTemplates.Count > 0 && selectedGlobalStatusIndex < globalStatusTemplates.Count)
            {
                GlobalStatusTemplate selectedGlobalStatus = globalStatusTemplates[selectedGlobalStatusIndex].Item1;

                if (globalStatusEditor == null || globalStatusEditor.target != selectedGlobalStatus)
                {
                    globalStatusEditor = UnityEditor.Editor.CreateEditor(selectedGlobalStatus);
                }

                contentScrollPosition = GUILayout.BeginScrollView(contentScrollPosition, false, false);
                globalStatusEditor.OnInspectorGUI();
                GUILayout.EndScrollView();
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void AddGlobalStatusTemplate()
        {
            // �����̻� ���ø� ����
            GlobalStatusTemplate newGlobalStatus = CreateInstance<GlobalStatusTemplate>();

            // ���� ����
            string defaultPath = "Assets/FrameWork/Core/GameData/Status/GlobalStatus";
            string path = EditorUtility.SaveFilePanelInProject("FrameWork/Core/GameData/Status/GlobalStatus", "GlobalStatus_", "asset", "�����̻� ���ø��� FrameWork/Core/GameData/Status/GlobalStatus ��ġ�� ����˴ϴ�..", defaultPath);
            if (!string.IsNullOrEmpty(path))
            {
                newGlobalStatus.SetDisplayName(Path.GetFileNameWithoutExtension(path).Replace("GlobalStatus_", ""));

                AssetDatabase.CreateAsset(newGlobalStatus, path);
                AssetDatabase.SaveAssets();
                LoadGlobalStatusTemplates();
            }
        }

        private void DeleteSelectedGlobalStatusTemplate()
        {
            if (globalStatusTemplates.Count > 0)
            {
                GlobalStatusTemplate selectedGlobalStatus = globalStatusTemplates[selectedGlobalStatusIndex].Item1;
                string assetPath = AssetDatabase.GetAssetPath(selectedGlobalStatus);
                globalStatusTemplates.RemoveAt(selectedGlobalStatusIndex);
                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.SaveAssets();
            }
        }

        private void LoadGlobalStatusTemplates()
        {
            if (emptyTexture2D == null) emptyTexture2D = CreateTexture(Color.gray);

            globalStatusTemplates.Clear();
            string[] guids = AssetDatabase.FindAssets("t:GlobalStatusTemplate");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GlobalStatusTemplate globalStatus = AssetDatabase.LoadAssetAtPath<GlobalStatusTemplate>(path);

                var texture = (globalStatus.sprite == null) ? emptyTexture2D : globalStatus.sprite.texture;
                texture = texture.ResizeTexture(30, 30);

                globalStatusTemplates.Add(new Tuple<GlobalStatusTemplate, Texture2D>(globalStatus, texture));
            }
        }
        #endregion

        #region ��Ƽ�� ��ų
        private void DrawActiveSkillTab()
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(GUILayout.Width(200));
            if (GUILayout.Button("��ų �߰�"))
            {
                AddActiveSkillTemplate();
            }
            if (GUILayout.Button("��ų ����"))
            {
                DeleteSelectedActiveSkillTemplate();
            }
            if (GUILayout.Button("��ų Ž��"))
            {
                LoadActiveSkillTemplates();
            }

            DrawLine();

            activeSkillScrollPosition = GUILayout.BeginScrollView(activeSkillScrollPosition, false, true);

            var skillCatalog = new GUIStyle(GUI.skin.button);
            skillCatalog.alignment = TextAnchor.MiddleLeft;
            skillCatalog.padding = new RectOffset(5, 5, 5, 5);
            skillCatalog.margin = new RectOffset(5, 5, -2, -2);
            skillCatalog.border = new RectOffset(0, 0, 0, 0);
            skillCatalog.fixedWidth = GUI.skin.box.fixedWidth;
            skillCatalog.fixedHeight = GUI.skin.box.fixedHeight;

            for (int i = 0; i < activeSkillTemplates.Count; i++)
            {
                bool isSelected = (selectedActiveSkillIndex == i);

                var text = "  " + activeSkillTemplates[i].Item1.displayName;
                text = text.Substring(0, Mathf.Min(text.Length, 13));
                GUIContent content = new GUIContent(text, activeSkillTemplates[i].Item2);

                if (GUILayout.Toggle(isSelected, content, skillCatalog))
                {
                    if (selectedActiveSkillIndex != i)
                    {
                        selectedActiveSkillIndex = i;

                        GUI.FocusControl(null);
                    }
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));

            if (activeSkillTemplates.Count > 0 && selectedActiveSkillIndex < activeSkillTemplates.Count)
            {
                ActiveSkillTemplate selectedSkill = activeSkillTemplates[selectedActiveSkillIndex].Item1;

                if (activeSkillEditor == null || activeSkillEditor.target != selectedSkill)
                {
                    activeSkillEditor = UnityEditor.Editor.CreateEditor(selectedSkill);
                }

                contentScrollPosition = GUILayout.BeginScrollView(contentScrollPosition, false, false);
                activeSkillEditor.OnInspectorGUI();
                GUILayout.EndScrollView();
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void AddActiveSkillTemplate()
        {
            // ��ų ���ø� ����
            ActiveSkillTemplate newSkill = CreateInstance<ActiveSkillTemplate>();

            // ���� ����
            string defaultPath = "Assets/FrameWork/Core/GameData/Skill/ActiveSkill";
            string path = EditorUtility.SaveFilePanelInProject("FrameWork/Core/GameData/Skill/ActiveSkill", "ActiveSkill_", "asset", "��ų ���ø��� FrameWork/Core/GameData/Skill/ActiveSkill ��ġ�� ����˴ϴ�..", defaultPath);
            if (!string.IsNullOrEmpty(path))
            {
                newSkill.SetDisplayName(Path.GetFileNameWithoutExtension(path).Replace("ActiveSkill_", ""));

                AssetDatabase.CreateAsset(newSkill, path);
                AssetDatabase.SaveAssets();
                LoadActiveSkillTemplates();
            }
        }

        private void DeleteSelectedActiveSkillTemplate()
        {
            if (activeSkillTemplates.Count > 0)
            {
                ActiveSkillTemplate selectedSkill = activeSkillTemplates[selectedActiveSkillIndex].Item1;
                string assetPath = AssetDatabase.GetAssetPath(selectedSkill);
                activeSkillTemplates.RemoveAt(selectedActiveSkillIndex);
                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.SaveAssets();
            }
        }

        private void LoadActiveSkillTemplates()
        {
            if (emptyTexture2D == null) emptyTexture2D = CreateTexture(Color.gray);

            activeSkillTemplates.Clear();
            string[] guids = AssetDatabase.FindAssets("t:ActiveSkillTemplate");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ActiveSkillTemplate skill = AssetDatabase.LoadAssetAtPath<ActiveSkillTemplate>(path);

                var texture = (skill.sprite == null) ? emptyTexture2D : skill.sprite.texture;
                texture = texture.ResizeTexture(30, 30);

                activeSkillTemplates.Add(new Tuple<ActiveSkillTemplate, Texture2D>(skill, texture));
            }
        }
        #endregion

        #region �нú� ��ų
        private void DrawPassiveSkillTab()
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(GUILayout.Width(200));
            if (GUILayout.Button("��ų �߰�"))
            {
                AddPassiveSkillTemplate();
            }
            if (GUILayout.Button("��ų ����"))
            {
                DeleteSelectedPassiveSkillTemplate();
            }
            if (GUILayout.Button("��ų Ž��"))
            {
                LoadPassiveSkillTemplates();
            }

            DrawLine();

            passiveSkillScrollPosition = GUILayout.BeginScrollView(passiveSkillScrollPosition, false, true);

            var skillCatalog = new GUIStyle(GUI.skin.button);
            skillCatalog.alignment = TextAnchor.MiddleLeft;
            skillCatalog.padding = new RectOffset(5, 5, 5, 5);
            skillCatalog.margin = new RectOffset(5, 5, -2, -2);
            skillCatalog.border = new RectOffset(0, 0, 0, 0);
            skillCatalog.fixedWidth = GUI.skin.box.fixedWidth;
            skillCatalog.fixedHeight = GUI.skin.box.fixedHeight;

            for (int i = 0; i < passiveSkillTemplates.Count; i++)
            {
                bool isSelected = (selectedPassiveSkillIndex == i);

                var text = "  " + passiveSkillTemplates[i].Item1.displayName;
                text = text.Substring(0, Mathf.Min(text.Length, 13));
                GUIContent content = new GUIContent(text, passiveSkillTemplates[i].Item2);

                if (GUILayout.Toggle(isSelected, content, skillCatalog))
                {
                    if (selectedPassiveSkillIndex != i)
                    {
                        selectedPassiveSkillIndex = i;

                        GUI.FocusControl(null);
                    }
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));

            if (passiveSkillTemplates.Count > 0 && selectedPassiveSkillIndex < passiveSkillTemplates.Count)
            {
                PassiveSkillTemplate selectedSkill = passiveSkillTemplates[selectedPassiveSkillIndex].Item1;

                if (passiveSkillEditor == null || passiveSkillEditor.target != selectedSkill)
                {
                    passiveSkillEditor = UnityEditor.Editor.CreateEditor(selectedSkill);
                }

                contentScrollPosition = GUILayout.BeginScrollView(contentScrollPosition, false, false);
                passiveSkillEditor.OnInspectorGUI();
                GUILayout.EndScrollView();
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void AddPassiveSkillTemplate()
        {
            // ��ų ���ø� ����
            PassiveSkillTemplate newSkill = CreateInstance<PassiveSkillTemplate>();

            // ���� ����
            string defaultPath = "Assets/FrameWork/Core/GameData/Skill/PassiveSkill";
            string path = EditorUtility.SaveFilePanelInProject("FrameWork/Core/GameData/Skill/PassiveSkill", "PassiveSkill_", "asset", "��ų ���ø��� FrameWork/Core/GameData/Skill/PassiveSkill ��ġ�� ����˴ϴ�..", defaultPath);
            if (!string.IsNullOrEmpty(path))
            {
                newSkill.SetDisplayName(Path.GetFileNameWithoutExtension(path).Replace("PassiveSkill_", ""));

                AssetDatabase.CreateAsset(newSkill, path);
                AssetDatabase.SaveAssets();
                LoadPassiveSkillTemplates();
            }
        }

        private void DeleteSelectedPassiveSkillTemplate()
        {
            if (passiveSkillTemplates.Count > 0)
            {
                PassiveSkillTemplate selectedSkill = passiveSkillTemplates[selectedPassiveSkillIndex].Item1;
                string assetPath = AssetDatabase.GetAssetPath(selectedSkill);
                passiveSkillTemplates.RemoveAt(selectedPassiveSkillIndex);
                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.SaveAssets();
            }
        }

        private void LoadPassiveSkillTemplates()
        {
            if (emptyTexture2D == null) emptyTexture2D = CreateTexture(Color.gray);

            passiveSkillTemplates.Clear();
            string[] guids = AssetDatabase.FindAssets("t:PassiveSkillTemplate");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                PassiveSkillTemplate skill = AssetDatabase.LoadAssetAtPath<PassiveSkillTemplate>(path);

                var texture = (skill.sprite == null) ? emptyTexture2D : skill.sprite.texture;
                texture = texture.ResizeTexture(30, 30);

                passiveSkillTemplates.Add(new Tuple<PassiveSkillTemplate, Texture2D>(skill, texture));
            }
        }
        #endregion

        #region ��Ƽ�� ������
        private void DrawActiveItemTab()
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(GUILayout.Width(200));
            if (GUILayout.Button("������ �߰�"))
            {
                AddActiveItemTemplate();
            }
            if (GUILayout.Button("������ ����"))
            {
                DeleteSelectedActiveItemTemplate();
            }
            if (GUILayout.Button("������ Ž��"))
            {
                LoadActiveItemTemplates();
            }

            DrawLine();

            activeItemScrollPosition = GUILayout.BeginScrollView(activeItemScrollPosition, false, true);

            var skillCatalog = new GUIStyle(GUI.skin.button);
            skillCatalog.alignment = TextAnchor.MiddleLeft;
            skillCatalog.padding = new RectOffset(5, 5, 5, 5);
            skillCatalog.margin = new RectOffset(5, 5, -2, -2);
            skillCatalog.border = new RectOffset(0, 0, 0, 0);
            skillCatalog.fixedWidth = GUI.skin.box.fixedWidth;
            skillCatalog.fixedHeight = GUI.skin.box.fixedHeight;

            for (int i = 0; i < activeItemTemplates.Count; i++)
            {
                bool isSelected = (selectedActiveSkillIndex == i);

                var text = "  " + activeItemTemplates[i].Item1.displayName;
                text = text.Substring(0, Mathf.Min(text.Length, 13));
                GUIContent content = new GUIContent(text, activeItemTemplates[i].Item2);

                if (GUILayout.Toggle(isSelected, content, skillCatalog))
                {
                    if (selectedActiveItemIndex != i)
                    {
                        selectedActiveItemIndex = i;

                        GUI.FocusControl(null);
                    }
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));

            if (activeItemTemplates.Count > 0 && selectedActiveItemIndex < activeItemTemplates.Count)
            {
                ActiveItemTemplate selectedItem = activeItemTemplates[selectedActiveItemIndex].Item1;

                if (activeItemEditor == null || activeItemEditor.target != selectedItem)
                {
                    activeItemEditor = UnityEditor.Editor.CreateEditor(selectedItem);
                }

                contentScrollPosition = GUILayout.BeginScrollView(contentScrollPosition, false, false);
                activeItemEditor.OnInspectorGUI();
                GUILayout.EndScrollView();
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void AddActiveItemTemplate()
        {
            // ������ ���ø� ����
            ActiveItemTemplate newItem = CreateInstance<ActiveItemTemplate>();

            // ���� ����
            string defaultPath = "Assets/FrameWork/Core/GameData/Item/ActiveItem";
            string path = EditorUtility.SaveFilePanelInProject("FrameWork/Core/GameData/Item/ActiveItem", "ActiveItem_", "asset", "������ ���ø��� FrameWork/Core/GameData/Item/ActiveItem ��ġ�� ����˴ϴ�..", defaultPath);
            if (!string.IsNullOrEmpty(path))
            {
                newItem.SetDisplayName(Path.GetFileNameWithoutExtension(path).Replace("ActiveItem_", ""));

                AssetDatabase.CreateAsset(newItem, path);
                AssetDatabase.SaveAssets();
                LoadActiveItemTemplates();
            }
        }

        private void DeleteSelectedActiveItemTemplate()
        {
            if (activeItemTemplates.Count > 0)
            {
                ActiveItemTemplate selectedItem = activeItemTemplates[selectedActiveItemIndex].Item1;
                string assetPath = AssetDatabase.GetAssetPath(selectedItem);
                activeItemTemplates.RemoveAt(selectedActiveItemIndex);
                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.SaveAssets();
            }
        }

        private void LoadActiveItemTemplates()
        {
            if (emptyTexture2D == null) emptyTexture2D = CreateTexture(Color.gray);

            activeItemTemplates.Clear();
            string[] guids = AssetDatabase.FindAssets("t:ActiveItemTemplate");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ActiveItemTemplate item = AssetDatabase.LoadAssetAtPath<ActiveItemTemplate>(path);

                var texture = (item.sprite == null) ? emptyTexture2D : item.sprite.texture;
                texture = texture.ResizeTexture(30, 30);

                activeItemTemplates.Add(new Tuple<ActiveItemTemplate, Texture2D>(item, texture));
            }
        }
        #endregion

        #region �нú� ��ų
        private void DrawPassiveItemTab()
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(GUILayout.Width(200));
            if (GUILayout.Button("������ �߰�"))
            {
                AddPassiveItemTemplate();
            }
            if (GUILayout.Button("������ ����"))
            {
                DeleteSelectedPassiveItemTemplate();
            }
            if (GUILayout.Button("������ Ž��"))
            {
                LoadPassiveItemTemplates();
            }

            DrawLine();

            passiveItemScrollPosition = GUILayout.BeginScrollView(passiveItemScrollPosition, false, true);

            var skillCatalog = new GUIStyle(GUI.skin.button);
            skillCatalog.alignment = TextAnchor.MiddleLeft;
            skillCatalog.padding = new RectOffset(5, 5, 5, 5);
            skillCatalog.margin = new RectOffset(5, 5, -2, -2);
            skillCatalog.border = new RectOffset(0, 0, 0, 0);
            skillCatalog.fixedWidth = GUI.skin.box.fixedWidth;
            skillCatalog.fixedHeight = GUI.skin.box.fixedHeight;

            for (int i = 0; i < passiveItemTemplates.Count; i++)
            {
                bool isSelected = (selectedPassiveItemIndex == i);

                var text = "  " + passiveItemTemplates[i].Item1.displayName;
                text = text.Substring(0, Mathf.Min(text.Length, 13));
                GUIContent content = new GUIContent(text, passiveItemTemplates[i].Item2);

                if (GUILayout.Toggle(isSelected, content, skillCatalog))
                {
                    if (selectedPassiveItemIndex != i)
                    {
                        selectedPassiveItemIndex = i;

                        GUI.FocusControl(null);
                    }
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));

            if (passiveItemTemplates.Count > 0 && selectedPassiveItemIndex < passiveItemTemplates.Count)
            {
                PassiveItemTemplate selectedItem = passiveItemTemplates[selectedPassiveItemIndex].Item1;

                if (passiveItemEditor == null || passiveItemEditor.target != selectedItem)
                {
                    passiveItemEditor = UnityEditor.Editor.CreateEditor(selectedItem);
                }

                contentScrollPosition = GUILayout.BeginScrollView(contentScrollPosition, false, false);
                passiveItemEditor.OnInspectorGUI();
                GUILayout.EndScrollView();
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void AddPassiveItemTemplate()
        {
            // ������ ���ø� ����
            PassiveItemTemplate newItem = CreateInstance<PassiveItemTemplate>();

            // ���� ����
            string defaultPath = "Assets/FrameWork/Core/GameData/Item/PassiveItem";
            string path = EditorUtility.SaveFilePanelInProject("FrameWork/Core/GameData/Item/PassiveItem", "PassiveItem_", "asset", "��ų ���ø��� FrameWork/Core/GameData/Item/PassiveItem ��ġ�� ����˴ϴ�..", defaultPath);
            if (!string.IsNullOrEmpty(path))
            {
                newItem.SetDisplayName(Path.GetFileNameWithoutExtension(path).Replace("PassiveItem_", ""));

                AssetDatabase.CreateAsset(newItem, path);
                AssetDatabase.SaveAssets();
                LoadPassiveItemTemplates();
            }
        }

        private void DeleteSelectedPassiveItemTemplate()
        {
            if (passiveItemTemplates.Count > 0)
            {
                PassiveItemTemplate selectedItem = passiveItemTemplates[selectedPassiveItemIndex].Item1;
                string assetPath = AssetDatabase.GetAssetPath(selectedItem);
                passiveItemTemplates.RemoveAt(selectedPassiveItemIndex);
                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.SaveAssets();
            }
        }

        private void LoadPassiveItemTemplates()
        {
            if (emptyTexture2D == null) emptyTexture2D = CreateTexture(Color.gray);

            passiveItemTemplates.Clear();
            string[] guids = AssetDatabase.FindAssets("t:PassiveItemTemplate");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                PassiveItemTemplate item = AssetDatabase.LoadAssetAtPath<PassiveItemTemplate>(path);

                var texture = (item.sprite == null) ? emptyTexture2D : item.sprite.texture;
                texture = texture.ResizeTexture(30, 30);

                passiveItemTemplates.Add(new Tuple<PassiveItemTemplate, Texture2D>(item, texture));
            }
        }
        #endregion

        #region ��ƿ��Ƽ
        private void DrawLine()
        {
            GUILayout.Space(5);
            Rect rect = GUILayoutUtility.GetRect(0, 1);
            EditorGUI.DrawRect(rect, Color.gray);
            GUILayout.Space(5);
        }

        Texture2D CreateTexture(Color color)
        {
            Texture2D texture = new Texture2D(64, 64);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
        #endregion
    }
}
