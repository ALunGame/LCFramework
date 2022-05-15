using LCMap;
using System.Collections.Generic;

namespace LCSkill
{
    /// <summary>
    /// Bullet击中造成伤害
    /// </summary>
    public class BulletHitAddBuffFunc : BulletHitFunc
    {
        public AddBuffModel addBuff;

        public override void Execute(BulletObj bullet, ActorObj actor)
        {

        }
    }
}