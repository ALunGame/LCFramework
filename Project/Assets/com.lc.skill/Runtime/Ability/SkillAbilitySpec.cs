using Demo.Config;
using LCGAS;
using LCSkill.Timeline;

namespace LCSkill
{
    public class ReleaseSkillInfo
    {
        public int skillId;
        public int skillLv;
        
        public SkillCnf Cnf { get; private set; }

        public SkillTimelineName TimelineName { get; private set; }

        public ReleaseSkillInfo(int pSkillId, int pSkillLv = 1)
        {
            skillId = pSkillId;
            skillLv = pSkillLv;

            Cnf = LCConfig.Config.SkillCnf[pSkillId];
            TimelineName = new SkillTimelineName();
            TimelineName.Name = Cnf.timeline;
        }

        public override string ToString()
        {
            return $"Id:{skillId} Lv:{skillLv}";
        }
    }
    
    public class SkillAbilitySpec : GameplayAbilitySpec
    {
        public SkillAbilitySpec(AbilitySystemCom pOwnerCom, GameplayAbility pModel) : base(pOwnerCom, pModel)
        {
        }


        private SkillTimelineSpec timelineSpec;
        
        public ReleaseSkillInfo CurrReleaseSkillInfo { get; private set; }
        
        protected override void PreActive(params object[] pParams)
        {
            CurrReleaseSkillInfo = pParams[0] as ReleaseSkillInfo;
        }

        protected override void OnActive(params object[] pParams)
        {
            timelineSpec = new SkillTimelineSpec(CurrReleaseSkillInfo.TimelineName.GetModel(), this);
            timelineSpec.Start(OnTimelineFinish);
        }

        private void OnTimelineFinish()
        {
            timelineSpec = null;
            EndActive();
        }
        
        protected override void OnEndActive()
        {
            //被其他打断
            if (timelineSpec != null)
            {
                timelineSpec.End();
            }
        }
    }
}