using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using LCToolkit;
using LCToolkit.Element;

namespace LCSkill.Timeline
{
    public class BottomPartialView : PartialView
    {
        public VisualElement RootElement { get; private set; }
        
        internal ScrollView trackScroll    { get; private set; }
        internal ScrollView clipAreaScroll { get; private set; }
        internal List<InternalTrackGroup_Element> groupElements = new List<InternalTrackGroup_Element>();

        public event Action OnTrackGroupChange;
        
        public void Init()
        {
            //UI元素
            RootElement = window.rootVisualElement.Q<VisualElement>("Bottom");
            trackScroll    = RootElement.Q<ScrollView>("Tracklist");
            trackScroll.BanScrollViewScroll();
            clipAreaScroll = RootElement.Q<ScrollView>("ClipScrollArea");
            clipAreaScroll.BanScrollViewScroll();
            
            //创建轨道组
            for (int i = 0; i < window.Model.trackGroups.Count; i++)
            {
                CreateTrackGroupElement(window.Model.trackGroups[i]);
            }
        }

        public override void OnClear()
        {
            OnTrackGroupChange = null;
        }

        public void AddTrackGroup(BaseTrackGroup pGroup)
        {
            foreach (BaseTrackGroup group in window.Model.trackGroups)
            {
                if (group.GetType() == pGroup.GetType())
                {
                    return;
                }
            }
            
            window.Model.trackGroups.Add(pGroup);
            CreateTrackGroupElement(pGroup);
            
            OnTrackGroupChange?.Invoke();
        }

        public void RemoveTrackGroup(InternalTrackGroup_Element pElement)
        {
            if (!groupElements.Contains(pElement))
            {
                return;
            }

            groupElements.Remove(pElement);
            trackScroll.Remove(pElement.TrackAreaElement);
            clipAreaScroll.Remove(pElement.ClipAreaElement);

            window.Model.trackGroups.Remove(pElement.OwerModel);
            
            OnTrackGroupChange?.Invoke();
        }

        private void CreateTrackGroupElement(BaseTrackGroup pGroup)
        {
            InternalTrackGroup_Element element = PartialView.Create(window, TimelineUtil.GetTrackGroupElementType(pGroup)) as InternalTrackGroup_Element;
            element.SetUp(pGroup,this);
            groupElements.Add(element);
        }

        public void RefreshTracks()
        {
            OnTrackGroupChange?.Invoke();
        }

        public int GetEndFrame()
        {
            int endFrame = 0;
            foreach (InternalTrackGroup_Element trackElement in groupElements)
            {
                int tEndFrame = trackElement.GetEndFrame();
                if (tEndFrame > endFrame)
                {
                    endFrame = tEndFrame;
                }
            }

            return endFrame;
        }
        
        public void ScrolleClips(float pValue)
        {
            clipAreaScroll.horizontalScroller.value = pValue;
        }
        
        public void ScrolleTracks(float pValue)
        {
            trackScroll.verticalScroller.slider.value = pValue;
            clipAreaScroll.verticalScroller.slider.value = pValue;
        }
    }
}