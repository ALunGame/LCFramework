using System.Collections;
using UnityEngine;

namespace LCTimeline.Player
{
    public class BasePlayer
    {
        private BaseTimelineView view;

        protected BaseTimelineView View
        {
            get { return view; }
        }

        public BasePlayer(BaseTimelineView view)
        {
            this.view = view;
        }

        public virtual void OnRunningTimeChange(double runningTime)
        {
        }
    }
}