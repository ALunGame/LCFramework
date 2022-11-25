using LCECS.Layer.Info;
using LCMap;
using System.Collections.Generic;

namespace LCSkill
{
    public abstract class BaseBulletSensor : ISensor
    {
        /// <summary>
        /// 获得Bullet击中的演员
        /// </summary>
        /// <param name="bulletObj"></param>
        /// <returns></returns>
        public abstract List<Actor> GetHitActors(BulletObj bulletObj);
    }
}