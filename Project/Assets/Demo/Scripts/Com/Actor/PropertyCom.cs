using LCECS.Core;

namespace Demo.Com
{
    /// <summary>
    /// 玩家属性组件
    /// </summary>
    public class PlayerPropertyCom : BaseCom
    {
        /// <summary>
        /// 魔法之类的
        /// </summary>
        public PropertyInfo Mp;
        /// <summary>
        /// 跳跃速度
        /// </summary>
        public PropertyInfo JumpSpeed = new PropertyInfo(10,5,10);
        /// <summary>
        /// 爬墙速度
        /// </summary>
        public PropertyInfo ClimbSpeed = new PropertyInfo(5, 2, 3);
    }
}