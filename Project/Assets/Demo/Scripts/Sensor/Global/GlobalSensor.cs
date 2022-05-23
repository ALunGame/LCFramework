using Demo.Com;
using LCECS;
using LCECS.Layer.Info;
using LCMap;
using LCToolkit;

namespace Demo
{
    [WorldSensor(SensorType.Global)]
    public class GlobalSensor : ISensor
    {
        /// <summary>
        /// 当前所在区域
        /// </summary>
        public BindableValue<MapArea> CurrArea = new BindableValue<MapArea>();

        /// <summary>
        /// 当前跟随演员
        /// </summary>
        public BindableValue<ActorObj> FollowActor = new BindableValue<ActorObj>();
    }
}
