using System.Collections.Generic;
using LCGAS;
using LCToolkit;

namespace LCSkill.Timeline
{
    [GroupAssetTypeNameAttribute("LCSkill.Timeline.SkillTimelineGroupAsset")]
    public class SkillTimelineName : GroupChildAssetName
    {
        private static Dictionary<string, BaseTimeline> modelDict = new Dictionary<string, BaseTimeline>();

        public BaseTimeline GetModel()
        {
            if (modelDict.ContainsKey(Name))
            {
                return modelDict[Name];
            }

            string str = IAFramework.GameContext.Asset.LoadString(Name);
            BaseTimeline timeline = LCJson.JsonMapper.ToObject<BaseTimeline>(str);
            modelDict.Add(Name,timeline);
            return timeline;
        }
    }
}