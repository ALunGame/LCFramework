using LCMap;

namespace LCSkill
{
    /// <summary>
    /// Bullet击中添加Bullet
    /// </summary>
    public class BulletHitAddBulletFunc : BulletHitFunc
    {
        public AddBulletModel addBullet;

        public override void Execute(BulletObj bullet, Actor actor)
        {
            SkillLocate.Skill.CreateBullet(bullet.ower,addBullet);
        }
    }
}