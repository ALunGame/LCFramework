using LCHelp;
using LCTimeline.Serialize;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LCTimeline.View
{
    /// <summary>
    /// ����������
    /// </summary>
    public class TopToolbarView : BaseView
    {
        #region ���Ų���UIԪ��

        private GUIContent PlayContent;

        private GUIContent GotoBeginingContent;

        private GUIContent GotoEndContent;

        private GUIContent NextFrameContent;

        private GUIContent PreviousFrameContent;

        #endregion ���Ų���UIԪ��

        #region ��ť�б�

        private GUIContent NewContent;
        private GUIContent OpenContent;
        private GUIContent SaveContent;
        private GUIContent SettingContent;

        #endregion ��ť�б�

        private const float HorWidth = 200;

        public Action OnClickNewFileFunc = null;
        public Action OnClickOpenFileFunc = null;
        public Action OnClickSaveFileFunc = null;

        public override void OnInit()
        {
            PlayContent = EditorGUIUtility.TrIconContent("Animation.Play", "Play the timeline");
            GotoBeginingContent = EditorGUIUtility.TrIconContent("Animation.FirstKey", "Go to the beginning of the timeline");
            GotoEndContent = EditorGUIUtility.TrIconContent("Animation.LastKey", "Go to the end of the timeline");
            NextFrameContent = EditorGUIUtility.TrIconContent("Animation.NextKey", "Go to the next frame");
            PreviousFrameContent = EditorGUIUtility.TrIconContent("Animation.PrevKey", "Go to the previous frame");

            NewContent = new GUIContent(TimelineStyle.LoadEdStyleImg("btn_editor_new.png"), "�½�.");
            OpenContent = new GUIContent(TimelineStyle.LoadEdStyleImg("btn_editor_open.png"), "��.");
            SaveContent = new GUIContent(TimelineStyle.LoadEdStyleImg("btn_editor_save.png"), "����.");
            SettingContent = new GUIContent(TimelineStyle.LoadEdStyleImg("SettingsIcon.png"), "����.");
        }

        public override void OnDraw()
        {
            GUILayout.BeginHorizontal();

            EDLayout.CreateHorizontal("", HorWidth, window.ToolbarSize.height, () =>
            {
                DrawPlayOperate();
                GUILayout.FlexibleSpace();

                DrawSkillLineTime();
            });

            GUILayout.FlexibleSpace();
            GUILayout.Label(GetTimelineName());
            GUILayout.FlexibleSpace();

            EDLayout.CreateHorizontal("", HorWidth, window.ToolbarSize.height, () =>
            {
                DrawBtnList();
            });

            GUILayout.EndHorizontal();
        }

        //���Ʋ��Ų���
        private void DrawPlayOperate()
        {
            //��ǰ
            if (GUILayout.Button(GotoBeginingContent, EditorStyles.toolbarButton))
            {
            }
            //��һ֡
            if (GUILayout.Button(PreviousFrameContent, EditorStyles.toolbarButton))
            {
            }
            //����
            if (GUILayout.Button(PlayContent, EditorStyles.toolbarButton))
            {
                window.SetSkillEdPlay(!window.IsPlayingSkill);
            }
            //��һ֡
            if (GUILayout.Button(NextFrameContent, EditorStyles.toolbarButton))
            {
            }
            //���
            if (GUILayout.Button(GotoEndContent, EditorStyles.toolbarButton))
            {
            }
        }

        //���Ƽ���ʱ��
        private void DrawSkillLineTime()
        {
            string timeValue;
            if (window.TimeInFrames)
                timeValue = TimeUtility.TimeAsFrames(window.RunningTime, (double)window.FrameRate, "F2");
            else
                timeValue = TimeUtility.TimeAsTimeCode(window.RunningTime, (double)window.FrameRate, "F2");
            GUILayout.Label(timeValue);
        }

        //��ť�б�
        private void DrawBtnList()
        {
            if (GUILayout.Button(NewContent, EditorStyles.toolbarButton))
            {
                EDPopPanel.PopWindow("����������", (string name) =>
                {
                    window.CreateTimeline(name);
                });
                OnClickNewFileFunc?.Invoke();
                GUIUtility.ExitGUI();
            }

            if (GUILayout.Button(OpenContent, EditorStyles.toolbarButton))
            {
                DrawTimelineList();
                OnClickOpenFileFunc?.Invoke();
                GUIUtility.ExitGUI();
            }

            if (GUILayout.Button(SaveContent, EditorStyles.toolbarButton))
            {
                BaseSequenceView sequenceView = window.GetPartialView<BaseSequenceView>();
                if (sequenceView != null)
                {
                    TimelineSerialize.Save((TimelineData)sequenceView.Data, window.SavePath);
                    window.OnSaveTimeline?.Invoke((TimelineData)sequenceView.Data);
                    AssetDatabase.Refresh();
                }

                OnClickSaveFileFunc?.Invoke();
                GUIUtility.ExitGUI();
            }

            if (GUILayout.Button(SettingContent, EditorStyles.toolbarButton))
            {
                DrawSettingPanel();
                GUIUtility.ExitGUI();
            }
        }

        private void DrawTimelineList()
        {
            List<string> filePathList = GetTimlineFilePaths();

            GenericMenu pm = new GenericMenu();

            for (int i = 0; i < filePathList.Count; i++)
            {
                string filePath = filePathList[i];
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                var paste = EditorGUIUtility.TrTextContent(fileName);
                pm.AddItem(paste, false, () =>
                {
                    window.LoadTimeline(filePath);
                });
            }
            Rect rect = new Rect(Event.current.mousePosition, new Vector2(200, 0));
            pm.DropDown(rect);
        }

        private void ChangeTimeCode(object obj)
        {
            string a = obj.ToString();
            if (a == "frames")
            {
                window.TimeInFrames = true;
            }
            else
            {
                window.TimeInFrames = false;
            }
        }

        private void DrawSettingPanel()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Seconds"), !window.TimeInFrames, new GenericMenu.MenuFunction2(this.ChangeTimeCode), "seconds");
            genericMenu.AddItem(new GUIContent("Frames"), window.TimeInFrames, new GenericMenu.MenuFunction2(this.ChangeTimeCode), "frames");
            genericMenu.AddSeparator("");
            genericMenu.AddDisabledItem(new GUIContent("Frame rate"));
            genericMenu.AddItem(new GUIContent("Film (24)"), window.FrameRate.Equals(24f), delegate (object r)
            {
                window.FrameRate = (float)r;
            }, 24f);
            genericMenu.AddItem(new GUIContent("30"), window.FrameRate.Equals(30f), delegate (object r)
            {
                window.FrameRate = (float)r;
            }, 30f);
            genericMenu.AddItem(new GUIContent("50"), window.FrameRate.Equals(50f), delegate (object r)
            {
                window.FrameRate = (float)r;
            }, 50f);
            genericMenu.AddItem(new GUIContent("60"), window.FrameRate.Equals(60f), delegate (object r)
            {
                window.FrameRate = (float)r;
            }, 60f);
            genericMenu.AddDisabledItem(new GUIContent("Custom"));
            genericMenu.AddSeparator("");
            genericMenu.AddItem(new GUIContent("Snap to Frame"), window.FrameSnap, delegate
            {
                window.FrameSnap = !window.FrameSnap;
            });
            genericMenu.AddItem(new GUIContent("Edge Snap"), window.EdgeSnaps, delegate
            {
                window.EdgeSnaps = !window.EdgeSnaps;
            });

            OnCreateSettingContent(genericMenu);
            genericMenu.ShowAsContext();
        }

        protected virtual void OnCreateSettingContent(GenericMenu menu)
        {

        }

        public List<string> GetTimlineFilePaths()
        {
            List<string> filePathList = new List<string>();
            filePathList.AddRange(Directory.GetFiles(window.SavePath, "*" + TimelineSerialize.TimelineAssetExNam, SearchOption.AllDirectories));
            return filePathList;
        }

        public bool CheckHasTimeline(string name)
        {
            List<string> filePathList = GetTimlineFilePaths();
            for (int i = 0; i < filePathList.Count; i++)
            {
                string filePath = filePathList[i];
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                if (fileName == name)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetTimelineName()
        {
            BaseSequenceView sequenceView = window.GetPartialView<BaseSequenceView>();
            if (sequenceView == null)
                return "";
            return ((TimelineData)sequenceView.Data).Name;
        }
    } 
}