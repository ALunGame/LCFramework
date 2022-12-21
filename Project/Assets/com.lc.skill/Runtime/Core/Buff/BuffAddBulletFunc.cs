using System.Collections.Generic;

namespace LCSkill
{
    /// <summary>
    /// Buff生命周期添加Bullet
    /// </summary>
    public class BuffLifeCycleAddBulletFunc : BuffLifeCycleFunc
    {
        public AddBulletModel addBullet;

        public override void Execute(BuffObj buff, int modifyStack = 0)
        {
            SkillLocate.Skill.CreateBullet(buff.ower, addBullet);
        }
    }
}