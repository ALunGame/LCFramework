using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCSkill
{
    /// <summary>
    /// 技能Timeline函数
    /// </summary>
    public abstract class TimelineFunc
    {
        /// <summary>
        /// 节点运行时间
        /// </summary>
        public float timeElapsed;

        public abstract void Execute(TimelineObj timelineObj);
    }

    /// <summary>
    /// 创建Aoe
    /// </summary>
    public class SkillTimeline_CreateAoe : TimelineFunc
    {
        public string aoeId;

        public override void Execute(TimelineObj timelineObj)
        {
            
        }
    }

    /// <summary>
    /// 创建子弹
    /// </summary>
    public class SkillTimeline_CreateBullet : TimelineFunc
    {
        public string bulletId;

        public override void Execute(TimelineObj timelineObj)
        {
            
        }
    }
}
