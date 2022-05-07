using LCECS.Layer.Info;
using LCMap;
using System.Collections.Generic;

namespace LCSkill
{
    /// <summary>
    /// Aoe世界收集器
    /// </summary>
    public abstract class AoeSensor : ISensor
    {
        /// <summary>
        /// 获取Aoe范围内的演员
        /// </summary>
        /// <param name="aoeObj"></param>
        /// <returns></returns>
        public abstract List<ActorObj> GetActorsInRange(AoeObj aoeObj);

        /// <summary>
        /// 检测演员是否在范围内
        /// </summary>
        /// <param name="aoeObj"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        public abstract bool CheckActorInRange(AoeObj aoeObj,ActorObj actor);

        /// <summary>
        /// 获取Aoe范围内的子弹
        /// </summary>
        /// <param name="aoeObj"></param>
        /// <returns></returns>
        public abstract List<BulletObj> GetBulletsInRange(AoeObj aoeObj);

        /// <summary>
        /// 检测子弹是否在范围内
        /// </summary>
        /// <param name="bulletObj"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        public abstract bool CheckBulletInRange(AoeObj aoeObj,BulletObj bullet);
    }
}
