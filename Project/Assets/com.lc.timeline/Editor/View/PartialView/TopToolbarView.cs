using LCToolkit;
using System;
using UnityEditor;
using UnityEngine;

namespace LCTimeline.View
{
    /// <summary>
    /// 顶部工具栏
    /// </summary>
    public class TopToolbarView : BaseView
    {
        #region 播放操作UI元素

        private GUIContent PlayContent;

        private GUIContent GotoBeginingContent;

        private GUIContent GotoEndContent;

        private GUIContent NextFrameContent;

        private GUIContent PreviousFrameContent;

        #endregion 播放操作UI元素

        #region 按钮列表

        private GUIContent NewContent;
        private GUIContent OpenContent;
        private GUIContent SaveContent;
        private GUIContent SettingContent;

        #endregion 按钮列表

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

            NewContent = new GUIContent(TimelineStyle.LoadEdStyleImg("btn_editor_new.png"), "新建.");
            OpenContent = new GUIContent(TimelineStyle.LoadEdStyleImg("btn_editor_open.png"), "打开.");
            SaveContent = new GUIContent(TimelineStyle.LoadEdStyleImg("btn_editor_save.png"), "保存.");
            SettingContent = new GUIContent(TimelineStyle.LoadEdStyleImg("SettingsIcon.png"), "设置.");
        }

        public override void OnDraw()
        {
            GUILayout.BeginHorizontal();

            GUILayoutExtension.HorizontalGroup(() => {
                DrawPlayOperate();
                GUILayout.FlexibleSpace();
                DrawSkillLineTime();
            }, EditorStylesExtension.NullStyle, GUILayout.Width(HorWidth), GUILayout.Height(window.ToolbarSize.height));

            GUILayout.FlexibleSpace();
            GUILayout.Label(GetTimelineName());
            GUILayout.FlexibleSpace();

            GUILayoutExtension.HorizontalGroup(() => {
                DrawBtnList();
            }, EditorStylesExtension.NullStyle,GUILayout.Width(HorWidth), GUILayout.Height(window.ToolbarSize.height));

            GUILayout.EndHorizontal();
        }

        //绘制播放操作
        private void DrawPlayOperate()
        {
            //最前
            if (GUILayout.Button(GotoBeginingContent, EditorStyles.toolbarButton))
            {
            }
            //上一帧
            if (GUILayout.Button(PreviousFrameContent, EditorStyles.toolbarButton))
            {
            }
            //播放
            if (GUILayout.Button(PlayContent, EditorStyles.toolbarButton))
            {
                window.SetSkillEdPlay(!window.IsPlayingSkill);
            }
            //下一帧
            if (GUILayout.Button(NextFrameContent, EditorStyles.toolbarButton))
            {
            }
            //最后
            if (GUILayout.Button(GotoEndContent, EditorStyles.toolbarButton))
            {
            }
        }

        //绘制技能时间
        private void DrawSkillLineTime()
        {
            string timeValue;
            if (window.TimeInFrames)
                timeValue = TimeUtility.TimeAsFrames(window.RunningTime, (double)window.FrameRate, "F2");
            else
                timeValue = TimeUtility.TimeAsTimeCode(window.RunningTime, (double)window.FrameRate, "F2");
            GUILayout.Label(timeValue);
        }

        //按钮列表
        private void DrawBtnList()
        {
            if (GUILayout.Button(SettingContent, EditorStyles.toolbarButton))
            {
                DrawSettingPanel();
                GUIUtility.ExitGUI();
            }
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

        public string GetTimelineName()
        {
            BaseSequenceView sequenceView = window.GetPartialView<BaseSequenceView>();
            if (sequenceView == null)
                return "";
            return sequenceView.Timeline.Name;
        }
    } 
}