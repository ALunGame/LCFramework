using System.Collections.Generic;
using LCToolkit.Element;
using UnityEngine;
using UnityEngine.UIElements;

namespace LCSkill.Timeline
{
    public class MiddlePartialView : PartialView
    {
        public VisualElement RootElement { get; private set; }
        
        public FrameSlider_Element FrameSlider { get; private set; }
        
        public PlayArea_Element PlayArea { get; private set; }
        
        public void Init()
        {
            RootElement = window.rootVisualElement.Q<VisualElement>("Middle");
            
            FrameSlider = PartialView.Create<FrameSlider_Element>(window);
            FrameSlider.Init(this);
            
            PlayArea = PartialView.Create<PlayArea_Element>(window);
            PlayArea.Init(this);
        }
        
        public void ScrolleFrames(float pValue)
        {
            FrameSlider.ScrolleFrames(pValue);
        }
    }
}