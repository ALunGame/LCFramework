using LCSkill;
using System;

namespace Demo
{
    public class SkillServer : ISkillServer
    {
        public AoeObj CreateAoe(SkillCom ower, AddAoeModel addAoe)
        {
            AoeCom aoeCom = LCECS.ECSLocate.ECS.GetWorld().GetCom<AoeCom>();
            AoeObj aoeObj = new AoeObj();
            return aoeObj;
        }

        public BuffObj CreateBuff(SkillCom ower, SkillCom target, AddBuffModel addBuff)
        {
            throw new NotImplementedException();
        }

        public BulletObj CreateBullet(SkillCom ower, AddBulletModel addBullet)
        {
            throw new NotImplementedException();
        }

        public void LearnSkill(SkillCom target, SkillModel skillModel, int level = 1)
        {
            throw new NotImplementedException();
        }

        public bool ReleaseSkill(SkillCom target, int skillId)
        {
            throw new NotImplementedException();
        }
    }
}
