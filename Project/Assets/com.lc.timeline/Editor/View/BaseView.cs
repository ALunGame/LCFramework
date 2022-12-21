using System.Collections;
using UnityEngine;

namespace LCTimeline
{
    public class BaseView
    {
        protected TimelineWindow window;
        public BaseTimelineGraph Model
        {
            get;
            protected set;
        }

        public void Init(TimelineWindow editorWindow, BaseTimelineGraph model)
        {
            this.window = editorWindow;
            this.Model = model;
            OnInit();
        }

        public void SyncRunningTime(double runningTime)
        {
            OnRunningTimeChange(runningTime);
        }

        public virtual void OnInit()
        {
        }

        public virtual void OnDraw()
        {
        }

        public virtual void OnDestroy()
        {
        }

        public virtual void OnHandleEvent(Event evt)
        {
        }

        public virtual void OnRunningTimeChange(double runningTime)
        {
        }
    }
}