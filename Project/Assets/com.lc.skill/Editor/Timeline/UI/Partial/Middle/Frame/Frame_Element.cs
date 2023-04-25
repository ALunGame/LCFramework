using UnityEngine.UIElements;
using UnityEngine;

namespace LCSkill.Timeline
{
    public class Frame_Element : PartialView
    {
        public const int FrameWidth = 30;

        public VisualElement RootElement { get; private set; }
        
        public void Init()
        {
            RootElement = new VisualElement();
            RootElement.style.width = FrameWidth;
            RootElement.style.height = Length.Percent(100);
            RootElement.style.borderRightColor = new StyleColor(Color.black);
            RootElement.style.borderRightWidth = 1;
            
            Label lable = new Label();
            lable.name = "frame";
            lable.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.LowerCenter);
            lable.style.width = Length.Percent(100);
            lable.style.height = Length.Percent(100);
            
            RootElement.Add(lable);
        }
    }
}