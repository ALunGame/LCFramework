using LCGAS;

namespace Demo.GAS.Attribute
{
    public static partial class AttributeDef
    {
        public const string CurrHP  = "当前血量";
        public const string MaxHP   = "最大血量";
        
        public const string Attack  = "攻击力";
        public const string Defense = "防御力";
        
        public const string Speed = "移动速度";
    }
    
    /// <summary>
    /// 当前血量
    /// </summary>
    public class Attr_ActorCurrHP : AttributeValue
    {
        public override string Name { get => AttributeDef.CurrHP; }

        public Attr_ActorCurrHP()
        {
            CurrentValue = 1;
            BaseValue = 1;
        }
    }
    
    /// <summary>
    /// 最大血量
    /// </summary>
    public class Attr_ActorMaxHP : AttributeValue
    {
        public override string Name { get => AttributeDef.MaxHP; }

        public Attr_ActorMaxHP()
        {
            CurrentValue = 1;
            BaseValue = 1;
        }
    }

    /// <summary>
    /// 攻击力
    /// </summary>
    public class Attr_ActorAttack : AttributeValue
    {
        public override string Name { get => AttributeDef.Attack; }
    }
    
    /// <summary>
    /// 防御力
    /// </summary>
    public class Attr_ActorDefense : AttributeValue
    {
        public override string Name { get => AttributeDef.Defense; }
    }
    
    /// <summary>
    /// 移动速度
    /// </summary>
    public class Attr_ActorSpeed : AttributeValue
    {
        public override string Name { get => AttributeDef.Speed; }
    }
}