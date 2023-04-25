using System.Collections.Generic;
using LCToolkit.Element;
using UnityEngine.UIElements;

namespace LCSkill.Timeline
{
    public class FrameSlider_Element : PartialView
    {
        public ScrollView RootElement { get; private set; }
        
        private List<Frame_Element> FrameElements = new List<Frame_Element>();

        public void Init(MiddlePartialView pMiddlePartialView)
        {
            RootElement   = pMiddlePartialView.RootElement.Q<ScrollView>("Framelist");
            RootElement.BanScrollViewScroll();
            
            CreateFrames(SkillTimelineWindow.MaxFrameCnt);
        }
        
        public void CreateFrames(int pFrameCnt)
        {
            RootElement.Clear();
            
            for (int i = 0; i < pFrameCnt; i++)
            {
                Frame_Element frameElement = PartialView.Create<Frame_Element>(window);
                frameElement.Init();
                
                VisualElement item = frameElement.RootElement;
                Label lable = item.Q<Label>("frame");
                lable.text = i.ToString();
                RootElement.Add(item);
                FrameElements.Add(frameElement);
            }
        }
        
        public void ScrolleFrames(float pValue)
        {
            RootElement.horizontalScroller.value = pValue;
        }
    }
}