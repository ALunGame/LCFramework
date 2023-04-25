using UnityEngine.UIElements;

namespace LCSkill.Timeline
{
    public class RightScrollPartialView : PartialView
    {
        public VisualElement RootElement { get; private set; }
        public ScrollView RightScroll { get; private set; }
        
        public void Init()
        {
            RootElement    = window.rootVisualElement.Q<VisualElement>("RightScrollBox");
            RightScroll   = RootElement.Q<ScrollView>("RightScroll");
            
            RightScroll.verticalScroller.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                window.Bottom.ScrolleTracks(evt.newValue);
            });
            CreateBox();
            
            window.rootVisualElement.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnScrollersGeometryChanged));

            window.Bottom.OnTrackGroupChange += () =>
            {
                CreateBox();
            };
        }
        
        public void CreateBox()
        {
            RightScroll.Clear();

            for (int i = 0; i < window.Model.trackGroups.Count; i++)
            {
                BaseTrackGroup group = window.Model.trackGroups[i];
                VisualElement box = new VisualElement();
                box.style.width = 0;
                box.style.height = InternalTrackGroup_Element.TrackGroupHeight;
                RightScroll.Add(box);

                for (int j = 0; j < group.tracks.Count; j++)
                {
                    VisualElement trackBox = new VisualElement();
                    trackBox.style.width = 0;
                    trackBox.style.height = InternalTrack_Element.TrackHeight;

                    RightScroll.Add(trackBox);
                }
            }
        }
        
        private void OnScrollersGeometryChanged(GeometryChangedEvent evt)
        {
            float value = RightScroll.verticalScroller.value;
            window.Bottom.ScrolleTracks(value);
        }
    }
}