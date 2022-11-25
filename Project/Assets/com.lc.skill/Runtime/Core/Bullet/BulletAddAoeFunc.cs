using LCMap;

namespace LCSkill
{
    /// <summary>
    /// Bullet击中添加Aoe
    /// </summary>
    public class BulletHitAddAoeFunc : BulletHitFunc
    {
        public AddAoeModel addAoe;

        public override void Execute(BulletObj bullet, Actor actor)
        {
            SkillLocate.Skill.CreateAoe(bullet.ower, addAoe);
        }
    }
}