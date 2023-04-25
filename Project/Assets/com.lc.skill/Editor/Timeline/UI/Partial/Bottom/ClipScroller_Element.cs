using UnityEngine;
using UnityEngine.UIElements;

namespace LCSkill.Timeline
{
    public class ClipScroller_Element : PartialView
    {
        private Scroller clipScroller;
        
        public void Init(VisualElement pParentElement)
        {
            clipScroller = pParentElement.Q<Scroller>("ClipScroller");
            clipScroller.highValue = 1;
            clipScroller.lowValue = 0;
            clipScroller.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnScrollersGeometryChanged));
            clipScroller.slider.Q<VisualElement>("unity-dragger").RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnHorizontalScrollDragElementChanged));
            
            clipScroller.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                window.Middle.ScrolleFrames(evt.newValue);
            });
        }

        private void OnHorizontalScrollDragElementChanged(GeometryChangedEvent evt)
        {
            Debug.LogError("OnHorizontalScrollDragElementChanged");
        }

        private void OnScrollersGeometryChanged(GeometryChangedEvent evt)
        {
            Debug.LogError("OnScrollersGeometryChanged");
        }
    }
}