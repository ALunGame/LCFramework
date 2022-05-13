using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCSkill
{
    public interface ISkillModelServer
    {
        public SkillModel GetSkillModel(string skillId);

        public TimelineModel GetTimelineModel(string timelineName);

        public BuffModel GetBuffModel(string buffId);

        public AoeModel GetAoeModel(string aoeId);

        public BulletModel GetBulletModel(string bulletId);
    }
}
