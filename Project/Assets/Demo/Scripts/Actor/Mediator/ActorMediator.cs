using LCMap;
using LCSkill;

namespace Demo.Mediator
{
    /// <summary>
    /// 演员逻辑层中介
    /// </summary>
    public static class ActorMediator
    {
        
        public static Actor GetActor(string pUid)
        {
            return LCMap.MapLocate.Map.GetActor(pUid);
        }
        
        public static Actor GetActor(int pActorId)
        {
            return LCMap.MapLocate.Map.GetActor(pActorId);
        }
        
        /// <summary>
        /// 演员释放技能
        /// </summary>
        /// <param name="pActor"></param>
        /// <param name="pSkillId"></param>
        /// <returns></returns>
        public static bool ReleaseSkill(Actor pActor, int pSkillId)
        {
            if (pActor == null)
            {
                return false;
            }

            SkillCom skillCom = pActor.GetCom<SkillCom>();
            return skillCom.ReleaseSkill(pSkillId.ToString());
        }
        
    }
}