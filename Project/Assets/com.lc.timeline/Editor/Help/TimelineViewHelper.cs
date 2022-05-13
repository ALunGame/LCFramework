using LCTimeline.View;
using LCToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCTimeline
{
    public static class TimelineViewHelper
    {
        private static Dictionary<Type, Type> clipViewDict = new Dictionary<Type, Type>();
        private static Dictionary<Type, Type> trackViewDict = new Dictionary<Type, Type>();

        static TimelineViewHelper()
        {
            CollectType();
        }

        private static void CollectType()
        {
            clipViewDict.Clear();
            trackViewDict.Clear();

            var viewTypeList = ReflectionHelper.GetChildTypes<BaseClipView>();
            foreach (var item in viewTypeList)
            {
                if (AttributeHelper.TryGetTypeAttribute(item, out CustomClipViewAttribute attr))
                {
                    clipViewDict.Add(attr.dataType, item);
                }
            }

            viewTypeList = ReflectionHelper.GetChildTypes<BaseTrackView>();
            foreach (var item in viewTypeList)
            {
                if (AttributeHelper.TryGetTypeAttribute(item, out CustomTrackViewAttribute attr))
                {
                    trackViewDict.Add(attr.dataType, item);
                }
            }
        }

        public static BaseClipView GetClipView(ClipModel clipData)
        {
            CollectType();
            BaseClipView clipView = null;
            if (clipViewDict.ContainsKey(clipData.GetType()))
            {
                clipView = Activator.CreateInstance(clipViewDict[clipData.GetType()], true) as BaseClipView;
            }
            else
            {
                clipView = new BaseClipView();
            }
            clipView.Clip = clipData;
            return clipView;
        }

        public static BaseTrackView GetTrackView(TrackModel trackData)
        {
            CollectType();
            BaseTrackView trackView = null;
            if (trackViewDict.ContainsKey(trackData.GetType()))
            {
                trackView = Activator.CreateInstance(trackViewDict[trackData.GetType()], true) as BaseTrackView;
            }
            else
            {
                trackView = new BaseTrackView();
            }
            trackView.Track = trackData;
            return trackView;
        }
    }
}
