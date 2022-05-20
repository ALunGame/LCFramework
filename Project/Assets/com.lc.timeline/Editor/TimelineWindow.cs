using LCTimeline.View;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace LCTimeline
{
    public class TimelineWindow : EditorWindow
    {
        //�ϲ㹤�ߴ���
        public readonly Rect ToolbarSize = new Rect(0, 0, 0, 10);

        //��ӹ������
        public readonly Rect AddTrackSize = new Rect(0, 0, 200, 10);

        //����б���
        public readonly Rect TrackListSize = new Rect(0, 50, 0, 0);

        public readonly Rect LeftTimeAreaSize = new Rect(220, 20, 0, 24);
        public readonly Rect RightTimeAreaSize = new Rect(0, 10, 280, 40);

        private List<BaseView> PartialViews = new List<BaseView>() { new TopToolbarView(), new TimeAreaView(), new AddTrackView(), new BaseSequenceView() };

        public Rect WinArea { get; set; }

        public bool IsPlayingSkill = false;
        public bool IsLockDragHeaderArrow = false;      //�������
        public double CutOffTime;                       //���ʱ��

        private double lastUpdateTime;

        private double runningTime;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public double RunningTime
        {
            get
            {
                return runningTime;
            }
            set
            {
                runningTime = value;
                for (int i = 0; i < PartialViews.Count; i++)
                {
                    PartialViews[i].SyncRunningTime(runningTime);
                }
            }
        }

        private bool timeInFrames = true;

        /// <summary>
        /// ֡ģʽ
        /// </summary>
        public bool TimeInFrames
        {
            get
            {
                return timeInFrames;
            }
            set
            {
                timeInFrames = value;
            }
        }


        private float frameRate = 30f;
        /// <summary>
        /// ֡��
        /// </summary>
        public float FrameRate
        {
            get
            {
                return frameRate;
            }
            set
            {
                frameRate = value;
            }
        }

        private bool frameSnap = true;
        /// <summary>
        /// ֡����
        /// </summary>
        public bool FrameSnap
        {
            get
            {
                return frameSnap;
            }
            set
            {
                frameSnap = value;
            }
        }

        private bool edgeSnaps = true;
        /// <summary>
        /// ��ק����
        /// </summary>
        public bool EdgeSnaps
        {
            get
            {
                return edgeSnaps;
            }
            set
            {
                edgeSnaps = value;
            }
        }

        protected UnityObject graphAsset;
        public UnityObject GraphAsset
        {
            get { return graphAsset; }
            protected set { graphAsset = value; }
        }

        /// <summary>
        /// ��ͼ
        /// </summary>
        public BaseTimelineGraph Graph
        {
            get;
            private set;
        }

        /// <summary>
        /// ������Timeline�ص�
        /// </summary>
        public virtual Action<BaseTimelineGraph> OnSaveTimeline { get; }

        private void Load(ITimelineGraphAsset graphAsset)
        {
            if (Graph != null)
            {
                if (Graph.Go != null)
                {
                    GameObject.DestroyImmediate(Graph.Go);
                }
            }
            GraphAsset = graphAsset as UnityObject;
            Graph      = graphAsset.DeserializeGraph();
            CreateTimelineGo();
            for (int i = 0; i < PartialViews.Count; i++)
            {
                PartialViews[i].Init(this, Graph);
            }
        }

        private void CreateTimelineGo()
        {
            UnityObject obj = ((InternalTimelineGraphAsset)GraphAsset).DisplayGo;
            if (obj != null)
            {
                Graph.Go = (GameObject)GameObject.Instantiate(obj);
            }
        }

        public void OnEnable()
        {
            EditorApplication.update = (EditorApplication.CallbackFunction)System.Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(OnEditorUpdate));
            lastUpdateTime = (float)EditorApplication.timeSinceStartup;
        }

        public void OnGUI()
        {
            if (PartialViews == null)
                return;
            WinArea = position;
            for (int i = 0; i < PartialViews.Count; i++)
            {
                PartialViews[i].OnDraw();
            }

            for (int i = 0; i < PartialViews.Count; i++)
            {
                PartialViews[i].OnHandleEvent(Event.current);
            }
            OnHandleEvent(Event.current);
        }

        public void OnHandleEvent(Event evt)
        {
            if (evt == null)
                return;
            //����Ctrl+S
            if (Event.current.Equals(Event.KeyboardEvent("^S")))
            {
                SaveAsset();
                Event.current.Use();
            }
        }

        public void OnDestroy()
        {
            for (int i = 0; i < PartialViews.Count; i++)
            {
                PartialViews[i].OnDestroy();
            }
            EditorApplication.update = (EditorApplication.CallbackFunction)System.Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(OnEditorUpdate));
        }

        private void OnEditorUpdate()
        {
            if (!Application.isPlaying && IsPlayingSkill)
            {
                float delta = 1.0f / 30;
                double fTime = (float)EditorApplication.timeSinceStartup - lastUpdateTime;

                if (fTime > delta)
                {
                    RunningTime += delta;
                    BaseSequenceView sequenceView = GetPartialView<BaseSequenceView>();
                    BaseTimelineGraph sequenceData = sequenceView.Timeline;
                    if (RunningTime >= sequenceData.DurationTime)
                    {
                        RunningTime = 0;
                        //IsPlayingSkill = false;
                    }
                    lastUpdateTime = (float)EditorApplication.timeSinceStartup;
                }
            }
            Repaint();
        }

        private void SaveAsset()
        {
            if (GraphAsset is ITimelineGraphAsset graphAsset)
                graphAsset.SaveGraph(Graph);
            EditorUtility.SetDirty(GraphAsset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #region Virtual

        public virtual void OnInit()
        {
        }

        #endregion Virtual

        #region �ӿ�

        public void AddPartialView(BaseView skillView)
        {
            skillView.Init(this, Graph);
            PartialViews.Add(skillView);
        }

        public T GetPartialView<T>() where T : BaseView
        {
            for (int i = 0; i < PartialViews.Count; i++)
            {
                if (PartialViews[i] is T)
                {
                    return (T)PartialViews[i];
                }
            }
            return null;
        }

        public void RemovePartialView<T>() where T : BaseView
        {
            for (int i = 0; i < PartialViews.Count; i++)
            {
                if (PartialViews[i] is T)
                {
                    PartialViews.RemoveAt(i);
                }
            }
        }

        //���ò���״̬
        public void SetSkillEdPlay(bool reqPlay)
        {
            if (IsPlayingSkill == reqPlay)
                return;

            IsPlayingSkill = reqPlay;
        }

        #endregion

        #region Static

        /// <summary> ��Graph���ͻ�ȡ��Ӧ��GraphWindow </summary>
        public static TimelineWindow GetGraphWindow(Type graphType)
        {
            var windowType = TimelineEditorUtility.GetGraphWindowType(graphType);
            UnityObject[] objs = Resources.FindObjectsOfTypeAll(windowType);
            TimelineWindow window = null;
            foreach (var obj in objs)
            {
                if (obj.GetType() == windowType)
                {
                    window = obj as TimelineWindow;
                    break;
                }
            }
            if (window == null)
            {
                window = GetWindow(windowType) as TimelineWindow;
            }
            window.Focus();
            return window;
        }

        /// <summary> ��GraphAsset��Graph </summary>
        public static TimelineWindow Open(ITimelineGraphAsset graphAsset)
        {
            if (graphAsset == null) return null;
            var window = GetGraphWindow(graphAsset.GraphType);
            window.Load(graphAsset);
            return window;
        }

        /// <summary> ˫����Դ </summary>
        [OnOpenAsset(0)]
        public static bool OnOpen(int instanceID, int line)
        {
            UnityObject go = EditorUtility.InstanceIDToObject(instanceID);
            if (go == null) return false;
            ITimelineGraphAsset graphAsset = go as ITimelineGraphAsset;
            if (graphAsset == null)
                return false;
            Open(graphAsset);
            return true;
        }

        #endregion
    }
}