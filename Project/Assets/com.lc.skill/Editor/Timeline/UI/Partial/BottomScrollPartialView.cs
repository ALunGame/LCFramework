using UnityEngine;
using UnityEngine.UIElements;

namespace LCSkill.Timeline
{
    /// <summary>
    /// 底部拖动条
    /// </summary>
    public class BottomScrollPartialView : PartialView
    {
        public VisualElement RootElement { get; private set; }
        public ScrollView BottomScroll { get; private set; }

        private int NeedCreateFrameCnt
        {
            get
            {
                return window.TotalFrameCnt + 2;
            }
        }

        public int CurrCreateFrameCnt
        {
            get
            {
                return BottomScroll.childCount;
            }
        }
        
        public void Init()
        {
            RootElement    = window.rootVisualElement.Q<VisualElement>("BottomScroll");
            BottomScroll   = RootElement.Q<ScrollView>("Scroll");
            
            BottomScroll.horizontalScroller.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                window.Middle.ScrolleFrames(evt.newValue);
                window.Bottom.ScrolleClips(evt.newValue);
            });
            CreateBox(NeedCreateFrameCnt);
            
            window.rootVisualElement.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnScrollersGeometryChanged));

            window.OnTotalFrameChange += () =>
            {
                //超过在创建
                if (NeedCreateFrameCnt > CurrCreateFrameCnt)
                {
                    CreateBox(NeedCreateFrameCnt);
                }
            };
        }

        
        public void CreateBox(int pCnt)
        {
            BottomScroll.Clear();
            
            for (int i = 0; i < pCnt; i++)
            {
                VisualElement box = new VisualElement();
                box.style.width = Frame_Element.FrameWidth;
                box.style.height = 0;

                BottomScroll.Add(box);
            }
        }
        
        private void OnScrollersGeometryChanged(GeometryChangedEvent evt)
        {
            float value = BottomScroll.horizontalScroller.value;
            window.Middle.ScrolleFrames(value);
            window.Bottom.ScrolleClips(value);
        }
    }
}