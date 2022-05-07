using UnityEditor;
using UnityEngine;

namespace LCTimeline
{
    public class TimelineStyle
    {
        private static readonly string EditImgPath = @"Assets/com.lc.timeline/Editor/Images/";

        public static Texture2D LoadEdStyleImg(string imgName)
        {
            return AssetDatabase.LoadAssetAtPath<Texture2D>(EditImgPath + imgName);
        }
    }
}
