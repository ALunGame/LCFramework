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

        public bool GetSkillModel(string skillId, out SkillModel model)
        {
            if (skillModelCache.ContainsKey(skillId))
            {
                model = skillModelCache[skillId];
                return true;
            }
            string jsonStr = LoadHelper.LoadString(SkillDef.GetSkillCnfName(skillId));
            if (string.IsNullOrEmpty(jsonStr))
            {
                model = default;
                return false;
            }
            model = JsonMapper.ToObject<SkillModel>(jsonStr);
            skillModelCache.Add(skillId, model);
            return true;
        }

        public bool GetTimelineModel(string timelineName, out TimelineModel model)
        {
            if (timelineModelCache.ContainsKey(timelineName))
            {
                model = timelineModelCache[timelineName];
                return true;
            }
            string jsonStr = LoadHelper.LoadString(SkillDef.GetTimelineCnfName(timelineName));
            if (string.IsNullOrEmpty(jsonStr))
            {
                model = default;
                return false;
            }
            model = JsonMapper.ToObject<TimelineModel>(jsonStr);
            timelineModelCache.Add(timelineName, model);
            return true;
        }

        public bool GetAoeModel(string aoeId, out AoeModel model)
        {
            if (aoeModelCache.ContainsKey(aoeId))
            {
                model = aoeModelCache[aoeId];
                return true;
            }
            string jsonStr = LoadHelper.LoadString(SkillDef.GetAoeCnfName(aoeId));
            if (string.IsNullOrEmpty(jsonStr))
            {
                model = default;
                return false;
            }
            model = JsonMapper.ToObject<AoeModel>(jsonStr);
            aoeModelCache.Add(aoeId, model);
            return true;
        }

        public bool GetBuffModel(string buffId, out BuffModel model)
        {
            if (buffModelCache.ContainsKey(buffId))
            {
                model = buffModelCache[buffId];
                return true;
            }
            string jsonStr = LoadHelper.LoadString(SkillDef.GetBuffCnfName(buffId));
            if (string.IsNullOrEmpty(jsonStr))
            {
                model = default;
                return false;
            }
            model = JsonMapper.ToObject<BuffModel>(jsonStr);
            buffModelCache.Add(buffId, model);
            return true;
        }

        public bool GetBulletModel(string bulletId, out BulletModel model)
        {
            if (bulletModelCache.ContainsKey(bulletId))
            {
                model = bulletModelCache[bulletId];
                return true;
            }
            string jsonStr = LoadHelper.LoadString(SkillDef.GetBulletCnfName(bulletId));
            if (string.IsNullOrEmpty(jsonStr))
            {
                model = default;
                return false;
            }
            model = JsonMapper.ToObject<BulletModel>(jsonStr);
            bulletModelCache.Add(bulletId, model);
            return true;
        }
    }
}
