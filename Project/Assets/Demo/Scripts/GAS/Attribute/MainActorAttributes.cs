using LCGAS;

namespace Demo.GAS.Attribute
{
    public static class MainActorAttributeDef
    {
        public const string SpeedRatio = "主角速度倍率";
        public const string Endurance = "主角耐力";
    }

    /// <summary>
    /// 主角速度倍率
    /// </summary>
    public class Attr_MainActorSpeedRatio : AttributeValue
    {
        public override string Name { get => MainActorAttributeDef.SpeedRatio; }

        public Attr_MainActorSpeedRatio()
        {
            CurrentValue = 1;
            BaseValue = 1;
        }
    }
    
    /// <summary>
    /// 主角耐力
    /// </summary>
    public class Attr_MainActorEndurance : AttributeValue
    {
        public override string Name { get => MainActorAttributeDef.Endurance; }

        public Attr_MainActorEndurance()
        {
            CurrentValue = 1;
            BaseValue = 1;
        }
    }
}