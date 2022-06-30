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

        private DayNightCom dayNightCom;
        public DayNightCom GetDayNightCom()
        {
            if (dayNightCom == null)
                dayNightCom = ECSLocate.ECS.GetWorld().GetCom<DayNightCom>();
            return dayNightCom;
        }

        /// <summary>
        /// 获得当前昼夜阶段
        /// </summary>
        /// <returns></returns>
        public DayNightStage CurrDayNightStage()
        {
            return GetDayNightCom().GetStage();
        }
    }
}
