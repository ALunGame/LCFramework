using LCToolkit;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LCTimeline.View
{
    public enum EDSequencePlayMode
    {
        EditorPause,
        EditorRun,
    }

    /// <summary>
    /// 序列视图（一个序列包含多个轨道）
    /// </summary>
    public class BaseSequenceView : BaseView
    {
        private static Color ColorBg = new Color(0.66f, 0.66f, 0.66f, 0.1f);

        //序列最后标识线
        private static Color ColorEndLine = new Color(0.8f, 0.1f, 0, 0.5f);

        #region 字段

        public BaseTimelineGraph Timeline;

        #endregion

        public EDSequencePlayMode PlayMode;
        public List<BaseTrackView> Tracklist = new List<BaseTrackView>();
        public Rect SequenceViewRect;

        //轨道最下方坐标
        public float TracksBottommY
        {
            get
            {
                if (Tracklist != null && Tracklist.Count > 0)
                {
                    BaseTrackView track = Tracklist.Last();
                    return track.TrackViewRect.y + track.TrackViewRect.height;
                }
                return window.TrackListSize.y;
            }
        }

        public override void OnInit()
        {
            Timeline = Model;
            for (int i = 0; i < Timeline.Tracks.Count; i++)
            {
                AddTrackView(Timeline.Tracks[i]);
            }
        }

        public override void OnDraw()
        {
            //显示
            SequenceViewRect = window.TrackListSize;
            SequenceViewRect.height = window.position.height - window.TrackListSize.y;
            SequenceViewRect.width = window.position.width - window.TrackListSize.x;

            //绘制背景
            EditorGUI.DrawRect(SequenceViewRect, ColorBg);

            //绘制所有轨道
            EditorGUILayout.BeginVertical(GUILayout.Width(SequenceViewRect.height));
            if (Tracklist != null && Tracklist.Count > 0)
            {
                for (int i = 0; i < Tracklist.Count; i++)
                {
                    Tracklist[i].OnDraw();
                    GUILayout.Space(BaseTrackView.TrackOffset);
                }
            }
            EditorGUILayout.EndVertical();

            //同步数据
            SyncSequenceData();

            //绘制结束标识线
            DrawEndLine();
        }

        //绘制结束标识线
        private void DrawEndLine()
        {
            TimeAreaView timeAreaView = window.GetPartialView<TimeAreaView>();
            float x = timeAreaView.TimeToPixel(Timeline.DurationTime);
            if (Timeline.DurationTime > 1e-1 && x > 200)
            {
                Rect rec = new Rect(x, timeAreaView.TimeContent.y, 1,
                    TracksBottommY - timeAreaView.TimeContent.y - 2);
                EditorGUI.DrawRect(rec, ColorEndLine);
            }
        }

        public override void OnHandleEvent(Event evt)
        {
            Vector2 mousePos = evt.mousePosition;
            switch (evt.type)
            {
                case EventType.MouseDown:
                    if (SequenceViewRect.Contains(mousePos))
                        OnSelect();
                    break;

                default:
                    break;
            }
            for (int i = 0; i < Tracklist.Count; i++)
            {
                Tracklist[i].OnHandleEvent(evt);
            }
        }

        public override void OnRunningTimeChange(double runningTime)
        {
            for (int i = 0; i < Tracklist.Count; i++)
            {
                Tracklist[i].OnRunningTimeChange(runningTime);
            }
        }

        public virtual void OnSelect()
        {
            InspectorExtension.DrawObjectInInspector(Timeline);
        }

        //同步数据
        private void SyncSequenceData()
        {
            Timeline.DurationTime = CalcSequenceDurationTime();
        }

        private void RefreshTrackIndex()
        {
            for (int i = 0; i < Tracklist.Count; i++)
            {
                Tracklist[i].TrackIndex = i;
            }
        }

        private float CalcSequenceDurationTime()
        {
            float dur = 0;
            for (int i = 0; i < Tracklist.Count; i++)
            {
                BaseTrackView trackView = Tracklist[i];
                trackView.Cliplist.ForEach((clip) =>
                {
                    ClipModel clipData = clip.Clip;
                    if (clipData.EndTime > dur)
                        dur = clipData.EndTime;
                });
            }
            return dur;
        }

        private void AddTrackView(TrackModel trackData)
        {
            BaseTrackView trackView = TimelineViewHelper.GetTrackView(trackData);
            trackView.Init(window, Model);
            trackView.Sequence = this;
            Tracklist.Add(trackView);
            RefreshTrackIndex();
        }

        #region 接口

        public void AddTrack(TrackModel trackData)
        {
            Timeline.Tracks.Add(trackData);
            AddTrackView(trackData);
        }

        public void RemoveTrack(BaseTrackView track)
        {
            for (int i = 0; i < Tracklist.Count; i++)
            {
                if (Tracklist[i].Equals(track))
                {
                    Tracklist.RemoveAt(i);
                    break;
                }
            }
            Timeline.Tracks.Remove(track.Track);    
            RefreshTrackIndex();
        }

        #endregion 接口
    }
}