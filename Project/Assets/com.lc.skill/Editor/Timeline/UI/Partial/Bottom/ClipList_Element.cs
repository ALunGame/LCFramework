using System.Collections.Generic;
using LCSkill.Timeline;
using LCToolkit.Element;
using UnityEngine;
using UnityEngine.UIElements;

namespace LCSkill.Timeline
{
    public class ClipList_Element : PartialView
    {
        public VisualElement RootElement { get;  set; }
        
        private List<InternalClip_Element> cliplist = new List<InternalClip_Element>();

        public int StartFrame { get;  private set; }
        public int EndFrame { get;  private set; }

        public InternalTrack_Element trackElement;
        
        public DropdownMenu RightClickMenu { get;  private set; }

        #region Init
        public void SetUp(InternalTrack_Element pTrackElement)
        {
            trackElement = pTrackElement;
            
            //创建UI元素
            CreateElement();

            //数据绑定
            BindProperties();

            //UI事件
            InitUIEvent();
            
            //子类初始化
            OnInit();
            
            //事件
            // window.OnTotalFrameChange += () =>
            // {
            //     RootElement.style.width  = window.BottomScroll.CurrCreateFrameCnt * Frame_Element.FrameWidth;
            // };
        }
        
        #endregion
        
        #region Clear

        public override void OnClear()
        {
            UnBindProperties();
        }

        #endregion
        
        #region 数据监听

        private void BindProperties()
        {
            
        }
        
        private void UnBindProperties()
        {
            
        }

        #endregion

        #region 数据改变
        
        #endregion

        #region UI元素

        private void CreateElement()
        {
            RootElement = new VisualElement();
            RootElement.name = "cliplistBox";
            RootElement.style.height = InternalTrack_Element.TrackHeight;
            RootElement.style.width  = SkillTimelineWindow.MaxFrameCnt * Frame_Element.FrameWidth;
            RootElement.style.borderBottomColor = new StyleColor(Color.black);
            RootElement.style.borderBottomWidth = 1;
            RootElement.style.flexDirection = FlexDirection.Row;
        }

        #endregion

        #region UI事件

        private void InitUIEvent()
        {
            RootElement.RegisterCallback<PointerDownEvent>(OnClickClipHandler);
        }
        
        private void OnClickClipHandler(PointerDownEvent evt)
        {
            LCToolkit.InspectorExtension.DrawObjectInInspector(trackElement.OwerModel);
            evt.StopImmediatePropagation();
        }
        
        #endregion

        protected virtual void OnInit()
        {
            
        }

        public void CreateClipElement(BaseClip pClip)
        {
            InternalClip_Element clipElement = PartialView.Create(window,TimelineUtil.GetClipElementType(pClip)) as InternalClip_Element;
            clipElement.SetUp(pClip,this);
            RootElement.Add(clipElement.RootElement);
            cliplist.Add(clipElement);
        }

        public void RemoveClipElement(InternalClip_Element clipElement)
        {
            if (!cliplist.Contains(clipElement))
            {
                return;
            }
            cliplist.Remove(clipElement);
            RootElement.Remove(clipElement.RootElement);
            clipElement.Clear();
        }

        public void RemoveClip(InternalClip_Element clipElement)
        {
            trackElement.RemoveClip(clipElement);
        }

        public InternalClip_Element GetClipElemnet(BaseClip pClip)
        {
            for (int i = 0; i < cliplist.Count; i++)
            {
                if (cliplist[i].OwerModel.Equals(pClip))
                {
                    return cliplist[i];
                }
            }

            return null;
        }
        
        public void UpdateFrame()
        {
            int oldStartFrame = StartFrame;
            int oldEndFrame = EndFrame;
            
            StartFrame = SkillTimelineWindow.MaxFrameCnt;
            EndFrame   = 0;
            
            foreach (InternalClip_Element clipElement in cliplist)
            {
                if (clipElement.OwerModel.startFrame < StartFrame)
                {
                    StartFrame = clipElement.OwerModel.startFrame;
                }

                if (clipElement.OwerModel.endFrame > EndFrame)
                {
                    EndFrame = clipElement.OwerModel.endFrame;
                }
            }

            if (EndFrame != oldEndFrame)
            {
                window.RefreshTotalFrame();
            }
        }
    }
}