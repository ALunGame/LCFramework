using LCJson;
using LCLoad;
using System.Collections.Generic;

namespace LCSkill
{
    public class SkillModelServer : ISkillModelServer
    {
        private Dictionary<string, SkillModel> skillModelCache = new Dictionary<string, SkillModel>();
        private Dictionary<string, TimelineModel> timelineModelCache = new Dictionary<string, TimelineModel>();
        private Dictionary<string, BuffModel> buffModelCache = new Dictionary<string, BuffModel>();
        private Dictionary<string, AoeModel> aoeModelCache = new Dictionary<string, AoeModel>();
        private Dictionary<string, BulletModel> bulletModelCache = new Dictionary<string, BulletModel>();

        public SkillModel GetSkillModel(string skillId)
        {
            if (skillModelCache.ContainsKey(skillId))
                return skillModelCache[skillId];
            string assetName = SkillDef.GetSkillCnfName(skillId);
            string jsonStr = LoadHelper.LoadString(assetName);
            if (string.IsNullOrEmpty(jsonStr))
            {
                return default;
            }
            return JsonMapper.ToObject<SkillModel>(jsonStr);    
        }

        public TimelineModel GetTimelineModel(string timelineName)
        {
            if (timelineModelCache.ContainsKey(timelineName))
                return timelineModelCache[timelineName];
            string assetName = SkillDef.GetTimelineCnfName(timelineName);
            string jsonStr = LoadHelper.LoadString(assetName);
            if (string.IsNullOrEmpty(jsonStr))
            {
                return default;
            }
            return JsonMapper.ToObject<TimelineModel>(jsonStr);
        }

        public BuffModel GetBuffModel(string buffId)
        {
            if (buffModelCache.ContainsKey(buffId))
                return buffModelCache[buffId];
            string assetName = SkillDef.GetBuffCnfName(buffId);
            string jsonStr = LoadHelper.LoadString(assetName);
            if (string.IsNullOrEmpty(jsonStr))
            {
                return default;
            }
            return JsonMapper.ToObject<BuffModel>(jsonStr);
        }

        public AoeModel GetAoeModel(string aoeId)
        {
            if (aoeModelCache.ContainsKey(aoeId))
                return aoeModelCache[aoeId];
            string assetName = SkillDef.GetAoeCnfName(aoeId);
            string jsonStr = LoadHelper.LoadString(assetName);
            if (string.IsNullOrEmpty(jsonStr))
            {
                return default;
            }
            return JsonMapper.ToObject<AoeModel>(jsonStr);
        }

        public BulletModel GetBulletModel(string bulletId)
        {
            if (bulletModelCache.ContainsKey(bulletId))
                return bulletModelCache[bulletId];
            string assetName = SkillDef.GetBulletCnfName(bulletId);
            string jsonStr = LoadHelper.LoadString(assetName);
            if (string.IsNullOrEmpty(jsonStr))
            {
                return default;
            }
            return JsonMapper.ToObject<BulletModel>(jsonStr);
        }
    }
}
