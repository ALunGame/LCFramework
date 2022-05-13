using LCToolkit;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using LCJson;

namespace LCTimeline.View
{
    /// <summary>
    /// 轨道视图（一个轨道包含多个片段）
    /// </summary>
    public class BaseTrackView : BaseView
    {
        #region Static

        public static float TrackOffset = 3;
        private static float TrackHeight = 30;
        private static float TrackHeadWidth = 200;

        #endregion Static

        #region 字段

        public TrackModel Track;

        #endregion

        public Rect TrackViewRect => new Rect(Sequence.SequenceViewRect.x, Sequence.SequenceViewRect.y + ((TrackHeight + TrackOffset) * (TrackIndex)), window.position.width, TrackHeight);

        public Rect TrackHeadViewRect => new Rect(Sequence.SequenceViewRect.x, Sequence.SequenceViewRect.y + ((TrackHeight + TrackOffset) * (TrackIndex)), TrackHeadWidth, TrackHeight);

        #region Virtual

        public Color UnSelectColor = Color.white;

        public Color SelectColor = Color.green;

        public Color DisplayNameColor = Color.black;

        public virtual void OnDrawTrack()
        {
            GUI.color = DisplayNameColor;
            EditorGUILayout.LabelField(Track.TitleName, EditorStylesExtension.MiddleLabelStyle, GUILayout.Height(TrackHeadViewRect.height));
            GUI.color = Color.white;
        }

        public virtual void OnSelect()
        {
            InspectorExtension.DrawObjectInInspector(Track);
        }

        #endregion Virtual

        public int TrackIndex;
        public BaseSequenceView Sequence;
        public List<BaseClipView> Cliplist = new List<BaseClipView>();

        private bool IsSelected = false;

        /// <summary>
        /// 右键菜单
        /// </summary>
        private void GenRightClickMenu()
        {
            GenericMenu pm = new GenericMenu();

            //删除
            var delPaste = EditorGUIUtility.TrTextContent("删除");
            pm.AddItem(delPaste, false, () =>
            {
                Sequence.RemoveTrack(this);
            });

            //复制
            var copyPaste = EditorGUIUtility.TrTextContent("复制");
            pm.AddItem(copyPaste, false, () =>
            {
                CopyView();
            });

            string clipMenyName = Track.ClipType.Name;
            if (AttributeHelper.TryGetTypeAttribute(Track.ClipType, out ClipMenuAttribute attr))
            {
                clipMenyName = attr.MenuName;
            }
            var paste = EditorGUIUtility.TrTextContent(clipMenyName);
            pm.AddItem(paste, false, () =>
            {
                CreateClipView(Track.ClipType);
            });

            Rect rect = new Rect(Event.current.mousePosition, new Vector2(200, 0));
            pm.DropDown(rect);
        }

        public override void OnInit()
        {
            if (AttributeHelper.TryGetTypeAttribute(Track.GetType(), out TrackColorAttribute attr))
            {
                UnSelectColor = attr.UnSelectColor;
                SelectColor = attr.SelectColor;
                DisplayNameColor = attr.DisplayNameColor;
            }
            for (int i = 0; i < Track.Clips.Count; i++)
            {
                AddClipView(Track.Clips[i]);
            }
        }

        public override void OnDraw()
        {
            //绘制轨道选择状态
            var backgroundColor = IsSelected ? SelectColor : UnSelectColor;
            EditorGUI.DrawRect(TrackViewRect, backgroundColor);

            //绘制轨道按钮
            GUILayout.BeginArea(TrackHeadViewRect);
            OnDrawTrack();
            GUILayout.EndArea();

            //绘制轨道按钮边框
            MiscHelper.DrawOutline(TrackHeadViewRect, 2, Color.gray);

            //绘制片段
            for (int i = 0; i < Cliplist.Count; i++)
            {
                Cliplist[i].OnDraw();
            }
        }

        public override void OnHandleEvent(Event evt)
        {
            Vector2 mousePos = evt.mousePosition;
            switch (evt.type)
            {
                case EventType.MouseDown:
                    if (TrackViewRect.Contains(mousePos))
                    {
                        IsSelected = true;
                        OnSelect();
                        if (Event.current.button == 1)
                        {
                            GenRightClickMenu();
                        }
                    }
                    else
                    {
                        IsSelected = false;
                    }

                    break;

                default:
                    break;
            }

            for (int i = 0; i < Cliplist.Count; i++)
            {
                Cliplist[i].OnHandleEvent(evt);
            }
        }

        public override void OnRunningTimeChange(double runningTime)
        {
            for (int i = 0; i < Cliplist.Count; i++)
            {
                Cliplist[i].OnRunningTimeChange(runningTime);
            }
        }

        public virtual void OnAddClipView(BaseClipView clipView)
        {
        }


        private void CreateClipView(Type clipType)
        {
            ClipModel clipModel = ReflectionHelper.CreateInstance(clipType) as ClipModel;
            clipModel.StartTime = 0;
            clipModel.EndTime = 1;
            clipModel.DurationTime = clipModel.EndTime - clipModel.StartTime;
            AddClip(clipModel);
        }

        public void AddClip(ClipModel clipModel)
        {
            Track.Clips.Add(clipModel);
            AddClipView(clipModel);
        }

        private void AddClipView(ClipModel clipData)
        {
            BaseClipView clipView = TimelineViewHelper.GetClipView(clipData);
            clipView.Init(window, Model);
            clipView.Track = this;
            Cliplist.Add(clipView);
            OnAddClipView(clipView);
        }

        private void CopyView()
        {
            string jsonStr = JsonMapper.ToJson(Track);
            TrackModel newTrack = JsonMapper.ToObject<TrackModel>(jsonStr);
            Sequence.AddTrack(newTrack);
        }

        public void RemoveClipView(BaseClipView clipView)
        {
            for (int i = 0; i < Cliplist.Count; i++)
            {
                if (Cliplist[i].Equals(clipView))
                {
                    Cliplist.RemoveAt(i);
                    break;
                }
            }
            Track.Clips.Remove(clipView.Clip);
        }
    }
}