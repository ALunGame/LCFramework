using System;
using System.Collections.Generic;
using IAEngine;
using LCToolkit.Command;
using LCToolkit.Element;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LCSkill.Timeline
{
    public class SkillTimelineWindow : EditorWindow
    {
        public const float FrameDeltaTime = 1.0f / 30;
        public const int MaxFrameCnt = 500;
        
        private const string UXMLPath = "Assets/com.lc.skill/Editor/Timeline/UIAsset/SkillEditorWindow.uxml";
        public static SkillTimelineAsset TimelineAsset;
        public static void Open(SkillTimelineAsset pAsset)
        {
            SkillTimelineWindow.TimelineAsset = pAsset;
            SkillTimelineWindow wnd = GetWindow<SkillTimelineWindow>();
            wnd.titleContent = new GUIContent("SkillTimeline");
            wnd.ReloadUI();
        }
        
        public BaseTimeline Model { get; private set; }
        public GameObject PreviewGo { get; set; }
        
        public CommandDispatcher CommandDispatcher
        {
            get;
            protected set;
        }
        
        public int CurrFrameCnt { get; private set; }
        public event Action OnCurrFrameChange;

        public int TotalFrameCnt
        {
            get
            {
                return Model.totalFrame;
            }
        }

        public event Action OnTotalFrameChange;
        
        public bool IsPlaying { get; private set; }
        
        public TopPartialView Top { get; private set; }
        public MiddlePartialView Middle { get; private set; }
        public BottomPartialView Bottom { get; private set; }
        public BottomScrollPartialView BottomScroll { get; private set; }
        public RightScrollPartialView RightScroll { get; private set; }
        public Frame_Arrow FrameArrow { get; private set; }
        
        private double lastUpdateTime;

        public event Action OnFocuseWindow;
        private EditorWindow lastFocuseWindow;

        public void CreateGUI()
        {
        }

        public void ReloadUI()
        {
            if (PreviewGo!=null)
            {
                GameObject.DestroyImmediate(PreviewGo);
            }
            
            Clear();
            
            Model = TimelineAsset.GetAsset();

            CurrFrameCnt  = 0;
            
            VisualElement root = rootVisualElement;

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UXMLPath);
            visualTree.CloneTree(root);

            CommandDispatcher = new CommandDispatcher();
            
            InitUIEvent();
            
            CreatePartialViews();

            if (TimelineAsset.previewGo.NotNull())
            {
                PreviewGo = GameObject.Instantiate(TimelineAsset.previewGo);
            }
        }

        private void Clear()
        {
            Model = null;
            
            //事件
            rootVisualElement.UnregisterCallback<PointerDownEvent>(OnClickClipHandler);

            //UI
            if (Top != null)
                Top.Clear();
            
            if (Middle != null)
                Middle.Clear();
            
            if (BottomScroll != null)
                BottomScroll.Clear();
            
            if (Bottom != null)
                Bottom.Clear();
            
            if (RightScroll != null)
                RightScroll.Clear();
            
            if (FrameArrow != null)
                FrameArrow.Clear();
            
            //删除
            rootVisualElement.Clear();
        }
        
        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            if (PreviewGo!=null)
            {
                GameObject.DestroyImmediate(PreviewGo);
            }
        }
        
        private void OnGUI()
        {
            if (EditorWindow.focusedWindow != null)
            {
                if (EditorWindow.focusedWindow != this )
                {
                    lastFocuseWindow = EditorWindow.focusedWindow;
                }
                else
                {
                    if (lastFocuseWindow != this)
                    {
                        OnFocuseWindow?.Invoke();
                    }
                    lastFocuseWindow = EditorWindow.focusedWindow;
                }
            }
            FrameArrow.Draw();
            OnHandleEvent(Event.current);
        }

        private void Update()
        {
            if (!Application.isPlaying && IsPlaying)
            {
                float delta = 1.0f / 30;
                double fTime = (float)EditorApplication.timeSinceStartup - lastUpdateTime;

                if (fTime > delta)
                {
                    int newFrame = CurrFrameCnt + 1;
                    if (CurrFrameCnt >= TotalFrameCnt)
                    {
                        SetCurrFrame(0);  
                    }
                    else
                    {
                        SetCurrFrame(newFrame);    
                    }
                    lastUpdateTime = (float)EditorApplication.timeSinceStartup;
                }
            }
        }

        private void CreatePartialViews()
        {
            Top = PartialView.Create<TopPartialView>(this);
            Top.Init();
            
            Middle = PartialView.Create<MiddlePartialView>(this);
            Middle.Init();
            
            BottomScroll = PartialView.Create<BottomScrollPartialView>(this);
            BottomScroll.Init();
            
            Bottom = PartialView.Create<BottomPartialView>(this);
            Bottom.Init();
            
            RightScroll = PartialView.Create<RightScrollPartialView>(this);
            RightScroll.Init();
            
            FrameArrow = PartialView.Create<Frame_Arrow>(this);
            FrameArrow.Init(Middle.FrameSlider.RootElement);
        }
        
        private void InitUIEvent()
        {
            rootVisualElement.RegisterCallback<PointerDownEvent>(OnClickClipHandler);
        }
        
        private void OnClickClipHandler(PointerDownEvent evt)
        {
            LCToolkit.InspectorExtension.DrawObjectInInspector(Model);
        }
        
        public void OnHandleEvent(Event evt)
        {
            if (evt == null)
                return;
            if (Event.current.Equals(Event.KeyboardEvent("^S")))
            {
                Save();
                Event.current.Use();
            }
            if (Event.current.Equals(Event.KeyboardEvent("^Z")))
            {
                CommandDispatcher.Undo();
                Event.current.Use();
            }
            if (Event.current.Equals(Event.KeyboardEvent("^Y")))
            {
                CommandDispatcher.Redo();
                Event.current.Use();
            }
        }

        #region 公共方法

        /// <summary>
        /// 通过位置计算帧数
        /// </summary>
        /// <param name="pPosX"></param>
        /// <returns></returns>
        public int CalcFrameByPosX(float pPosX)
        {
            int frame  = (int)(pPosX / Frame_Element.FrameWidth);
            return frame;
        }
        
        /// <summary>
        /// 通过时间计算帧数
        /// </summary>
        /// <param name="pPosX"></param>
        /// <returns></returns>
        public int CalcFrameByTime(float pTime)
        {
            int frame  = (int)(pTime / FrameDeltaTime);
            return frame;
        }
        
        /// <summary>
        /// 设置当前帧
        /// </summary>
        /// <param name="pFrame"></param>
        public void SetCurrFrame(int pFrame)
        {
            if (CurrFrameCnt == pFrame)
                return;
            if (pFrame > TotalFrameCnt)
            {
                return;
            }
            CurrFrameCnt = pFrame;
            OnCurrFrameChange?.Invoke();
            rootVisualElement.MarkDirtyRepaint();
        }
        
        /// <summary>
        /// 设置总帧数
        /// </summary>
        /// <param name="pFrame"></param>
        public void SetTotalFrame(int pTotalFrame)
        {
            if (TotalFrameCnt == pTotalFrame)
                return;
            Model.totalFrame = pTotalFrame;
            OnTotalFrameChange?.Invoke();
            rootVisualElement.MarkDirtyRepaint();
        }

        /// <summary>
        /// 设置播放
        /// </summary>
        public void SetPlaying(bool pIsPlaying)
        {
            IsPlaying = pIsPlaying;
        }

        /// <summary>
        /// 刷新总帧数
        /// </summary>
        public void RefreshTotalFrame()
        {
            int totalFrame = Bottom.GetEndFrame();
            SetTotalFrame(totalFrame);
        }
        
        #endregion

        public void Save()
        {
            Debug.Log("保存");
            TimelineAsset.Export(Model);
            ReloadUI();
        }
    }
}