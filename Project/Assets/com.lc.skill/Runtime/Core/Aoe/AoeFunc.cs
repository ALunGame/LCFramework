using LCMap;
using System.Collections.Generic;

namespace LCSkill
{
    /// <summary>
    /// Aoe生命周期函数
    /// 创建，移除，OnTick
    /// </summary>
    public abstract class AoeLifeCycleFunc
    {
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="aoe">aoe对象</param>
        public abstract void Execute(AoeObj aoe);
    }

    /// <summary>
    /// Aoe移动方式函数
    /// 有函数Aoe才可以移动
    /// </summary>
    public abstract class AoeMoveFunc
    {
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="aoe">aoe对象</param>
        /// <param name="moveTime">已经移动时间</param>
        public abstract AoeMoveInfo Execute(AoeObj aoe, float moveTime);
    }

    #region 演员

    /// <summary>
    /// 当有新的演员进入范围
    /// </summary>
    public abstract class AoeActorEnter
    {
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="aoe">aoe对象</param>
        /// <param name="actors">新进入的演员</param>
        public abstract void Execute(AoeObj aoe, List<ActorObj> actors);
    }

    /// <summary>
    /// 当有演员离开范围
    /// </summary>
    public abstract class AoeActorLeave
    {
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="aoe">aoe对象</param>
        /// <param name="actors">离开的演员</param>
        public abstract void Execute(AoeObj aoe, List<ActorObj> actors);
    }

    #endregion

    #region 子弹

    /// <summary>
    /// 当有新的子弹进入范围
    /// </summary>
    public abstract class AoeBulletEnter
    {
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="aoe">aoe对象</param>
        /// <param name="bullets">新进入的演员</param>
        public abstract void Execute(AoeObj aoe, List<BulletObj> bullets);
    }

    /// <summary>
    /// 当有子弹离开范围
    /// </summary>
    public abstract class AoeBulletLeave
    {
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="aoe">aoe对象</param>
        /// <param name="bullets">离开的子弹</param>
        public abstract void Execute(AoeObj aoe, List<BulletObj> bullets);
    }

    #endregion
}
