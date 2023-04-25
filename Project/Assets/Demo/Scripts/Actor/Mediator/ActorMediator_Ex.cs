using System;
using Demo;
using LCMap;
using LCSkill;

namespace LCMap
{
    /// <summary>
    /// 演员逻辑层中介
    /// </summary>
    public static partial class ActorMediator
    {
        /// <summary>
        /// 演员释放技能
        /// </summary>
        /// <param name="pActor"></param>
        /// <param name="pSkillId"></param>
        /// <returns></returns>
        public static bool ReleaseSkill(Actor pActor, int pSkillId, Action pSkillFinsihCallBack = null)
        {
            if (pActor == null)
            {
                return false;
            }

            if (Requeset(pActor,ActorRequestType.Skill,pSkillFinsihCallBack,pSkillId))
            {
                return true;
            }

            return false;
        }

    }
}