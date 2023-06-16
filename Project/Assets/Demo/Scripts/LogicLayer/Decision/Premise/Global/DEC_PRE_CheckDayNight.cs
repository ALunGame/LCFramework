using Demo.Com;
using LCECS.Core.Tree.Base;
using System;

namespace Demo.Decision
{
    /// <summary>
    /// 检测昼夜阶段
    /// </summary>
    public class DEC_PRE_CheckDayNightStage : NodePremise
    {
        [NonSerialized]
        private GlobalSensor globalSensor;

        public DayNightStage checkStage;

        public override bool OnMakeTrue(NodeData wData)
        {
            if (globalSensor == null)
                globalSensor = LCECS.ECSLayerLocate.Info.GetSensor<GlobalSensor>(LCECS.SensorType.Global);
            return globalSensor.CurrDayNightStage() == checkStage;
        }
    }
}
