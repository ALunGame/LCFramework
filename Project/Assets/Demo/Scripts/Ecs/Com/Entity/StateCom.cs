

using LCECS.Core.ECS;

namespace Demo.Com
{
    public enum EntityState
    {
        Normal,             //正常
        Stop,               //僵直
        Dead,               //死亡
    }

    [Com(GroupName = "Entity", ViewName = "状态组件")]
    public class StateCom : BaseCom
    {
        [ComValue]
        public EntityState CurState = EntityState.Normal;
    }
}
