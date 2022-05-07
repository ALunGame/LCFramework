using LCMap;

namespace LCSkill
{

    /// <summary>
    /// Bullet生命周期函数
    /// 创建，移除
    /// </summary>
    public abstract class BulletLifeCycleFunc
    {
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="bullet">aoe对象</param>
        public abstract void Execute(BulletObj bullet);
    }

    /// <summary>
    /// 子弹移动函数
    /// </summary>
    public abstract class BulletMoveFunc
    {
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="bullet">bullet对象</param>
        /// <param name="moveTime">bullet移动时间</param>
        /// <param name="target">bullet跟踪对象,如果不是跟踪不使用</param>
        public abstract BulletMoveInfo Execute(BulletObj bullet, float moveTime, ActorObj target = null);
    }


    /// <summary>
    /// 子弹在发射瞬间，可以捕捉一个演员作为目标
    /// </summary>
    public abstract class BulletCatchActorFunc
    {
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="bullet">bullet对象</param>
        public abstract ActorObj Execute(BulletObj bullet);
    }

    /// <summary>
    /// 子弹命中目标函数
    /// </summary>
    public abstract class BulletHitFunc
    {
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="bullet">bullet对象</param>
        public abstract void Execute(BulletObj bullet, ActorObj actor);
    }
}
